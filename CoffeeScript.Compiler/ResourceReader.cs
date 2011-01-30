using System.IO;

namespace CoffeeScript.Compiler
{

    class ResourceReader 
    {
        public static string GetFromResources(string resourceName)
        {
            var asm = typeof(ResourceReader).Assembly;
            using (var reader = new StreamReader(asm.GetManifestResourceStream(resourceName)))
            {
                    return reader.ReadToEnd();
            }
        }
    }

}
