using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree.Declaretion
{
    class TypeArrayNode : TypeDeclaretionNode
    {

      public  List<Range> ListRange = new List<Range>();

        public string TypeArray;
              

        public override void ValidateSemantic()
        {
            var type = TypesTable.Instance.GetType(this.TypeArray);

            foreach (var range in ListRange)
            {
              type  =  new ArrayType(range,type);
            }

            TypesTable.Instance.RegisterType(this.Name, type);

        }

        public override string GenerateCode()
        {

            return null;
        }
    }
}
