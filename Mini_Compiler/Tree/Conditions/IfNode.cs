using System.Collections.Generic;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public class IfNode :SentencesNode
    {
        public ExpressionNode IfCondition;
        public List<SentencesNode> TrueBlock;
        public List<SentencesNode> FalseBlock;

        public override void ValidateSemantic()
        {
            if (IfCondition.ValidateSemantic() is BooleanType) 
            {
                
            }
            foreach (var sentencesNode in TrueBlock)
            {
                sentencesNode.ValidateSemantic();
            }
            foreach (var sentencesNode in FalseBlock)
            {
                sentencesNode.ValidateSemantic();
            }
        }

        public override void GenerateCode()
        {
            throw new System.NotImplementedException();
        }
    }
}