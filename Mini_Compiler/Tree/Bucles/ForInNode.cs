using System.Collections.Generic;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    public class ForInNode : SentencesNode
    {
        public IdNode FirstId;
       
        public  List<SentencesNode> ListSentences;
        public IdNode SecondId { get; set; }
        public override void ValidateSemantic()
        {
            
            foreach (var statement in ListSentences)
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