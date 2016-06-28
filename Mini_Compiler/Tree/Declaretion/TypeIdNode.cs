using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic;

namespace Mini_Compiler.Tree.Declaretion
{
    class TypeIdNode : TypeDeclaretionNode
    {
        public string TypeId;
        public override void ValidateSemantic()

        {
           var  type =  TypesTable.Instance.GetType(TypeId);
             SymbolTable.Instance.DeclareVariable(Name, type);
            
        }

        public override string GenerateCode()
        {

            return TypeId;
        }
    }
}
