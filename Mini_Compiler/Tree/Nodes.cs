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

    public class ProcedureNode : SentencesNode
    {
        public IdNode NameOfProcedure;
        public List<ParamsOfDeclaretion> Params;
        public List<SentencesNode> BlockProcedure;

    }

    public class FunctionNode : SentencesNode
    {
        public IdNode NameOfFunction  = new IdNode();
        public List<ParamsOfDeclaretion> Params = new List<ParamsOfDeclaretion>();
        public List<SentencesNode> BlockFunction = new List<SentencesNode>();
        public IdNode TypeOfReturn = new IdNode();
    }

    public class ConstNode : SentencesNode
    {
        public IdNode ConstName = new IdNode();
        public IdNode TypeOfConst = new IdNode();
        public ExpressionNode ExpressionConst = new IdNode();
    }


}
