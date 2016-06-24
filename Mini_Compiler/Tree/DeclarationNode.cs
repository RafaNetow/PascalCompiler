using System;
using System.Collections.Generic;
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

        public override void GenerateCode()
        {
            throw new System.NotImplementedException();
        }

    }
}