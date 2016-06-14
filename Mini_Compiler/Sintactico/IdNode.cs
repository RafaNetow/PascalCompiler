using System.Collections.Generic;
using Mini_Compiler.Tree;

namespace Mini_Compiler.Sintactico
{
    public class IdNode : BinaryOperatorNode
    {
        public string Value { get; set; }
        public List<AccesorNode> Accesors = new List<AccesorNode>();
    }
}