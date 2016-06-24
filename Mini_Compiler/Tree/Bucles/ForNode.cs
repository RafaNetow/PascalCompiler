using System.Collections.Generic;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public class ForNode : SentencesNode
    {
        public ExpressionNode FirstCondition;
        public ExpressionNode SecondCondition;
        public List<SentencesNode> Sentences;

        public IdNode FirstIdOfCondition;
        public override void ValidateSemantic()
        {
            if (!(FirstCondition.ValidateSemantic() is IntType))
            {
                throw new SemanticException("Expected integer variable  ");
            }
            if (!(SecondCondition.ValidateSemantic() is IntType))
            {
                throw new SemanticException("Expected integer variable ");
            }
            

            foreach (var statement in Sentences)
            {
                statement.ValidateSemantic();
            }
        }

        public override void GenerateCode()
        {
            throw new System.NotImplementedException();
        }
    }
    }
