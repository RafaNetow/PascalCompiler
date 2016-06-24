using Mini_Compiler.Semantic.Types;

namespace Mini_Compiler.Tree
{
    public class BooleanType : BaseType
    {
        public override bool IsAssignable(BaseType otherType)
        {
            return otherType is BooleanType;
        }
    }
}