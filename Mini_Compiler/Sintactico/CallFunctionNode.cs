using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Tree;
using TechTalk.SpecFlow;

namespace Mini_Compiler.Sintactico
{
    class CallFunctionNode : ExpressionNode
    {
        public List<ExpressionNode> List;
        public string Name;


        
        public override BaseType ValidateSemantic()
        {
            var type = SymbolTable.Instance.GetVariable(Name);

            int count = 0;

            
            if (type is FunctionType)
            {

                var function = (FunctionType) type;

                if (function._parameter.Count != List.Count)
                {
                    throw new Exception("Cant of function is wrong");
                }
               
                foreach (var expressionNode in List)
                {

                    if (!expressionNode.ValidateSemantic().IsAssignable(function._parameter[count].Type) )
                    {
                        throw  new Exception("Parameter Type is wrong");
                    }

                    count++;
                }
                return function._functValue;

            }
            else
            {
                throw  new Exception("Is not a function");
            }

            
            
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }
}
