using System.Collections.Generic;
using Mini_Compiler.Semantic;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public class ProcedureNode : SentencesNode
    {
        public IdNode NameOfProcedure;
        public List<ParamsOfDeclaretion> Params;
        public List<SentencesNode> BlockProcedure;

        public override void ValidateSemantic()
        {
            List<ParameterFunction> listParams = new List<ParameterFunction>();
          

            foreach (var param in Params)
            {

                foreach (var parameterFunction in param.Variables)
                {
                    var temp = TypesTable.Instance.GetType(param.TypeV.Value);
                    listParams.Add(new ParameterFunction { IsVar = param.IsDeclaretionVar, Type = temp });
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
            throw new System.NotImplementedException();
        }
    }
}