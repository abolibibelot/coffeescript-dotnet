#region usings

using System;
using System.IO;
using NUnit.Framework;
using Composable.System.IO;
using System.Linq;
using Composable.System.Linq;

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

            var sourceFiles = sourceDir.GetFilesResursive().WithExtension(".coffee");
            var expectedTargetFiles = sourceFiles
                .Select(src => Path.ChangeExtension(src.FullName, "js"))
                .Select(src => src.Replace(sourceDir.ToString(), OutputDir.ToString()))
                .OrderBy(f => f)
                .ToArray();

            var actualTargetFiles = OutputDir.GetFilesResursive().Select(f => f.FullName).OrderBy(f => f).ToArray();

            Console.WriteLine("Expecting:");
            expectedTargetFiles.ForEach(f => Console.WriteLine(f));

            Console.WriteLine("Got:");
            actualTargetFiles.ForEach(f => Console.WriteLine(f));

            Assert.That(actualTargetFiles, Is.EqualTo(expectedTargetFiles));

        }
    }
}