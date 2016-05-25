﻿namespace Mini_Compiler.Lexer
{
    public enum TokenTypes{
        Eof,
        OpAnd,
        RwAnd,
        UnequalOp,
        GreaterThanOp,
        LessThanOp,
        LessThanOrEqualOp,
        GreaterThanOrEqualOp,
        OpMod,
        Id,
       SumOp,
        MultOp,
        SubOp,
        OpEqual,
        AccesOp,
        DivOp,
        SbLeftParent,
        SbRightParent,
        OpOr,
        OpOneComplement,
        OpLeftShiftOperator,
        OpRightShiftOperator,
        EqualOp,
        RwDiv,
        RwMode,
        RwThen,
        RwOr,
        RwElse,
        RwNot,
        RwXor,
        RwShl,
        RwShe,
        AsiggnationOp,
        RwByte,
        RwReal,
        RwSingle,
        RwDouble,
        RwChar,
        RwInteger,
        RwConst,
        RwWriteLn,
        Eos,
        RwWrite,
        RwRead,
        RwReadln,
        RwBegin,
        RwEnd,
        RwProgram,
        RwFunction,
        RwProcedure,
        RwIf,
        RwCase,
        RwString,
        RwUntil,
        RwWhile,
        RwFor,
        RwDo,
        RwIn,
        RwNumber,
        NumericLiteral,
        Hexa,
        Binary,
        RangeOp,
        StringLiteral,
        OpenBracketOperator,
        CloseBracketOperator,
        CommaOperator,
        CharPound,
        GreaterOp,
        RwType,
        RwTo,
        RwOf,
        RwVar,
        RwRepeat,
        Html,
        Declaretion
    }

    public class Token
    {

        public string Lexeme;
        public TokenTypes Type;
        public int Row;
        public int Column;

        public Token()
        {
        }
    }
}