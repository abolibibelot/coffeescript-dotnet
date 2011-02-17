namespace CoffeeScript.Compiler
{
    public class CompilerOptions
    {
        public bool Compile { get; set; }
        public bool Print { get; set; }
        public bool Help { get; set; }
        public bool Bare { get; set; }
        public string OutputDir { get; set; }
        public bool Watch { get; set; }
        public string Path { get; set; }
    }
}