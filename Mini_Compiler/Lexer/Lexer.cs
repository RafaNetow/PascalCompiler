using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Lexer
{
    public class Lexer
    {
        public StringContent Content;
        private Symbol currentSymbol;
        private ReserverdWords rw = new ReserverdWords();

        public bool OnlyHexInString(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }
        public Lexer(StringContent content)
        {
            Content = content;
            currentSymbol = content.nextSymbol();
            
        }

        
        public Token GetNextToken()
        {
            int state = 0;
            string lexeme = "";
            int tokenRow = 0;
            int tokenColumn = 0;

            while (true)
            {
                switch (state)
                {
                    case 0:
                         if (char.IsWhiteSpace(currentSymbol.CurrentSymbol))
                        {
                            state = 0;
                            currentSymbol = Content.nextSymbol();
                        }
                        else if (char.IsLetter(currentSymbol.CurrentSymbol))
                        {
                            state = 1;
                            tokenColumn = currentSymbol.Column;
                            tokenRow = currentSymbol.Row;
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                        else if (char.IsDigit(currentSymbol.CurrentSymbol))
                        {
                            state = 2;
                            tokenColumn = currentSymbol.Column;
                            tokenRow = currentSymbol.Row;
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                          else if (currentSymbol.CurrentSymbol == '\"')
                        {
                            state = 3;
                            tokenColumn = currentSymbol.Column;
                            tokenRow = currentSymbol.Row;
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                        else if (currentSymbol.CurrentSymbol == ':')
                        {
                            state = 4;
                            tokenColumn = currentSymbol.Column;
                            tokenRow = currentSymbol.Row;
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                        else if (rw.operators.ContainsKey(currentSymbol.CurrentSymbol.ToString()))
                        {

                            state = 5;
                            tokenColumn = currentSymbol.Column;
                            tokenRow = currentSymbol.Row;
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();

                        }

                        else if (currentSymbol.CurrentSymbol == '\0')
                        {
                            state = 6;
                            tokenColumn = currentSymbol.Column;
                            tokenRow = currentSymbol.Row;
                            lexeme = "$";
                        }
                      
                       else if (currentSymbol.CurrentSymbol == '$')
                        {
                            state = 7;
                            tokenColumn = currentSymbol.Column;
                            tokenRow = currentSymbol.Row;
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();

                        }
                        else if (currentSymbol.CurrentSymbol == '%')
                        {
                            state = 8;
                            tokenColumn = currentSymbol.Column;
                            tokenRow = currentSymbol.Row;
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                        else if(currentSymbol.CurrentSymbol == '#')
                        {
                            state = 10;
                            tokenColumn = currentSymbol.Column;
                            tokenRow = currentSymbol.Row;
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        } 
                        else if (currentSymbol.CurrentSymbol == '{')
                        {
                            state = 11;
                            tokenColumn = currentSymbol.Column;
                            tokenRow = currentSymbol.Row;
                            currentSymbol = Content.nextSymbol();
                        } 
                        else if (currentSymbol.CurrentSymbol == '/')
                        {
                            state = 12;
                            tokenColumn = currentSymbol.Column;
                            tokenRow = currentSymbol.Row;
                        }                 
                        
                        else
                        {
                            throw new LexicalException($"Symbol {currentSymbol.CurrentSymbol} not recognized at Row:{currentSymbol.Row} Col: {currentSymbol.Column}");
                        }
                        break;
                    case 1:
                        if (char.IsLetterOrDigit(currentSymbol.CurrentSymbol))
                        {
                            state = 1;
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                        else if (rw.reserverdWords.ContainsKey(lexeme))
                        {
                            return new Token
                            {
                                Type = rw.reserverdWords[lexeme],
                                Lexeme = lexeme,
                                Column = tokenColumn,
                                Row = tokenRow
                            };
                        }
                        
                        else

                        {

                            return new Token { Type = TokenTypes.Id, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow };
                        }
                        break;
                    case 2:
                        if (char.IsDigit(currentSymbol.CurrentSymbol))
                        {
                            state = 2;
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                        else if (currentSymbol.CurrentSymbol == '.')
                        {
                            state = 9;
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                            return new Token { Type = TokenTypes.NumericLiteral, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow };
                        }
                        break;
                    case 3:
                        if (currentSymbol.CurrentSymbol != '\"')
                        {
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                        else if (currentSymbol.CurrentSymbol == '\"')
                        {
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                            return new Token { Type = TokenTypes.StringLiteral, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow };
                        }
                        break;
                    case 4:

                       
                        if (currentSymbol.CurrentSymbol == '=')
                        {
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                            return new Token
                            {
                                Type = rw.operators[lexeme],
                                Lexeme = lexeme,
                                Column = tokenColumn,
                                Row = tokenRow
                            };
                        }
                        else
                        {
                            throw new LexicalException(
                                $"Symbol {currentSymbol.CurrentSymbol} not recognized at Row:{currentSymbol.Row} Col: {currentSymbol.Column}");
                        }
                 

                        break;
                    case 5:
                        if ( rw.specialSymbols.Contains(currentSymbol.CurrentSymbol))
                        {
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                            return new Token
                            {
                                Type = rw.operators[lexeme],
                                Lexeme = lexeme,
                                Column = tokenColumn,
                                Row = tokenRow
                            };
                        }
                        else
                        {
                            return new Token
                            {
                                Type = rw.operators[lexeme],
                                Lexeme = lexeme,
                                Column = tokenColumn,
                                Row = tokenRow
                            };
                        }          
                        break;
                    case 6:
                        return new Token { Type = TokenTypes.Eof, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow };

                    case 7:
                        if (OnlyHexInString(currentSymbol.CurrentSymbol.ToString()))
                        {
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                           
                            return new Token {Type = TokenTypes.Hexa, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow};
                        }
                        break;
                    case 8:
                        if (currentSymbol.CurrentSymbol == '1' || currentSymbol.CurrentSymbol == '0')
                        {
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                        else
                        {

                            return new Token { Type = TokenTypes.Binary, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow };
                        }
                        break;

                    case 9:
                        if (char.IsDigit(currentSymbol.CurrentSymbol))
                        {
                            state = 9;
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                            return new Token { Type = TokenTypes.NumericLiteral, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow };
                        }

                        break;
                    case 10:
                        if (char.IsDigit(currentSymbol.CurrentSymbol))
                        {
                            lexeme += currentSymbol.CurrentSymbol;
                            currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                            return new Token
                            {
                                Type = TokenTypes.CharPound,
                                Lexeme = lexeme,
                                Column = tokenColumn,
                                Row = tokenColumn
                            };
                        }
                        break;
                    case 11:
                        if (currentSymbol.CurrentSymbol == '\0')
                        {
                            state = 6;
                        }
                        else if (currentSymbol.CurrentSymbol == '}')
                        {
                            state = 0;
                            currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                            currentSymbol = Content.nextSymbol();
                        }
                        break;
                        
                    case 12:
                       var temp = Content.nextSymbol();
                        if (temp.CurrentSymbol == '/')
                        {
                            state = 13;
                            currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                            currentSymbol = temp;
                            return new Token { Type = TokenTypes.DivOp, Lexeme = "/", Column = tokenColumn, Row = tokenRow };
                        }
                        break;
                    case 13:

                         if (currentSymbol.CurrentSymbol == '\0')
                        {
                            state = 6;
                            tokenColumn = currentSymbol.Column;
                            tokenRow = currentSymbol.Row;
                            lexeme = "$";
                        }
                        else if (tokenRow == currentSymbol.Row)
                        {
                            currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                            state = 0;
                           
                        }
                        break;

                            default:
                        break;
                }
            }

            throw new NotImplementedException();
        }
    }
}
