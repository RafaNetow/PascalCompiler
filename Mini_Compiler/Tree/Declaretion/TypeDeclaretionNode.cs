﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree.Declaretion
{
    public abstract class TypeDeclaretionNode : SentencesNode
    {
       public string  Name;
        public abstract override string GenerateCode();



    }
}
