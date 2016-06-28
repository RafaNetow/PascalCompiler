using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Generate_Java;
using Mini_Compiler.Handlers;
using Mini_Compiler.Lexer;
using Mini_Compiler.Sintactico;
using Mini_Compiler.Tree;
using Mini_Compiler.Tree.Declaretion;

namespace Mini_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {

            GenerateMain generatePascal = new GenerateMain();
            HandlerFiles handlFile = new HandlerFiles();
            string pascalCode = handlFile.getCode();
            string firstCode = " ";

            //Declaraciones



              Syntactic parser = new Syntactic(new Lexer.Lexer(new StringContent(pascalCode.ToLower())));

            
            try
            {
                var treeNodes = parser.Parse();
                var javaCode = string.Empty;
                foreach (var sentencesNode in treeNodes)
                {
                    sentencesNode.ValidateSemantic();
                    if (sentencesNode is ProcedureNode || sentencesNode is FunctionNode || sentencesNode is RecordNode ||
                        sentencesNode is EnumerateNode)
                        firstCode += sentencesNode.GenerateCode();
                    else
                    {


                        javaCode += sentencesNode.GenerateCode();
                    }

                }

                Console.WriteLine(generatePascal.ReturnCode(javaCode,firstCode));
                handlFile.writeCode(generatePascal.ReturnCode(javaCode,firstCode));
     


                Console.WriteLine("errors no founds");
                Console.ReadKey();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ReadKey();
            }
            System.Diagnostics.Process.Start("C:\\Users\\Sequeiros\\Documents\\Compi\\pascal.bat");




        }

    }
}
