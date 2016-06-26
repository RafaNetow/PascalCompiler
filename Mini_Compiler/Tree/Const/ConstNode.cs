using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public class ConstNode : SentencesNode
    {
        public IdNode ConstName = new IdNode();
        public IdNode TypeOfConst = new IdNode();
        public ExpressionNode ExpressionConst = new IdNode();
        public bool declare;
        public override void ValidateSemantic()
        {

            if (declare)
            {


                var type = TypesTable.Instance.GetType(TypeOfConst.Value);
                SymbolTable.Instance.DeclareVariable(ConstName.Value, new ConstType() {TypeConst = type} );
            }
            else
            {
                var type = ExpressionConst.ValidateSemantic();
                SymbolTable.Instance.DeclareVariable(ConstName.Value, type);


            }
        }

        public override string GenerateCode()
        {
            throw new System.NotImplementedException();
        }
    }
}