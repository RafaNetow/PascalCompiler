using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;

namespace Mini_Compiler.Sintactico
{
    public abstract class BinaryOperatorNode :ExpressionNode
    {
        public ExpressionNode RightOperand;
        public ExpressionNode LeftOperand;
        public Dictionary<Tuple<BaseType, BaseType>, BaseType> Validation;

        public  override BaseType ValidateSemantic()
        {
            var leftType = LeftOperand.ValidateSemantic();
            var rightType = RightOperand.ValidateSemantic();

            BaseType result;
            if (Validation.TryGetValue(new Tuple<BaseType, BaseType>(leftType, rightType), out result))
            // Returns true.
            {
                return result;

            }
            else
            {
                throw new SemanticException("No se puede realizar esta operacion");
            }

        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }
}
