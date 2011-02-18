#region usings

using System;
using System.Linq;
using System.IO;
using CoffeeScript.Compiler.Util;
using NUnit.Framework;


#endregion

namespace CoffeeScript.Compiler.Tests
{
    [TestFixture]
    public class when_building_directory : CompilerTestBase
    {
        [Test]
        public void source_directory_is_mirrored_in_output()
        {
            var sourceDir = ExampleScripts.Valid.Hierarchy;


            new Compiler().Compile(new CompilerOptions
                                       {
                                           Compile = true,
                                           OutputDir = OutputDir.ToString(),
                                           Path = sourceDir.ToString()
                                       });

            var sourceFiles = sourceDir.Glob("*.coffee");
            var expectedTargetFiles = sourceFiles
                .Select(src => Path.ChangeExtension(src.FullName, "js"))
                .Select(src => src.Replace(sourceDir.ToString(), OutputDir.ToString()))
                .OrderBy(f => f)
                .ToArray();

            var actualTargetFiles = OutputDir.Glob("*.*").Select(f => f.FullName).OrderBy(f => f).ToArray();

            Console.WriteLine("Expecting:");
            expectedTargetFiles.ForEach(Console.WriteLine);

            Console.WriteLine("Got:");
            actualTargetFiles.ForEach(Console.WriteLine);

            Assert.That(actualTargetFiles, Is.EqualTo(expectedTargetFiles));

        }
    }
}