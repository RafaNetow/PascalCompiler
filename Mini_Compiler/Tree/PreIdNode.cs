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
        public bool IsAProcedure;
        public List<ExpressionNode> ListExpressionNodes;
        public IdNode Variable;
        public ExpressionNode ExpressionAssigned;


        public override void ValidateSemantic()
        {
            if (IsAProcedure)
            {

                var type = SymbolTable.Instance.GetVariable(Variable.Value);
                int count = 0;


                if (type is ProceureType)
                {

                    var function = (ProceureType) type;

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
                    throw new Exception("Is not a procedure");
                }

            }
            else
            {


                var typeVariable = SymbolTable.Instance.GetVariable(Variable.Value);
                var typeOfAssigned = ExpressionAssigned.ValidateSemantic();
                if (typeVariable is EnumerateType)
                {
                    var typeToAssign = ExpressionAssigned.ValidateSemantic();
                    if (typeToAssign is EnumParam)
                    {
                        var castTypeToAssign = (EnumParam)typeToAssign;
                       var castTypeVariable = (EnumerateType) typeVariable;

                        if(!castTypeVariable.ListOfParams.Contains(castTypeToAssign.Name))
                            throw  new SemanticException(" No se puede asignar el valor al enum");
                    }
                    else
                    {
                        throw new SemanticException("el tipo de asignacion no esta permita");
                    }


                }

               
                else if (typeOfAssigned is ConstType)
                {
                    throw new SemanticException("no se puede asignar a una constante.");
                }
                else
                {

                     typeOfAssigned = ExpressionAssigned.ValidateSemantic();
                    var valueType = ListExpressionNodes[0].ValidateSemantic();
                    if (!typeOfAssigned.IsAssignable(valueType))
                    {
                        throw new SemanticException("Tipos incompatibles entre si.");
                    }

                }


            }
            

            
        }

        public override string GenerateCode()
        {


            if (IsAProcedure)
            {
                string listParameters = "";
                int count = 0;
                if (this.Variable.Value == "writeln")
                {
                    return "out.println(" + ListExpressionNodes[0].GenerateCode() + ");";
                }
                foreach (var listExpressionNode in ListExpressionNodes)
                {
                    if (count == 0)
                    {
                        listParameters = listParameters + listExpressionNode.GenerateCode();
                        count++;
                    }
                    else
                    {
                        listParameters = listParameters + "," + listExpressionNode.GenerateCode();
                    }
                }

                return this.Variable.Value + "(" + listParameters + ");";
            }
            else
            {

                return Variable.GenerateCode() + " " + "=" + ExpressionAssigned.GenerateCode() + ";\n";
            }
        }
    }
}