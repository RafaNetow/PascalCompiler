using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Lexer
{
    public class StringContent
    {
        private int _row;
        private int _column;
        private string _input;
        private int _currentIndex;

        public StringContent(string input)
        {
            _input = input;
            _currentIndex = 0;
            _row = 0;
            _column = 0;
        }
    
        public Symbol nextSymbol() {


            if (_currentIndex >= _input.Length)
                return new Symbol{Row = _row, Column = _column, CurrentSymbol = '\0' };

            Symbol currentSym = new Symbol { Row = _row, Column = _column, CurrentSymbol = _input[_currentIndex++] };

            if (currentSym.CurrentSymbol.Equals('\n'))
            {
                _column = 0;
                _row += 1;
            }
            else
            {
                _column += 1;
            }
                

            return currentSym;
        }


    }
}
