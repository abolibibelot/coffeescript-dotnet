using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ncoffee;
using Composable.System;
using Composable.System.IO;

namespace CoffeeScript.Compiler
{
    public class Compiler
    {
        public void Compile(CompilerOptions opt)
        {
            if (!File.Exists(opt.Path) && !Directory.Exists(opt.Path))
            {
                throw new SourceNotFound(opt.Path);
            }

            if (opt.OutputDir.IsNullOrWhiteSpace())
            {
                opt.OutputDir = Path.GetDirectoryName(opt.Path);
            }

            if(!opt.OutputDir.AsDirectory().Exists)
            {
                throw new TargetNotFound(opt.OutputDir);
            }

            IEnumerable<string> toCompile;
            if (!Directory.Exists(opt.Path) && File.Exists(opt.Path))
                toCompile = new[] { opt.Path };
            else
                toCompile = Glob(opt.Path, "*.coffee");

            if (opt.Compile)
            {
                CompileMany(toCompile, opt);                
            }

            if (opt.Watch)
            {
                StartWatching(opt);
            }

            return;
        }

        private static IEnumerable<string> Glob(string path, string pattern)
        {
            if (!Directory.Exists(path))
                yield break;
            foreach (var file in Directory.GetFiles(path, pattern))
                yield return file;
            foreach (var child in Directory.GetDirectories(path).SelectMany(dir => Glob(dir, pattern)))
                yield return child;
        }


        private static void StartWatching(CompilerOptions opt)
        {
            Console.WriteLine("watching:" + opt.Path);
            var watcher = new ChangeWatcher(opt.Path, opt,
                                (path, options) =>
                                {
                                    Console.WriteLine(DateTime.Now + ":" + path + " has changed.");
                                    CompileMany(new[] { path }, options);
                                    Console.WriteLine(DateTime.Now + ":" + path + " recompiled");
                                });

            watcher.Start();
        }



        private static void CompileMany(IEnumerable<string> toCompile, CompilerOptions opt)
        {
            foreach (var sourcePath in toCompile)
            {
                string result = Compile(sourcePath, opt);

                if (opt.Print)
                    Console.WriteLine(result);
                else
                {
                    var dest = sourcePath.Replace(opt.Path, opt.OutputDir);
                    dest = Path.ChangeExtension(dest, ".js");
                    var destDir = new FileInfo(dest).Directory;
                    if(!destDir.Exists)
                    {
                        destDir.Create();
                    }

                    File.WriteAllText(dest, result, Encoding.UTF8);
                }
            }
        }

        private static string Compile(string sourcePath, CompilerOptions opt)
        {
            //ugly hack. When triggered by the filesystemwatcher (--watch mode), the file may be transiently locked
            //by another process, and the call to Read would throw. Trying to monitor this in procmon I found at least
            //three background processes happily opening the file. We should poll and check locking beforehand, but this
            //will do for now. Wow, that's a big comment/code ratio.
            Thread.Sleep(100);

            var source = File.ReadAllText(sourcePath, Encoding.UTF8);
            string result = "";
            try
            {
                result = CoffeeScriptProcessor.Process(source, opt.Bare);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while compiling " + sourcePath + " :");
                Console.WriteLine(ex.Message);
            }
            return result;
        }
    }
}