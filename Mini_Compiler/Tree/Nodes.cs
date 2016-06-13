using System.Collections.Generic;
using System.Runtime.Remoting;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public abstract class SentencesNode
    {
        


   
    }


    public class IfNode :SentencesNode
    {
        public ExpressionNode IfCondition;
        public List<SentencesNode> TrueBlock;
        public List<SentencesNode> FalseBlock;

    }
    public  class PreIdNode : SentencesNode
    {
       public bool IsAfunction;
        public List<ExpressionNode> ListExpressionNodes;
        public IdNode Variable;



    }

    

   public class DeclarationNode :SentencesNode
   {
       public List<IdNode>LIstIdNode;
       public IdNode TypeId;
       public ExpressionNode ExpressionType;
       public bool Expression;
   }

    public class WhileNode : SentencesNode
    {
        public ExpressionNode Condition;
        public List<SentencesNode> Sentences;

    }

    public class ForNode : SentencesNode
    {
        public ExpressionNode FirstCondition;
        public ExpressionNode SecondCondition;
        public List<SentencesNode> Sentences;

        public IdNode FirstIdOfCondition;
    }

    public class ForInNode : SentencesNode
    {
        public IdNode FirstId;
       
        public  List<SentencesNode> ListSentences;
        public IdNode SecondId { get; set; }
    }

    public class RepeatNode : SentencesNode
    {
        public ExpressionNode Condition;
        public List<SentencesNode> ListSentences;
    }

    public class procedureNode : SentencesNode
    {
        public IdNode nameOfProcedure;
        public bool varDeclaretion;
        public List<IdNode> firstId;
        public IdNode secondId;
    }

}
