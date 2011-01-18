using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jurassic;

namespace CoffeeScript.Compiler.Web.Utils
{
    /// <summary>
    /// Processes CoffeeScript files into javascript
    /// </summary>
    public class CoffeeScriptProcessor
    {
        private static readonly string COMPILE_TASK = "CoffeeScript.compile(Source, {bare: true})";

        [ThreadStatic]
        private static ScriptEngine _engine;
        private static object _o = new object();

        private static ScriptEngine Engine
        {
            get
            {
                if (_engine == null)
                {
                    _engine = new ScriptEngine();
                    _engine.Execute(Resources.CoffeeScript);
                }

                return _engine;
            }
        }

        /// <summary>
        /// Precesses contents as a coffeescript file
        /// </summary>
        /// <param name="contents">The javascript contents</param>
        /// <returns></returns>
        public static string Process(string contents)
        {
            lock (_o)
            {
                try
                {
                    Engine.SetGlobalValue("Source", contents);
                    return Engine.Evaluate<string>(COMPILE_TASK);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
    }
}