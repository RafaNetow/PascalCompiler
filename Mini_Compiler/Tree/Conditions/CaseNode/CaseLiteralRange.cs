using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree.CaseNode
{
    class CaseLiteralRange : CaseLiteral
    {
        public List<Range> LiteralRanges;
    }
}
