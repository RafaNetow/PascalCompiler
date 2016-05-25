﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Lexer;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declaraciones
            Syntactic parser = new Syntactic(new Lexer.Lexer(new StringContent(" var a, b : id = 5++5 ")));
            parser.Parse();
            Console.ReadKey();
            Console.WriteLine("");
        }
    }
}