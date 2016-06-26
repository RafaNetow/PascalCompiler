using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Mini_Compiler.Semantic;
using Mini_Compiler.Sintactico;
using TechTalk.SpecFlow;

namespace Mini_Compiler.Tree
{
    public  class DeclarationNode :SentencesNode
    {
        public List<IdNode> LIstIdNode;
        public IdNode TypeId;
        public ExpressionNode ExpressionType;
        public bool Expression;
        public override void ValidateSemantic()
        {
            var typeId = TypesTable.Instance.GetType(TypeId.Value);
            if (Expression)
            {
                var name = LIstIdNode[0];


               
                SymbolTable.Instance.DeclareVariable(name.Value, typeId);

               

                if (typeId != ExpressionType.ValidateSemantic())
                {
                    throw new Exception("Parameter dnt equals");
                }
            }
            else
            {

                foreach (var idNode in LIstIdNode)
                {
                    SymbolTable.Instance.DeclareVariable(idNode.Value, typeId);
                }
                
            }
        }

        public override string GenerateCode()
        {
            string type = "";
             type = TypeId.Value;
            string variables = "";
            int count = 0;
            if (Expression)
            {
                


                return type + " " + this.LIstIdNode[0] + "=" + ExpressionType.GenerateCode() + ";";

            }
            else
            {
                foreach (var idNode in LIstIdNode)
                {
                    if (count == 0) { 
                        variables = idNode.Value;
                        count++;
                    }
                    else
                    {
                        variables = variables+"," + idNode.Value;
                        
                    }


                }
                return type + " " + variables + ";";

            }
            
        }
    }
}