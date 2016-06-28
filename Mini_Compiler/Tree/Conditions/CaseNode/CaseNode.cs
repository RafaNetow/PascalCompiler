using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree.CaseNode
{
    class CaseNode : SentencesNode
    {
        public IdNode CaseName = new IdNode();

        public List<CaseStatement> CaseStatements;

        public override void ValidateSemantic()
        {
            
           var typeNode =  CaseName.ValidateSemantic();

            if ( typeNode is IntType)
            {
                foreach (var caseStatement in CaseStatements)
                {
                    foreach (var statement in caseStatement.Statements)
                    {
                        statement.ValidateSemantic();
                    }
                }
                }
                 
            }

        public override string GenerateCode()
        {

            string blockCase = "";
            int count = 1;
            foreach (var caseStatement in CaseStatements)
            {
                blockCase = blockCase+"case " +count+":" + caseStatement.Statements[0].GenerateCode()+"break;\n";
                count++;
            }

            return "switch("+CaseName.Value+"){"+blockCase+"}";
        }
    }

       
    }

