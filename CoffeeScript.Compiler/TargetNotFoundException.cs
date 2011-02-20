using System.IO;

namespace CoffeeScript.Compiler
{
    public class TargetNotFoundException : DirectoryNotFoundException
    {
        public TargetNotFoundException(string path):base(path)
        {            
        }
    }
}