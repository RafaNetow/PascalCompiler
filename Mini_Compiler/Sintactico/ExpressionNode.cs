using Mini_Compiler.Semantic.Types;

namespace Mini_Compiler.Sintactico
{
    public abstract class ExpressionNode
    {
        public abstract BaseType ValidateSemantic();
    }
}