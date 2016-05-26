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
        private Symbol _currentSymbol;
        private bool _pascalMode;
        private ReserverdWords rw = new ReserverdWords();

        public bool OnlyHexInString(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }
        public Lexer(StringContent content)
        {
            Content = content;
            _pascalMode = false;
            _currentSymbol = content.nextSymbol();
            
        }

        
        public Token GetNextToken()
        {
            int state = 100;
            string lexeme = "";
            int eof = 0;
            int tokenRow = 0;
            int tokenColumn = 0;

            if (_pascalMode)
                state = 0;

            while (true)
            {
                switch (state)
                {

                    //consume html o verficia si va a venir algo perecido a un html
                    case 100:

                        if (_currentSymbol.CurrentSymbol == '\0')
                        {
                            state = 600;
                        }
                    
                          else if (_currentSymbol.CurrentSymbol == '<')
                          {
                              //    this->lexemaA = token.Lexema;
                              lexeme += _currentSymbol.CurrentSymbol;
                              _currentSymbol = Content.nextSymbol();
                              state = 200;
                          }
                          else
                          {
                              lexeme += _currentSymbol.CurrentSymbol;
                              _currentSymbol = Content.nextSymbol();
                              state = 100;
                          }
                        break;
                    case 200:

                        if (_currentSymbol.CurrentSymbol == '%')
                        {
                            //  token.Lexema = lexemaAnterior;
                            this._pascalMode = true;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();

                            return new Token
                            {
                                Type = TokenTypes.Html,
                                Column = tokenColumn,
                                Row = tokenRow,
                                Lexeme = lexeme


                            };

                        }
                        else if (_currentSymbol.CurrentSymbol == '\0')
                        {
                            state = 6;
                      
                            return new Token {Row = tokenRow,Column = tokenColumn,Lexeme = lexeme, Type = TokenTypes.Html};
  
                        }
                        else
                        {
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row; 
                            lexeme +=_currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();                                       
                            state = 300;
                        }
                        break;
                    case 300:
                        if (_currentSymbol.CurrentSymbol == '<')
                        {
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                            state = 200;
                        }
                        else if (_currentSymbol.CurrentSymbol == '\0')
                        {
                            state = 6;

                            return new Token
                            {
                                Column = tokenColumn,
                                Row = tokenRow,
                                Lexeme = lexeme,
                                Type = TokenTypes.Html
                            };
                        }
                        else
                        {
                           lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        }
                        break;



                    case 0:
                         if (char.IsWhiteSpace(_currentSymbol.CurrentSymbol))
                        {
                            state = 0;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else if (char.IsLetter(_currentSymbol.CurrentSymbol))
                        {
                            state = 1;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else if (char.IsDigit(_currentSymbol.CurrentSymbol))
                        {
                            state = 2;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        }
                          else if (_currentSymbol.CurrentSymbol == '\"')
                        {
                            state = 3;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else if (_currentSymbol.CurrentSymbol == ':')
                        {
                            state = 4;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else if (rw.operators.ContainsKey(_currentSymbol.CurrentSymbol.ToString()))
                        {

                            state = 5;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();

                        }

                        else if (_currentSymbol.CurrentSymbol == '\0')
                        {
                            state = 6;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            lexeme = "$";
                        }
                      
                       else if (_currentSymbol.CurrentSymbol == '$')
                        {
                            state = 7;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();

                        }
                        else if (_currentSymbol.CurrentSymbol == '%')
                        {
                            state = 8;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else if(_currentSymbol.CurrentSymbol == '#')
                        {
                            state = 10;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        } 
                        else if (_currentSymbol.CurrentSymbol == '{')
                        {
                            state = 11;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            _currentSymbol = Content.nextSymbol();
                        } 
                        else if (_currentSymbol.CurrentSymbol == '/')
                        {
                            state = 12;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                        }
                        else if (_currentSymbol.CurrentSymbol == '%')
                        {
                            state = 14;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            _currentSymbol = Content.nextSymbol();

                        }                 
                        
                        else
                        {
                            throw new LexicalException($"Symbol {_currentSymbol.CurrentSymbol} not recognized at Row:{_currentSymbol.Row} Col: {_currentSymbol.Column}");
                        }
                        break;
                    case 1:
                        if (char.IsLetterOrDigit(_currentSymbol.CurrentSymbol))
                        {
                            state = 1;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
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
                        if (char.IsDigit(_currentSymbol.CurrentSymbol))
                        {
                            state = 2;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else if (_currentSymbol.CurrentSymbol == '.')
                        {
                            state = 9;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                            return new Token { Type = TokenTypes.NumericLiteral, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow };
                        }
                        break;
                    case 3:
                        if (_currentSymbol.CurrentSymbol != '\"')
                        {
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else if (_currentSymbol.CurrentSymbol == '\"')
                        {
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                            return new Token { Type = TokenTypes.StringLiteral, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow };
                        }
                        break;
                    case 4:

                       
                        if (_currentSymbol.CurrentSymbol == '=')
                        {
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
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
                                Type = TokenTypes.AsiggnationOp,
                                 Lexeme =  lexeme,
                                 Column = tokenColumn,
                                 Row = tokenColumn
                             
                            };
                        }
                 

                    case 5:
                        if ( rw.specialSymbols.Contains(_currentSymbol.CurrentSymbol))
                        {
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
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
                        
                    case 6:
                        return new Token { Type = TokenTypes.Eof, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow };
                    case 600:

                        _pascalMode = true;
                        return new Token { Type = TokenTypes.Html, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow };

                    case 7:
                        if (OnlyHexInString(_currentSymbol.CurrentSymbol.ToString()))
                        {
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                           
                            return new Token {Type = TokenTypes.Hexa, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow};
                        }
                        break;
                    case 8:
                        if (_currentSymbol.CurrentSymbol == '1' || _currentSymbol.CurrentSymbol == '0')
                        {
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else
                        {

                            return new Token { Type = TokenTypes.Binary, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow };
                        }
                        break;

                    case 9:
                        if (char.IsDigit(_currentSymbol.CurrentSymbol))
                        {
                            state = 9;
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                            return new Token { Type = TokenTypes.NumericLiteral, Lexeme = lexeme, Column = tokenColumn, Row = tokenRow };
                        }

                        break;
                    case 10:
                        if (char.IsDigit(_currentSymbol.CurrentSymbol))
                        {
                            lexeme += _currentSymbol.CurrentSymbol;
                            _currentSymbol = Content.nextSymbol();
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
                        if (_currentSymbol.CurrentSymbol == '\0')
                        {
                            state = 6;
                        }
                        else if (_currentSymbol.CurrentSymbol == '}')
                        {
                            state = 0;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                            _currentSymbol = Content.nextSymbol();
                        }
                        break;
                        
                    case 12:
                       var temp = Content.nextSymbol();
                        if (temp.CurrentSymbol == '/')
                        {
                            state = 13;
                            _currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                            _currentSymbol = temp;
                            return new Token { Type = TokenTypes.DivOp, Lexeme = "/", Column = tokenColumn, Row = tokenRow };
                        }
                        break;
                    case 13:

                         if (_currentSymbol.CurrentSymbol == '\0')
                        {
                            state = 6;
                            tokenColumn = _currentSymbol.Column;
                            tokenRow = _currentSymbol.Row;
                            lexeme = "$";
                        }
                        else if (tokenRow == _currentSymbol.Row)
                        {
                            _currentSymbol = Content.nextSymbol();
                        }
                        else
                        {
                            state = 0;
                           
                        }
                        break;
                    case 14:
                        if (_currentSymbol.CurrentSymbol == '>')
                        {
                            _currentSymbol = Content.nextSymbol();
                            state = 0;
                            _pascalMode = false;
                        }
                        else
                        {
                            throw new LexicalException("No se ha cerrado codigo pascal");
                        }

                    
                        break;
                }
            }

            throw new NotImplementedException();
        }
    }
}
