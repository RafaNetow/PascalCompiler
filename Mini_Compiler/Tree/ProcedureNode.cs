using System.Collections.Generic;
using Mini_Compiler.Generate_Java;
using Mini_Compiler.Semantic;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public class ProcedureNode : SentencesNode
    {
        public IdNode NameOfProcedure;
        public List<ParamsOfDeclaretion> Params;
        public List<SentencesNode> BlockProcedure;
        public  PascalToJava convert = new PascalToJava();


        public override void ValidateSemantic()
        {
            List<ParameterFunction> listParams = new List<ParameterFunction>();
          

            foreach (var param in Params)
            {

                foreach (var parameterFunction in param.Variables)
                {
                    var temp = TypesTable.Instance.GetType(param.TypeV.Value);
                    listParams.Add(new ParameterFunction { IsVar = param.IsDeclaretionVar, Type = temp });
                    SymbolTable.Instance.DeclareVariable(param.TypeV.Value, temp);

                }

            }

            SymbolTable.Instance.DeclareVariable(NameOfProcedure.Value, new ProceureType(listParams));

            foreach (var sentencesNode in BlockProcedure)
            {
                sentencesNode.ValidateSemantic();
            }

        }

        public override string GenerateCode()
        {
            int count = 0;
            string blockParameter = " ";
            string blockBody = " ";
            foreach (var typeOfParam in Params)
            {
                foreach (var param in typeOfParam.Variables)
                {
                    if (convert.convertToJava.ContainsKey(typeOfParam.TypeV.Value))
                        typeOfParam.TypeV.Value = convert.convertToJava[typeOfParam.TypeV.Value];
                    if (count == 0)
                    {

                        blockParameter = blockParameter + typeOfParam.TypeV.Value + " " + param.Value;
                        count++;
                    }
                    else
                    {
                        blockParameter = blockParameter + " , " + typeOfParam.TypeV.Value + " " + param.Value;
                    }
                }
            }
            foreach (var sentencesNode in this.BlockProcedure)
            {
                blockBody = blockBody + sentencesNode.GenerateCode();
            }
            

            return " void "+this.NameOfProcedure.Value+"("+blockParameter+"){"+blockBody+"}";
        }
    }
}