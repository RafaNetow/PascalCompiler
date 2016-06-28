using System.Collections.Generic;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;

namespace Mini_Compiler.Tree
{
    public class ProceureType : BaseType
    {
        public override bool IsAssignable(BaseType otherType)
        {
            return false;
        }

        public override string GenerateCode()
        {
            throw new System.NotImplementedException();
        }

        public List<ParameterFunction> _parameter;
        public ProceureType(List<ParameterFunction> parameter)
        {
            this._parameter = parameter;
           
        }
    }
}