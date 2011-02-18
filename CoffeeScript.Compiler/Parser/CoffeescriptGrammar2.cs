using System;
using Irony.Parsing;

namespace CoffeeScript.Compiler.Parser
{
    public class CoffeescriptGrammar2 : Grammar
    {
        public CoffeescriptGrammar2() : base(false)  //case-sensitive
        {


// ReSharper disable InconsistentNaming

            //Terminals
            var NUMBER = new NumberLiteral("Number");
            var IDENTIFIER = new IdentifierTerminal("Identifier");
            var STATEMENT = ToTerm("break") | "continue" | "debugger";
            var STRING = new StringLiteral("String", "\"", StringOptions.AllowsAllEscapes);
            // actually this could be simplified to @"^`[^`]*`" but we're sticking to the original here
            //var JS = new RegexBasedTerminal("JS", @"^`[^`]*(?:\.[^`]*)*`");
            var JS = new QuotedValueLiteral("JS", "`", TypeCode.String);
            var HERECOMMENT = new QuotedValueLiteral("HERECOMMENT", "###", TypeCode.String);
            var comment = new CommentTerminal("comment", "#", "\n", "\r");

            NonGrammarTerminals.Add(comment);
            //FIXME!
/*            
            var REGEX = new RegexBasedTerminal("REGEX",
                                               @"
/ ^
  / (?! \s )       # disallow leading whitespace
  [^ [ / \n \\ ]*  # every other thing
  (?:
    (?: \[\s\S]   # anything escaped
      | \[         # character class
           [^ \] \n \\ ]*
           (?: \\[\s\S] [^ \] \n \\ ]* )*
         ]
    ) [^ [ / \n \\ ]*
  )*
  / [imgy]{0,4} (?!\w)
/
");
 */          
 
            var BOOL = ToTerm("true") | "false" | "null" | "undefined";
            var CLASS = ToTerm("class");
            var EXTENDS = ToTerm("extends");
            var PARAM_START = ToTerm("(") | Empty;
            var PARAM_END = ToTerm(")") | Empty;
            var FUNC_GLYPH = ToTerm("=>") | ToTerm("->");
            var COMMA = ToTerm(",");
            var OPT_COMMA = ToTerm(",") | Empty;
            //
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

            //TERMINATOR terminal is Eol non-terminal here

            //Non terminals
            var Body = new NonTerminal("Body");
            var Eol = new NonTerminal("Eol");
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
            var Assign = new NonTerminal("Assign");
            var If = new NonTerminal("If");
            var Try = new NonTerminal("Try");
            var While = new NonTerminal("While");
            var For = new NonTerminal("For");
            var Switch = new NonTerminal("Switch");
            var Class = new NonTerminal("Class");
            var Block = new NonTerminal("Block");
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
            var CompoundAssignOp = new NonTerminal("CompoundAssign");

            // ReSharper restore InconsistentNaming

            UnaryOp.Rule = ToTerm("+") | ToTerm("-");
            BinaryOp.Rule = ToTerm("+") | "-" | "*";

            CompoundAssignOp.Rule = ToTerm("+=") | "-=" | "*=" | "/=";

            Eol.Rule = NewLine | ";";

            Body.Rule = MakePlusRule(Body, Line);
            Line.Rule = Expression + Eol
                       | Statement + Eol;
            Statement.Rule = Return | Throw | Comment | STATEMENT;
            Expression.Rule = Value | Invocation | Code | Operation | Assign
                              | If | Try | While | For | Switch | Class;

            Block.Rule = Indent + Body + Dedent;
            AlphaNumeric.Rule = NUMBER | STRING;
            Literal.Rule = AlphaNumeric | JS | BOOL; // | REGEX; FIXME

            Assign.Rule = (Assignable + "=" + Expression)
                          | (Assignable + "=" + Indent + Expression + Dedent);

            AssignObj.Rule = ObjAssignable | (ObjAssignable + ":" + Expression)
                             | (ObjAssignable + ":" + Indent + Expression + Dedent);

            ObjAssignable.Rule = IDENTIFIER | AlphaNumeric | ThisProperty;

            Return.Rule = "return" + Expression | "return";

            Comment.Rule = HERECOMMENT;

            Code.Rule = (PARAM_START + ParamList + PARAM_END + FUNC_GLYPH + Block)
                        | (FUNC_GLYPH + Block);

            ParamList.Rule = MakePlusRule(ParamList, COMMA, Param);

            Param.Rule = ParamVar
                         | (ParamVar + "...")
                         | (ParamVar + "=" + Expression);

            ParamVar.Rule = IDENTIFIER | ThisProperty | ArrayNonTerm | ObjectNonTerm;

            Splat.Rule = Expression + "...";

            SimpleAssignable.Rule = IDENTIFIER | Value + Accessor | Invocation + Accessor | ThisProperty;

            Assignable.Rule = SimpleAssignable | ArrayNonTerm | ObjectNonTerm;

            Value.Rule = Assignable | Literal | Parenthetical | Range | This;

            Accessor.Rule = ToTerm(".") + IDENTIFIER
                            | ToTerm("?:") + IDENTIFIER
                            | ToTerm("::") + IDENTIFIER
                            | ToTerm("::")
                            | Index
                            | Slice;

            Index.Rule = INDEX_START + Expression + INDEX_END
                         | INDEX_SOAK + Index
                         | INDEX_PROTO + Index;

            ObjectNonTerm.Rule = "{" + AssignList + OPT_COMMA + "}";

            AssignList.Rule = Empty
                              | AssignObj
                              | AssignList + COMMA + AssignObj
                              | AssignList + OPT_COMMA + Eol + AssignObj
                              | AssignList + OPT_COMMA + Indent + AssignList + OPT_COMMA + Dedent;

            Class.Rule = CLASS
                         | CLASS + Block
                         | CLASS + EXTENDS + Value
                         | CLASS + EXTENDS + Value + Block
                         | CLASS + SimpleAssignable
                         | CLASS + SimpleAssignable + Block
                         | CLASS + SimpleAssignable + EXTENDS + Value
                         | CLASS + SimpleAssignable + EXTENDS + Value + Block;


            Invocation.Rule = Value + OptFuncExist + Arguments
                              | Invocation + OptFuncExist + Arguments
                              | SUPER
                              | SUPER + Arguments;


            OptFuncExist.Rule = Empty | FUNC_EXIST;

            Arguments.Rule = CALL_START + CALL_END
                             | CALL_START + ArgList + OPT_COMMA + CALL_END;

            This.Rule = THIS | "@";

            ThisProperty.Rule = "@" + IDENTIFIER;

            ArrayNonTerm.Rule = ToTerm("[") + "]"
                                | "[" + ArgList + OPT_COMMA + "]";

            RangeDots.Rule = ToTerm("..") | "...";

            Range.Rule = "[" + Expression + RangeDots + Expression + "]";

            Slice.Rule = INDEX_START + Expression + RangeDots + Expression + INDEX_END
                         | INDEX_START + Expression + RangeDots + INDEX_END
                         | INDEX_START + RangeDots + Expression + INDEX_END; 




            //FIXME:
            ArgList.Rule = MakePlusRule(ArgList, COMMA, Arg, TermListOptions.AllowTrailingDelimiter);
            

  //            ArgList: [
  //  o 'Arg',                                              -> [$1]
  //  o 'ArgList , Arg',                                    -> $1.concat $3
  //  o 'ArgList OptComma TERMINATOR Arg',                  -> $1.concat $4
  //  o 'INDENT ArgList OptComma OUTDENT',                  -> $2
  //  o 'ArgList OptComma INDENT ArgList OptComma OUTDENT', -> $1.concat $4
  //]

            Arg.Rule = Expression | Splat;

            SimpleArgs.Rule = Expression | SimpleArgs + COMMA + Expression;

            Try.Rule = TRY + Block
                       | TRY + Block + Catch
                       | TRY + Block + FINALLY + Block
                       | TRY + Block + Catch + FINALLY + Block;

            Catch.Rule = CATCH + IDENTIFIER + Block;

            Throw.Rule = THROW + Expression;

            Parenthetical.Rule = "(" + Body + ")"
                                 | "(" + Indent + Body + Dedent + ")";

            WhileSource.Rule = WHILE + Expression
                               | WHILE + Expression + WHEN + Expression
                               | UNTIL + Expression
                               | UNTIL + Expression + WHEN + Expression;

            While.Rule = WhileSource + Block
                               | Statement + WhileSource
                               | Expression + WhileSource
                               | Loop;

            Loop.Rule = LOOP + Block
                        | LOOP + Expression;

            For.Rule = Statement + ForBody
                       | Expression + ForBody
                       | ForBody + Block;

            ForBody.Rule = FOR + Range
                           | ForStart + ForSource;

            ForStart.Rule = FOR + ForVariables
                            | FOR + OWN + ForVariables;

            ForValue.Rule = IDENTIFIER | ArrayNonTerm | ObjectNonTerm;

            ForVariables.Rule = ForValue
                                | ForValue + COMMA + ForValue;

            ForSource.Rule = FORIN + Expression
                             | FOROF + Expression
                             | FORIN + Expression + WHEN + Expression
                             | FOROF + Expression + WHEN + Expression
                             | FORIN + Expression + BY + Expression;

            Switch.Rule = SWITCH + Expression + Indent + Whens + Dedent
                          | SWITCH + Expression + Indent + Whens + ELSE + Block + Dedent
                          | SWITCH + Indent + Whens + Dedent
                          | SWITCH + Indent + Whens + ELSE + Block + Dedent;

            Whens.Rule = When
                         | Whens + When;


            When.Rule = LEADING_WHEN + SimpleArgs + Block
                        | LEADING_WHEN + SimpleArgs + Block + Eol;

            IfBlock.Rule = IF + Expression + Block
                           | IfBlock + ELSE + IF + Expression + Block;

            If.Rule = IfBlock
                      | IfBlock + ELSE + Block
                      | Statement + POST_IF + Expression
                      | Expression + POST_IF + Expression;

            //FIXME: operations (lacks assignment and compound assignement)
            Operation.Rule = UnaryOp + Expression
                             | Expression + "?"
                             | Expression + BinaryOp + Expression;
            



            Root = Body;
        }
    }
}
