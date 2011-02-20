using System.IO;

namespace CoffeeScript.Compiler
{
    public class SourceNotFoundException : DirectoryNotFoundException
    {
        public SourceNotFoundException(string path):base(path)
        {            
        }
    }
}