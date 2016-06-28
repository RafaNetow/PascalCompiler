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
            if (TypesTable._instance._table.ContainsKey(Value))
            {
                return TypesTable.Instance.GetType(Value);
            }
            var type = SymbolTable.Instance.GetVariable(Value);
          

            foreach (var variable in Accesors)
            {
               type = variable.Validate(type);

            }
            return type;
            
        }

        public override string GenerateCode()
        {
            if(Accesors.Count == 0)
            return $"{Value}";
            string accesors = "";
            foreach (var accesorNode in Accesors)
            {
                accesors = accesors + accesorNode.GeneratedCodeAttribute();

            
            }
            return this.Value+accesors;

        }
    }
}