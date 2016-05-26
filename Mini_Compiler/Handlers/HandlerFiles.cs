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
    }
    }
