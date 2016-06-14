using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Handlers;
using Mini_Compiler.Lexer;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {

          
            HandlerFiles handlFile = new HandlerFiles();
            string pascalCode = handlFile.getCode();
            
            //Declaraciones



            Syntactic parser = new Syntactic(new Lexer.Lexer(new StringContent(pascalCode.ToLower())));
          var treeNodes =   parser.Parse();
            treeNodes.Reverse();
            Console.WriteLine("errors no founds");
            Console.ReadKey();
            
        }

    }
}
