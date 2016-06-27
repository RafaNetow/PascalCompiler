using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree
{
    class PropertyAccesorNode : AccesorNode
    {
        public IdNode IdNode { get; set; }
        public override BaseType Validate(BaseType type)
        {
            var idNodeType= TypesTable.Instance.GetType(IdNode.Value);


            return idNodeType;
        }
    }
}
