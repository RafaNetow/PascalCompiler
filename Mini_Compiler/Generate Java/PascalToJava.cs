using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Generate_Java
{
    public class PascalToJava
    {
      public  Dictionary<string, string> convertToJava = new Dictionary<string, string>();

        public PascalToJava()
        {
            convertToJava.Add("integer","int");
            convertToJava.Add("real", "double");
            convertToJava.Add("string", "String");


        }
    }
}
