using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Lexer;

namespace Mini_Compiler.Sintactico
{
    class Syntactic
    {
        private Lexer.Lexer lexer;
        private Token currentToken;

        public Syntactic(Lexer.Lexer lexer)
        {
            this.lexer = lexer;
        }

        public void Parse()
        {
            BeginProgram();
            if (currentToken.Type != TokenTypes.Eof)
            {
                throw new SyntaxException("Se esperaba EOF");
            }
        }

        private void BeginProgram()
        {
            throw new NotImplementedException();
        }
    }
}
