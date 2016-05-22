using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Lexer
{
    class ReserverdWords
    {
        public Dictionary<string, TokenTypes> operators;
        public Dictionary<string, TokenTypes> reserverdWords;
        public List<char>specialSymbols;

        public ReserverdWords()
        {
            operators = new Dictionary<string, TokenTypes>();
            reserverdWords = new Dictionary<string, TokenTypes>();
            specialSymbols =  new List<char>();
            InitOperatorsDictionary();
            InitReservdWords();
            InitSpecialOperator();
        }

       public void InitOperatorsDictionary()
        {
            operators.Add("+",TokenTypes.SumOp);
            operators.Add("*",TokenTypes.MultOp);
            operators.Add("=",TokenTypes.EqualOp);
            operators.Add("-",TokenTypes.SubOp);
            operators.Add("(",TokenTypes.SbLeftParent);
            operators.Add(")",TokenTypes.SbRightParent);
            operators.Add("<>",TokenTypes.UnequalOp);
            operators.Add(">",TokenTypes.GreaterOp);
            operators.Add("<", TokenTypes.LessThanOp);
            operators.Add(">=",TokenTypes.GreaterThanOrEqualOp);
            operators.Add("<=",TokenTypes.LessThanOrEqualOp);
            operators.Add(":=", TokenTypes.AsiggnationOp);
            operators.Add(";",TokenTypes.Eos);
            operators.Add(".", TokenTypes.AccesOp);
            operators.Add("..", TokenTypes.RangeOp);
            operators.Add("[",TokenTypes.OpenBracketOperator);
            operators.Add("]",TokenTypes.CloseBracketOperator);
            operators.Add(",",TokenTypes.CommaOperator);
        }

        public void InitSpecialOperator()
        {
            specialSymbols.Add('>');
            specialSymbols.Add('<');
            specialSymbols.Add('.');
            specialSymbols.Add('=');


        }

        public void InitReservdWords()
        {
            reserverdWords.Add("end", TokenTypes.RwEnd);
            
            reserverdWords.Add("begin",TokenTypes.RwBegin);
            reserverdWords.Add("div", TokenTypes.RwDiv);
            reserverdWords.Add("mod", TokenTypes.RwMode);
            reserverdWords.Add("and",TokenTypes.RwAnd);
            reserverdWords.Add("then", TokenTypes.RwThen);
            reserverdWords.Add("or",TokenTypes.RwOr);
            reserverdWords.Add("else",TokenTypes.RwElse);
            reserverdWords.Add("not",TokenTypes.RwNot);
            reserverdWords.Add("xor",TokenTypes.RwXor);
            reserverdWords.Add("shl",TokenTypes.RwShl);
            reserverdWords.Add("shr",TokenTypes.RwShe);
            reserverdWords.Add("Byte",TokenTypes.RwByte);
            reserverdWords.Add("Real", TokenTypes.RwReal);
            reserverdWords.Add("Single",TokenTypes.RwSingle);
            reserverdWords.Add("Double",TokenTypes.RwDouble);
            reserverdWords.Add("char", TokenTypes.RwChar);
            reserverdWords.Add("integer",TokenTypes.RwInteger);
            reserverdWords.Add("const", TokenTypes.RwConst);
            reserverdWords.Add("write",TokenTypes.RwWrite);
            reserverdWords.Add("writeln", TokenTypes.RwWriteLn);
            reserverdWords.Add("read", TokenTypes.RwRead);
            reserverdWords.Add("readln",TokenTypes.RwReadln);
            reserverdWords.Add("program",TokenTypes.RwProgram);
            reserverdWords.Add("function",TokenTypes.RwFunction);
            reserverdWords.Add("procedure",TokenTypes.RwProcedure);
            reserverdWords.Add("if", TokenTypes.RwIf);
            reserverdWords.Add("string", TokenTypes.RwString);
            reserverdWords.Add("case",TokenTypes.RwCase);
            reserverdWords.Add("until",TokenTypes.RwUntil);
            reserverdWords.Add("while",TokenTypes.RwWhile);
            reserverdWords.Add("for",TokenTypes.RwFor);
            reserverdWords.Add("do",TokenTypes.RwDo);
            reserverdWords.Add("in",TokenTypes.RwIn);








        }
    }
}
