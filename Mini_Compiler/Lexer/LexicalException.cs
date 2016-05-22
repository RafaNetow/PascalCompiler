 using System;
using System.Runtime.Serialization;

namespace Mini_Compiler.Lexer
{
    [Serializable]
    internal class LexicalException : Exception
    {
        private Symbol currentSymbol;

        public LexicalException()
        {
        }

        public LexicalException(string message) : base(message)
        {
        }

        public LexicalException(Symbol currentSymbol)
        {
            this.currentSymbol = currentSymbol;
        }

        public LexicalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LexicalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}