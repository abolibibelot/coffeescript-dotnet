using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoffeeScript.Compiler.Web.Utils;
using Mono.Options;

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

            var p = new OptionSet()
                .Add("c|compile", "compile to JavaScript and save as .js files", _ => compile = true)
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
                    Console.WriteLine("Compiling " + sourcePath);
                    var source = File.ReadAllText(sourcePath);
                    var result = CoffeeScriptProcessor.Process(source, bare);
                    File.WriteAllText(StripExtension(sourcePath) + ".js", result);
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
            Console.WriteLine("ncoffee a .net command line compiler based on coffeescript");
            Console.WriteLine("Usage: ncoffee [options] path/to/script.coffee");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
