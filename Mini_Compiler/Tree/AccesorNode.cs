using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Mini_Compiler.Semantic.Types;

namespace Mini_Compiler.Tree
{
   public  abstract class AccesorNode
   {
       public abstract BaseType Validate(BaseType type);
       public abstract  string GeneratedCodeAttribute();
   }
}
