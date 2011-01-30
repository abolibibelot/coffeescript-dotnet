using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoffeeScript.Compiler.Web.Utils;
using Mono.Options;
using System.Text;

namespace ncoffee
{
    class Program
    {
        static void Main(string[] args)
        {
            bool compile = false;
            bool print = false;
            bool help = false;
            bool bare = false;
            string outputDir = null;

            var p = new OptionSet()
                .Add("c|compile", "compile to JavaScript and save as .js files", _ => compile = true)
                .Add("o=|output=","set the directory for compiled JavaScript", d => outputDir = d )
                .Add("p|print", "print the compiled JavaScript to stdout", _ => print = true)
                .Add("b|bare", "compile without the top-level function wrapper", _ => bare = true)
                .Add("h|help", "display this help message", _ => help = true);

            if (args.Length == 0)
            {
                DisplayHelp(p);
                return;
            }

            var path = p.Parse(args).First();



            IEnumerable<string> toCompile;
            if (!Directory.Exists(path) && File.Exists(path))
                toCompile = new[] { path };
            else
                toCompile = Glob(path, "*.coffee");

            
            if (compile)
            {
                foreach (var sourcePath in toCompile)
                {
                    var source = File.ReadAllText(sourcePath,Encoding.UTF8);
                    var result = CoffeeScriptProcessor.Process(source, bare);
                    if (print)
                        Console.WriteLine(result);
                    else
                    {
                        var dest = (outputDir == null)
                                       ? sourcePath
                                       : Path.Combine(outputDir, new FileInfo(sourcePath).Name);
                        
                        File.WriteAllText(StripExtension(dest) + ".js", result);
                    }
                }
                return;
            }

            if (help)
            {
                DisplayHelp(p);
                return;
            }
        }

        private static IEnumerable<string> Glob(string path, string pattern)
        {
            if (!Directory.Exists(path))
                yield break;
            foreach (var file in Directory.EnumerateFiles(path, pattern))
                yield return file;
            foreach (var child in Directory.EnumerateDirectories(path).SelectMany(dir => Glob(dir, pattern)))
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
            Console.WriteLine("Usage: ncoffee [options] path/to/script.coffee");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
