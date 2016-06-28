using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Semantic.Types
{
    class StringType : BaseType
    {
         
        public override bool IsAssignable(BaseType otherType)
        {
            return otherType is StringType;
        }

        public override string GenerateCode()
        {
            return " String";
        }
    }
}
