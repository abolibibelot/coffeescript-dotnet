using System.IO;

namespace CoffeeScript.Compiler
{
    public class SourceNotFound : DirectoryNotFoundException
    {
        public SourceNotFound(string path):base(path)
        {            
        }
    }
}