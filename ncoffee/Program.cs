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

           if (opt.Help)
              DisplayHelp(p);

            
            try
            {
                opt.Path = p.Parse(args).First();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing arguments: " + ex.Message);
                DisplayHelp(p);
            }

            new Compiler().Compile(opt);

            if(opt.Watch)
            {
                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }




        private static void DisplayVersion()
        {
            Console.WriteLine("Coffeescript version " + Version);
            Environment.Exit(0);
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

    

}
