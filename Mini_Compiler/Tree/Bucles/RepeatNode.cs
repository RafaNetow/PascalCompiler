using System.Collections.Generic;
using Mini_Compiler.Semantic;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public class RepeatNode : SentencesNode
    {
        public ExpressionNode Condition;
        public List<SentencesNode> ListSentences;
        public override void ValidateSemantic()
        {
            if (!(Condition.ValidateSemantic() is BooleanType))
                throw new SemanticException("Se esperaba expresion booleana en la sentencia while");

            foreach (var statement in ListSentences)
            {
                statement.ValidateSemantic();
            }
        }

        public override string GenerateCode()
        {

            string repeatBlock = "";
            foreach (var sentence in ListSentences)
            {
                repeatBlock = repeatBlock + sentence.GenerateCode();
            }
            return "do{" + repeatBlock +"}"+"while("+Condition.GenerateCode()+");";
    }
    }
}