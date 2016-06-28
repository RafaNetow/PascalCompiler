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
        public bool isPrimitive(BaseType type)
        {
            return type is BooleanType ||
                   type is IntType ||
                   type is StringType ||
                   type is CharType ||
                   type is RealType;

        }

        public override string GenerateCode()
        {
            string parameter = " ";

            var type = SymbolTable.Instance.GetVariable(Name);
          var function  = (FunctionType)type;
            List<string> value = new List<string>();
            string code = " ";
            
            
            int count = 0;
            // var function = (FunctionType) type;
            
            if (this.Name == "getformdata")
            {
                return "request.getParameter(" + this.List[0].GenerateCode() + ");";
            }
            else
            {
                foreach (var expressionNode in List)
                {
                    if (function._parameter[0].IsVar)
                    {
                        if (count == 0)
                        {

                            parameter = parameter + expressionNode.GenerateCode();
                            count++;
                        }
                        else
                        {
                            count++;
                            parameter = parameter + "," + expressionNode.GenerateCode();
                        }
                    }
                    else
                    {
                        if (function._parameter[count].Type is ArrayType)
                        {
                            parameter = parameter + "," +"("+ expressionNode.GenerateCode()+")"+"."+"clone()";
                            count++;
                        }
                       else if (function._parameter[count].Type is RecordType)
                       {
                           var record = (RecordType) type;
                           parameter = parameter + "," + "new " + record.name + "(" + expressionNode.GenerateCode() +
                                       ");";

                           count++;


                       }
                       else
                       {
                            count++;
                            parameter = parameter + "," + expressionNode.GenerateCode();
                        }

                    }
                }


                return  this.Name+"("+parameter+")\n";

            }
        }
    }
}
