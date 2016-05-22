using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Lexer;

namespace Mini_Compiler
{
    class SampleParserBinaryToFloat
    {
        private Lexer.Lexer lexer;
        private Token currentToken;

        SampleParserBinaryToFloat(Lexer.Lexer lexer)
        {
            this.lexer = lexer;
        }
        public void Parse()
        {
            currentToken = lexer.GetNextToken();
            S();
            if (currentToken.Type != TokenTypes.Eof)
            {
                throw new SyntaxException("Se esperaba EOF");
            }
        }

        private void S()
        {
            D();

        }

        private void D()
        {
            if (currentToken.Lexeme == "0")
            {
                
            }
            else if (currentToken.Lexeme =="1")
            {
                
            }
            else
            {
                
            }




        }
    }
}
