using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        public override void GenerateCode()
        {
            throw new System.NotImplementedException();
        }
    }
}