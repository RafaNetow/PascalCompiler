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

        public override string GenerateCode()
        {
          var type =  FirstId.ValidateSemantic();
            string blockForIn = " ";
            foreach (var sentencesNode in ListSentences)
            {
                blockForIn = blockForIn + sentencesNode.GenerateCode();
            }

            var stringFor = "for (" + type.GenerateCode() + " " + FirstId.Value + " :" + " "+SecondId.Value+")";
            return stringFor + "{" + blockForIn + "}\n";

        }
    }
}