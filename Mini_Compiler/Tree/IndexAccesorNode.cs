using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    class IndexAccesorNode : AccesorNode
    {
        public  ExpressionNode IndexExpression { get; set; }
        public override BaseType Validate(BaseType type)
        {
            if (type is ArrayType)
            {
                var arrayType = (ArrayType)type;
                return arrayType.Type;


            }
            else
            {
                throw new Exception("Error isn't array ");
            }
                 
          
          

        }
    }
}
