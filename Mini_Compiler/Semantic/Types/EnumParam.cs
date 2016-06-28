using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Semantic.Types
{
    class EnumParam : BaseType
    {
        public string Name;
        public override bool IsAssignable(BaseType otherType)
        {
            return false;
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }
}
