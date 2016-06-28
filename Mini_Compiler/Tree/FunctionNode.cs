using System;
using System.Collections.Generic;
using Mini_Compiler.Generate_Java;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public class FunctionNode : SentencesNode
    {
        public IdNode NameOfFunction  = new IdNode();
        public List<ParamsOfDeclaretion> Params = new List<ParamsOfDeclaretion>();
        public List<SentencesNode> BlockFunction = new List<SentencesNode>();
        public IdNode TypeOfReturn = new IdNode();
       public  PascalToJava convert = new PascalToJava();
        public ExpressionNode ReturParameter;


        public override void ValidateSemantic()
        {

            List<ParameterFunction> listParams = new List<ParameterFunction>();
   

         
        
            

            foreach (var param in Params)
            {

                foreach (var parameterFunction in param.Variables)
                {
                    var temp = TypesTable.Instance.GetType(param.TypeV.Value);
                    listParams.Add(new ParameterFunction { IsVar = param.IsDeclaretionVar, Type = temp });
                    SymbolTable.Instance.DeclareVariable(parameterFunction.Value,temp);
                }
                
            }
           




            foreach (var sentencesNode in BlockFunction)
            {
                sentencesNode.ValidateSemantic();
            }
            var Return = TypesTable.Instance.GetType(TypeOfReturn.Value);
            BaseType VariableToReturn = ReturParameter.ValidateSemantic();
            if (VariableToReturn != Return)
            {
                throw new Exception("The return parameter is wrong");
            }
            var functValue = TypesTable.Instance.GetType(TypeOfReturn.Value);
            SymbolTable.Instance.DeclareVariable(NameOfFunction.Value, new FunctionType(listParams, functValue));


        }

        public override string GenerateCode()
        {
            string blockParameter = "";
            string blockBody = "";
            int count = 0;
            if (convert.convertToJava.ContainsKey(TypeOfReturn.Value))
                TypeOfReturn.Value = convert.convertToJava[TypeOfReturn.Value];
            foreach (var typeOfParam in Params)
            {
                foreach (var param in typeOfParam.Variables)
                {
                    if (convert.convertToJava.ContainsKey(typeOfParam.TypeV.Value))
                        typeOfParam.TypeV.Value= convert.convertToJava[typeOfParam.TypeV.Value];
                        if (count == 0)
                    {
                        //if()

                        blockParameter = blockParameter + typeOfParam.TypeV.Value+" " + param.Value;
                        count++;
                    }
                    else
                    {
                        blockParameter = blockParameter+"," + typeOfParam.TypeV.Value +" " + param.Value+"\n";
                    }
                }
            }
            foreach (var sentencesNode in BlockFunction)
            {
               blockBody = blockBody+ sentencesNode.GenerateCode();
            }

            
            return "public"+" "+ TypeOfReturn.Value+" "+this.NameOfFunction.Value+"("+ blockParameter+"){"+blockBody+"return"+" "+ ReturParameter.GenerateCode()+";" + "}";
        }
    }
}