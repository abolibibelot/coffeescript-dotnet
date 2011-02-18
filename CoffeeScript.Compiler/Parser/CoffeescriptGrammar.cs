using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;

namespace CoffeeScript.Compiler.Parser
{
    public class CoffeescriptGrammar : Grammar
    {
        public CoffeescriptGrammar() : base(false)  //case-sensitive
        {

            // ReSharper disable InconsistentNaming

            //Terminals
            var NUMBER = new NumberLiteral("Number");
            var IDENTIFIER = new IdentifierTerminal("Identifier");
            var STATEMENT = ToTerm("break") | "continue" | "debugger";
            var STRING = new StringLiteral("String", "\"", StringOptions.AllowsAllEscapes);

            var comment = new CommentTerminal("comment", "#", "\n", "\r");

            NonGrammarTerminals.Add(comment);

            // ReSharper enable InconsistentNaming
        }

        //Make parser indentation-aware
        public override void CreateTokenFilters(LanguageData language, TokenFilterList filters)
        {
            var options = OutlineOptions.ProduceIndents | OutlineOptions.CheckBraces | OutlineOptions.CheckOperator;
            var outlineFilter = new CodeOutlineFilter(language.GrammarData,options,null);
            filters.Add(outlineFilter);
        }

    }
}
