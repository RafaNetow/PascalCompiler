using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Tree.Declaretion;

namespace Mini_Compiler.Tree.CaseNode
{
    abstract class CaseStatement
    {
        public List<SentencesNode> Statements { get; set; }

       
        
    }
}
