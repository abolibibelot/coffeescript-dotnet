using System;
using Jurassic;

namespace CoffeeScript.Compiler.Web.Utils
{
    /// <summary>
    /// Processes CoffeeScript files into javascript
    /// </summary>
    public class CoffeeScriptProcessor
    {
        private static readonly string COMPILE_TASK = "CoffeeScript.compile(Source, {{bare: {0}}})";

        private static ScriptEngine _engine;
        private static readonly object _o = new object();

        static CoffeeScriptProcessor()
        {
            _engine = new ScriptEngine();
            _engine.Execute(ResourceReader.GetFromResources("CoffeeScript.Compiler.coffeescript.js"));
        }


        private static ScriptEngine Engine
        {
            get
            {
                return _engine;
            }
        }

        public static string Process(string contents)
        {
            return Process(contents, false);
        }


        /// <summary>
        /// Processes contents as a coffeescript file
        /// </summary>
        /// <param name="contents">The javascript contents</param>
        /// <returns></returns>
        public static string Process(string contents, bool bare)
        {
            lock (_o)
            {
                try
                {
                    Engine.SetGlobalValue("Source", contents);
                    var bareArg = bare ? "true" : "false";
                    return Engine.Evaluate<string>(String.Format(COMPILE_TASK,bareArg));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
        }
    }
}