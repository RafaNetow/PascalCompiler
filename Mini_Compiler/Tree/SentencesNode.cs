using System.Runtime.Remoting;

namespace Mini_Compiler.Tree
{
    public abstract class SentencesNode
    {
        public abstract void ValidateSemantic();
        public abstract string GenerateCode();



    }
}
