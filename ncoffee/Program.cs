using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CoffeeScript.Compiler;
using Mono.Options;
using System.Text;

namespace ncoffee
{
    class Program
    {

        const string Version = "1.0.0";

        static void Main(string[] args)
        {
            var opt = new CompilerOptions();

            var p = new OptionSet()
                .Add("c|compile", "compile to JavaScript and save as .js files", _ => opt.Compile = true)
                .Add("o=|output=","set the directory for compiled JavaScript", d => opt.OutputDir = d )
                .Add("w|watch","watch scripts for changes, and recompile", d => opt.Watch = true )
                .Add("p|print", "print the compiled JavaScript to stdout", _ => opt.Print = true)
                .Add("b|bare", "compile without the top-level function wrapper", _ => opt.Bare = true)
                .Add("v|version", "display coffeescript version", _ => DisplayVersion())
                .Add("h|help", "display this help message", _ => opt.Help = true);

            if (args.Length == 0)
            {
                DisplayHelp(p);
            }

            
            try
            {
                opt.Path = p.Parse(args).First();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing arguments: " + ex.Message);
                DisplayHelp(p);
            }
            if (!File.Exists(opt.Path) && !Directory.Exists(opt.Path))
            {
                Console.WriteLine("Directory or file does not exist: " + opt.Path);
                Environment.Exit(-1);
            }


            if (opt.Help)
                DisplayHelp(p);

            if (opt.Watch)
                StartWatching(opt);

            IEnumerable<string> toCompile;
            if (!Directory.Exists(opt.Path) && File.Exists(opt.Path))
                toCompile = new[] { opt.Path};
            else
                toCompile = Glob(opt.Path, "*.coffee");
            
            if (opt.Compile)
            {
                CompileMany(toCompile, opt);
                return;
            }
        }

        private static void StartWatching(CompilerOptions opt)
        {
            var watcher = new ChangeWatcher(opt.Path,opt,
                                (path,options) =>
                                   {
                                       Console.WriteLine(DateTime.Now + ":" + path + " has changed.");
                                       CompileMany(new[] {path}, options);
                                       Console.WriteLine(DateTime.Now + ":" + path + " recompiled");
                                   });
            
            watcher.Start();
            
            Console.WriteLine("Press enter to quit");
            Console.ReadLine();
            Environment.Exit(0);
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
                    var dest = (opt.OutputDir == null)
                                   ? sourcePath
                                   : Path.Combine(opt.OutputDir, new FileInfo(sourcePath).Name);
                        
                    File.WriteAllText(StripExtension(dest) + ".js", result,Encoding.UTF8);
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

            var source = File.ReadAllText(sourcePath,Encoding.UTF8);
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


        private static void DisplayVersion()
        {
            Console.WriteLine("Coffeescript version " + Version);
            Environment.Exit(0);
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

        private static string StripExtension(string fileName)
        {
            var idx = fileName.LastIndexOf(".");
            return (idx == -1 || idx == fileName.Length - 1 )? fileName : fileName.Substring(0,idx);
        }


        private static void DisplayHelp(OptionSet p)
        {
            Console.WriteLine("ncoffee a .net coffeescript command line compiler");
            Console.WriteLine("coffeescript is (c)2010 Jeremy Ashkenas - https://github.com/jashkenas/coffee-script");
            Console.WriteLine("coffeescript version: " + Version);
            Console.WriteLine("Usage: ncoffee [options] path/to/script.coffee");
            p.WriteOptionDescriptions(Console.Out);
            Environment.Exit(0);
        }
    }

    class CompilerOptions
    {
        public bool Compile { get; set; }
        public bool Print { get; set; }
        public bool Help { get; set; }
        public bool Bare { get; set; }
        public string OutputDir { get; set; }
        public bool Watch { get; set; }
        public string Path { get; set; }
    }

}
