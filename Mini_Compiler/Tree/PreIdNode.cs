using System;
using System.Collections.Generic;
using System.Reflection;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public  class PreIdNode : SentencesNode
    {
        public bool IsAfunction;
        public List<ExpressionNode> ListExpressionNodes;
        public IdNode Variable;


        public override void ValidateSemantic()
        {
            if (IsAfunction)
            {

                var type = SymbolTable.Instance.GetVariable(Variable.Value);
                int count = 0;


                if (type is FunctionType)
                {

                    var function = (FunctionType) type;

                    if (function._parameter.Count != ListExpressionNodes.Count)
                    {
                        throw new Exception("Cant of function is wrong");
                    }

                    foreach (var expressionNode in ListExpressionNodes)
                    {

                        if (!expressionNode.ValidateSemantic().IsAssignable(function._parameter[count].Type))
                        {
                            throw new Exception("Parameter Type is wrong");
                        }
                        count++;
                    }


                }
                else
                {
                    throw new Exception("Is not a function");
                }

            }
            else
            {
                var name = Variable.ValidateSemantic();
                var valueType = ListExpressionNodes[0].ValidateSemantic();
                if (!name.IsAssignable(valueType) )
                {
                    throw new SemanticException("Tipos incompatibles entre si.");
                }

            }
            

            
        }

        public override void GenerateCode()
        {
            throw new System.NotImplementedException();
        }
    }
}