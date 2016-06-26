using System.Collections.Generic;
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
           

   

        public override void ValidateSemantic()
        {

            List<ParameterFunction> listParams = new List<ParameterFunction>();
            var functValue =   TypesTable.Instance.GetType(TypeOfReturn.Value);

            foreach (var param in Params)
            {

                foreach (var parameterFunction in param.Variables)
                {
                    var temp = TypesTable.Instance.GetType(param.TypeV.Value);
                    listParams.Add(new ParameterFunction { IsVar = param.IsDeclaretionVar, Type = temp });
                }
                
            }

            SymbolTable.Instance.DeclareVariable(NameOfFunction.Value,new FunctionType(listParams,functValue));

            foreach (var sentencesNode in BlockFunction)
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