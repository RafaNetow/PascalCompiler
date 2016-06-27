using System.Collections.Generic;
using Mini_Compiler.Semantic;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public class WhileNode : SentencesNode
    {
        public ExpressionNode Condition;
        public List<SentencesNode> Sentences;

        public override void ValidateSemantic()
        {
            if (!(Condition.ValidateSemantic() is BooleanType))
                throw new SemanticException("Se esperaba expresion booleana en la sentencia while");

            foreach (var statement in Sentences)
            {
                statement.ValidateSemantic();
            }
        }

        public override string GenerateCode()
        {
            string blockWhile = "";
            foreach (var sentence in Sentences)
            {
                blockWhile = blockWhile + sentence.GenerateCode();
            }

            return "while(" + Condition.GenerateCode() + ")" + "{" + blockWhile + "}";
        }
    }
}