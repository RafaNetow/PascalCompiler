using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Handlers
{
    class HandlerFiles
    {
        private string defaultPath = "PascalCode.txt";
           
        public HandlerFiles()
        {
            
        }

        public HandlerFiles(string path)
        {
            defaultPath = path;
        }

        public string getCode()
        {
            string file = "";
            try
            {
                file = System.IO.File.ReadAllText(defaultPath);
            }
            catch (Exception e)

            {
                Console.Write(" No se ha encontrado el archivo");
                return "";
            }
            return file;

        }

        public void writeCode(string code)
        {
            string file = "";
            try
            {
               System.IO.File.WriteAllText("C:\\Users\\Sequeiros\\Documents\\Compi\\pascal\\pascal.java", code);
            }
            catch (Exception e)

            {
                Console.Write(" No se ha encontrado el archivo");
                
            }
            
        }
    }
    }
