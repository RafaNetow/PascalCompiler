using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Mini_Compiler.Generate_Java;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Sintactico;
using TechTalk.SpecFlow;

namespace Mini_Compiler.Tree
{
    public class DeclarationNode : SentencesNode
    {
        public List<IdNode> LIstIdNode;
        public IdNode TypeId;
        public ExpressionNode ExpressionType;
        public bool Expression;

        public PascalToJava convert = new PascalToJava();

         

        public bool isPrimitive(BaseType type)
        {
           
            return type is IntType ||
                   type is BooleanType ||
                   type is CharType ||
                   type is RealType ||
                   type is StringType;


        }
    

    public BaseType getPrimType(BaseType type)
    {
        if (isPrimitive(type))
            return type;

        if (type is ArrayType)
        {

            var arrayType = (ArrayType) type;
            while ( true)
            {
                type = arrayType.Type;
                if (isPrimitive(type))
                    return type;
                arrayType = (ArrayType) type;

            }
        }
        return null;

         }
       
        public override void ValidateSemantic()
        {
            BaseType typeId;
            


                 typeId = TypesTable.Instance.GetType(TypeId.Value);
            
            if (Expression)
            {
                var name = LIstIdNode[0];


               
                SymbolTable.Instance.DeclareVariable(name.Value, typeId);


                var typeOfExpression = ExpressionType.ValidateSemantic();


                
                if (typeId != getPrimType(typeOfExpression))
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

        public BaseType getType(ArrayType typ)
        {
            BaseType dimensions;

            while (true)
            {


                if (isPrimitive(typ.Type))
                {
                    return dimensions = typ.Type;

                }




                typ = (ArrayType) typ.Type;

            }
        }

        public override string GenerateCode()
        {
            string type = "";
             type = TypeId.Value;
            if (convert.convertToJava.ContainsKey(type))
                type = convert.convertToJava[type];
            string variables = "";
            int count = 0;
            if (Expression)
            {
                


                return type + " " + this.LIstIdNode[0].Value + "=" + ExpressionType.GenerateCode() + ";";

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
              var  typeId = TypesTable.Instance.GetType(TypeId.Value);
                if (typeId is RecordType)
                {
                    return type + " " + variables + " " + "=" + "new " + type + "();";
                }
                if (typeId is ArrayType)
                {
                    var typeArray = (ArrayType) typeId;
                  var primitiveType =   getType(typeArray);
                    return primitiveType.GenerateCode() + "[]" + variables+ "="+ type+";";
                }
                return type + " " + variables + ";";

            }
            
        }
    }
}