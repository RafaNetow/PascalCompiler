using System.Collections.Generic;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public abstract class SentencesNode
    {
        


   
    }


    public class IfNode :SentencesNode
    {
        public ExpressionNode _ifCondition;
        public List<SentencesNode> _trueBlock;
        public List<SentencesNode> _falseBlock;

    }

   public class DeclarationNode :SentencesNode
   {
       public List<IdNode>LIstIdNode;
       public IdNode TypeId;
       public ExpressionNode ExpressionType;
       public bool Expression;




   }


}
