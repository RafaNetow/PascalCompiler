using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Sintactico
{
    class BinaryOperatorNode :ExpressionNode
    {
        public ExpressionNode RightOperand;
        public ExpressionNode LeftOperand;
    }
}
