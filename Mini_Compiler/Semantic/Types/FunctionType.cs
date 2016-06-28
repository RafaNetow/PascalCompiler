using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Semantic.Types
{
    public class FunctionType : BaseType
    {
        
            public override bool IsAssignable(BaseType otherType)
            {
            return false;
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }

        public List<ParameterFunction> _parameter;
        public readonly BaseType _functValue;


        public FunctionType(List<ParameterFunction> parameter, BaseType functValue)
        {
            this._parameter = parameter;
            _functValue = functValue;
        }
    }
}
