using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;
using Irony.Ast;

namespace CoffeeScript.Compiler.Parser
{




    [Language("coffeescript", "1.0.1", "Coffeescript grammar")]
    public class CoffeescriptGrammar : Grammar
    {

        void CreateBlock(ParsingContext ctx, ParseTreeNode node)
        {
            int i = 0;
        }


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

            var BOOL = ToTerm("true") | "false" | "null" | "undefined";
            var CLASS = ToTerm("class");
            var EXTENDS = ToTerm("extends");
            var PARAM_START = ToTerm("(") | Empty;
            var PARAM_END = ToTerm(")") | Empty;
            var FUNC_GLYPH = ToTerm("=>") | ToTerm("->");
            var COMMA = ToTerm(",");
            var OPT_COMMA = ToTerm(",") | Empty;
            var INDEX_START = ToTerm("[");
            var INDEX_END = ToTerm("]");
            var INDEX_SOAK = ToTerm("?");
            var INDEX_PROTO = ToTerm("::");
            var SUPER = ToTerm("super");
            var FUNC_EXIST = ToTerm("?");
            var CALL_START = ToTerm("(");
            var CALL_END = ToTerm(")");
            var THIS = ToTerm("this");
            var TRY = ToTerm("try");
            var FINALLY = ToTerm("finally");
            var CATCH = ToTerm("catch");
            var THROW = ToTerm("throw");
            var WHILE = ToTerm("while");
            var WHEN = ToTerm("when");
            var UNTIL = ToTerm("until");
            var LOOP = ToTerm("loop");
            var FOR = ToTerm("for");
            var OWN = ToTerm("own");
            var FORIN = ToTerm("in");
            var FOROF = ToTerm("of");
            var BY = ToTerm("by");
            var SWITCH = ToTerm("switch");
            var ELSE = ToTerm("else");
            var LEADING_WHEN = ToTerm("When");
            var IF = ToTerm("IF");
            var POST_IF = ToTerm("if");

            #region Non Terminals
            var Body = new NonTerminal("Body", CreateBlock);
            var UnaryOp = new NonTerminal("UnaryOp");
            var Statement = new NonTerminal("Statement");
            var Expression = new NonTerminal("Expression");
            var Line = new NonTerminal("Line");
            var Return = new NonTerminal("Return");
            var Throw = new NonTerminal("Throw");
            var Comment = new NonTerminal("Comment");
            var Value = new NonTerminal("Value");
            var Invocation = new NonTerminal("Invocation");
            var Code = new NonTerminal("Code");
            var Operation = new NonTerminal("Operation");
            var Assign = new NonTerminal("Assign",typeof(AssignmentNode));
            var If = new NonTerminal("If");
            var Try = new NonTerminal("Try");
            var While = new NonTerminal("While");
            var For = new NonTerminal("For");
            var Switch = new NonTerminal("Switch");
            var Class = new NonTerminal("Class");
            var Block = new NonTerminal("Block",CreateBlock);
            var AlphaNumeric = new NonTerminal("AlphaNumeric");
            var Literal = new NonTerminal("Literal");
            var Assignable = new NonTerminal("Assignable");
            var AssignObj = new NonTerminal("AssignObj");
            var ObjAssignable = new NonTerminal("ObjAssignable");
            var ThisProperty = new NonTerminal("ThisProperty");
            var ParamList = new NonTerminal("ParamList");
            var Param = new NonTerminal("Param");
            var ParamVar = new NonTerminal("ParamVar");
            var ArrayNonTerm = new NonTerminal("Array");
            var ObjectNonTerm = new NonTerminal("Object");
            var Splat = new NonTerminal("Splat");
            var SimpleAssignable = new NonTerminal("SimpleAssignable");
            var Accessor = new NonTerminal("Accessor");
            var Parenthetical = new NonTerminal("Parenthetical");
            var This = new NonTerminal("This");
            var Range = new NonTerminal("Range");
            var Index = new NonTerminal("Index");
            var Slice = new NonTerminal("Slice");
            var AssignList = new NonTerminal("AssignList");
            var OptFuncExist = new NonTerminal("OptFuncExist");
            var Arguments = new NonTerminal("Arguments");
            var ArgList = new NonTerminal("ArgList");
            var RangeDots = new NonTerminal("RangeDots");
            var Arg = new NonTerminal("Arg");
            var SimpleArgs = new NonTerminal("SimpleArgs");
            var Catch = new NonTerminal("Catch");
            var WhileSource = new NonTerminal("WhileSource");
            var Loop = new NonTerminal("Loop");
            var ForBody = new NonTerminal("ForBody");
            var ForStart = new NonTerminal("ForStart");
            var ForSource = new NonTerminal("ForSource");
            var ForVariables = new NonTerminal("ForVariables");
            var ForValue = new NonTerminal("ForValue");
            var Whens = new NonTerminal("Whens");
            var When = new NonTerminal("When");
            var IfBlock = new NonTerminal("IfBlock");
            var BinaryOp = new NonTerminal("BinaryOp");
            var BinExpr = new NonTerminal("BinExpr",typeof(BinaryOperationNode));
            var CompoundAssignOp = new NonTerminal("CompoundAssign");


            #endregion


            #region Production rules
            
            BinaryOp.Rule = ToTerm("+") | "-" | "*";

            Body.Rule = MakePlusRule(Body, Line);
            Line.Rule = Expression + Eos | Statement + Eos;

            Statement.Rule = STATEMENT;

            Expression.Rule = Value | BinExpr | Assign;

            BinExpr.Rule = Value + BinaryOp + Value;

            Assign.Rule = (Assignable + "=" + Expression)
                        | (Assignable + "=" + Indent + Expression + Dedent);

            SimpleAssignable.Rule = IDENTIFIER;

            Assignable.Rule = SimpleAssignable;

            Value.Rule = Assignable | Literal;

            AlphaNumeric.Rule = NUMBER | STRING;
            Literal.Rule = AlphaNumeric;

            #endregion


            MarkTransient(Line);

            Root = Body;

            LanguageFlags = LanguageFlags.CreateAst;



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
