using System.IO;
using CoffeeScript.Compiler.Util;

namespace CoffeeScript.Compiler.Tests
{
    public static class ExampleScripts
    {
        private static readonly DirectoryInfo Base = CompilerTestBase.BaseDir.SubDir("ExampleScriptFolder");

        public static class Valid
        {
            private static readonly DirectoryInfo Base = ExampleScripts.Base.SubDir("Valid");
            public static readonly DirectoryInfo Hierarchy = Base.SubDir("Hierarchy");
            public static readonly FileInfo SingleScript = CompilerTestBase.BaseDir.File("valid.coffee");
            public static readonly string Relative = "../../ExampleScriptFolder/Valid/";
        }

        public static class InValid
        {
            private static readonly DirectoryInfo Base = ExampleScripts.Base.SubDir("InValid");
            public static readonly DirectoryInfo Hierarchy = Base.SubDir("Hierarchy");
            public static readonly FileInfo SingleScript = CompilerTestBase.BaseDir.File("valid.coffee");
        }            
    }
}