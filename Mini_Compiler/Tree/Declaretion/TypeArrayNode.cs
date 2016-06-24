using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree.Declaretion
{
    class TypeArrayNode : TypeDeclaretionNode
    {

      public  List<Range> ListRange = new List<Range>();
        



        public override void ValidateSemantic()
        {
            throw new NotImplementedException();
        }

        public override void GenerateCode()
        {
            throw new NotImplementedException();
        }
    }
}
