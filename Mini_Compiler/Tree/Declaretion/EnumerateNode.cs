using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree.Declaretion
{
    class EnumerateNode : TypeDeclaretionNode
    {
        
        public  List<IdNode> list;
        public override void ValidateSemantic()
        {
            var enumParams = new List<string>();
            foreach (var param in list)
            {
                enumParams.Add(param.Value);
                SymbolTable.Instance.DeclareVariable(param.Value,new EnumParam () {Name = param.Value});
            }


            SymbolTable.Instance.DeclareVariable(this.Name,new EnumerateType {ListOfParams = enumParams});
            

        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }
}
