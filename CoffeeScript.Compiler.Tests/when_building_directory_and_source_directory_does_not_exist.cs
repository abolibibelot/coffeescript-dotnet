#region usings

using NUnit.Framework;

#endregion

namespace CoffeeScript.Compiler.Tests
{
    [TestFixture]
    public class when_building_directory_and_source_directory_does_not_exist : CompilerTestBase
    {
        [Test]
        public void throws_SourceNotFound_exception()
        {
            Assert.Throws<SourceNotFound>(() => new Compiler().Compile(new CompilerOptions
                                                                           {
                                                                               Compile = true,
                                                                               Path = @"Q:\nonsense\nonsense\nonsense\nonsense"
                                                                           }));
        }
    }
}