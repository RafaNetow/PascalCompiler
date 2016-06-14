using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Semantic.Types
{
    
        public abstract class BaseType
        {
            public abstract bool IsAssignable(BaseType otherType);
        }
    
}
