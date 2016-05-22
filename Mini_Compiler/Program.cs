using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Lexer;

namespace Mini_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleParser parser = new SampleParser(new Lexer.Lexer(new StringContent("1++2*3(")));
            parser.Parse();
            Console.ReadKey();
            Console.WriteLine("");
        }
    }
}
