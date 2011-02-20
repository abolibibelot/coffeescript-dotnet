using System.IO;

namespace CoffeeScript.Compiler
{
    public class TargetNotFound : DirectoryNotFoundException
    {
        public TargetNotFound(string path):base(path)
        {            
        }
    }
}