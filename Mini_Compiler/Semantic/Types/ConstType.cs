using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Semantic.Types
{
    class ConstType : BaseType
    {
       public BaseType TypeConst;
        public override bool IsAssignable(BaseType otherType)
        {
          
            return otherType == TypeConst;
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }
}
