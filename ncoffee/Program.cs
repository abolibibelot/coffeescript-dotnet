using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoffeeScript.Compiler;
using Mono.Options;
using System.Text;

namespace ncoffee
{
    class Program
    {

        const string Version = "1.0.1";

        static void Main(string[] args)
        {
            var opt = new CompilerOptions();

            var p = new OptionSet()
                .Add("c|compile", "compile to JavaScript and save as .js files", _ => opt.Compile = true)
                .Add("o=|output=","set the directory for compiled JavaScript", d => opt.OutputDir = d )
                .Add("p|print", "print the compiled JavaScript to stdout", _ => opt.Print = true)
                .Add("b|bare", "compile without the top-level function wrapper", _ => opt.Bare = true)
                .Add("v|version", "display coffeescript version", _ => DisplayVersion())
                .Add("h|help", "display this help message", _ => opt.Help = true);

            if (args.Length == 0)
            {
                DisplayHelp(p);
            }

            string path = null;

            try
            {
                path = p.Parse(args).First();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing arguments: " + ex.Message);
                DisplayHelp(p);
            }

            if (opt.Help)
                DisplayHelp(p);


            IEnumerable<string> toCompile;
            if (!Directory.Exists(path) && File.Exists(path))
                toCompile = new[] { path };
            else
                toCompile = Glob(path, "*.coffee");

            
            if (opt.Compile)
            {
                foreach (var sourcePath in toCompile)
                {
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
                return;
            }

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
    }

}
