using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CoffeeScript.Compiler.Parser;
using Irony.Parsing;
using NUnit.Framework;


namespace CoffeeScript.Compiler.Tests.Grammar
{
    
    public class GrammarFixture
    {
        private Irony.Parsing.Parser _parser;

        public GrammarFixture()
        {
            _parser = new Irony.Parsing.Parser(new CoffeescriptGrammar());
        }


        public string Code(string name)
        {
            return File.ReadAllText(Path.Combine("Grammar/Snippets", name + ".coffee"));
        }

        public ParseTree Parse(string name)
        {
            return ParseString(Code(name));
        }

        public ParseTree ParseString(string code)
        {

            var tree = _parser.Parse(code);
            Assert.IsNotNull(tree);
            return tree;
        }

    }
}
