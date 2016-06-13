using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler
{
    public class ParamsOfDeclaretion
    {
       public List<IdNode> Variables = new List<IdNode>();
       public IdNode TypeV = new IdNode();
       public bool IsDeclaretionVar;


    }
}
