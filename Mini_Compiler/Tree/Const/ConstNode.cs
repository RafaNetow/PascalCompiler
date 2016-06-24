using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public class ConstNode : SentencesNode
    {
        public IdNode ConstName = new IdNode();
        public IdNode TypeOfConst = new IdNode();
        public ExpressionNode ExpressionConst = new IdNode();
        public override void ValidateSemantic()
        {

           var   type = ExpressionConst.ValidateSemantic();
        }

        public override void GenerateCode()
        {
            throw new System.NotImplementedException();
        }
    }
}