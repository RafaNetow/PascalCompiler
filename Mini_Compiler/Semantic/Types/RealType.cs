using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Semantic.Types
{
    class RealType : BaseType
    {
        public override bool IsAssignable(BaseType otherType)
        {

            return otherType is RealType;
        }

        public override string GenerateCode()
        {
            return " double";
        }
    }
}
