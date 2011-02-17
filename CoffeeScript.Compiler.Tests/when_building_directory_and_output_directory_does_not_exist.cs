#region usings

using System;
using NUnit.Framework;

#endregion

namespace CoffeeScript.Compiler.Tests
{
    [TestFixture]
    public class when_building_directory_and_output_directory_does_not_exist : CompilerTestBase
    {
        [Test]
        public void throws_TargetNotFound_exception()
        {
            Assert.Throws<TargetNotFound>(() => new Compiler().Compile(new CompilerOptions
                                                                           {
                                                                               Compile = true,
                                                                               Path = Environment.CurrentDirectory,
                                                                               OutputDir = @"Q:\nonsense\nonsense\nonsense\nonsense"
                                                                           }));
        }
    }
}