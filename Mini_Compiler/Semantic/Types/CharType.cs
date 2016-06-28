using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Semantic.Types
{
    class CharType : BaseType
    {
        public override bool IsAssignable(BaseType otherType)
        {

            return otherType is CharType;
        }

        public override string GenerateCode()
        {
            return " char";
        }
    }
}
