using System.Collections.Generic;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Tree;

namespace Mini_Compiler.Sintactico
{
    public class IdNode : BinaryOperatorNode
    {
        public string Value { get; set; }
        public List<AccesorNode> Accesors = new List<AccesorNode>();
        public override BaseType ValidateSemantic()
        {
          var type = SymbolTable.Instance.GetVariable(Value);
          

            foreach (var variable in Accesors)
            {
               type = variable.Validate(type);

            }
            return type;
            
        }
    }
}