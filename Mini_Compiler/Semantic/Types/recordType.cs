using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Tree.Declaretion;

namespace Mini_Compiler.Semantic.Types
{
    class RecordType : BaseType
    {
        public string name;
        public List<RecordProperties> RecordProperties = new List<RecordProperties>();
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
