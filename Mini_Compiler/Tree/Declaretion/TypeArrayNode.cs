using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Sintactico;

namespace Mini_Compiler.Tree.Declaretion
{
    class TypeArrayNode : TypeDeclaretionNode
    {
       
      public  List<Range> ListRange = new List<Range>();

        public string TypeArray;

      
        public override void ValidateSemantic()
        {
            var type = TypesTable.Instance.GetType(this.TypeArray);

            foreach (var range in ListRange)
            {
              type  =  new ArrayType(range,type);
            }

            TypesTable.Instance.RegisterType(this.Name, type);

        }

        public List<string> GetDimensione( ArrayType type)
        {
      
            List<string> dimensions = new List<string>();
            
            while (true)
                {

                dimensions.Add(type.Dimension.Super.GenerateCode());
                if (isPrimitive(type.Type))
                    {
                        return dimensions;

                    }



               
                type = (ArrayType)type.Type;

                }
            
           

        }

        public bool isPrimitive(BaseType type)
        {
            return type is BooleanType ||
                   type is IntType ||
                   type is StringType ||
                   type is CharType ||
                   type is RealType;

        }

        public bool isClass(BaseType type )
        {
            return TypesTable._instance._table.ContainsValue(type);
        }




        public override string GenerateCode()
        {



          

            List<string> dimensionArray = new List<string>();


            var type = TypesTable.Instance.GetType(this.TypeArray);

            
          /*  if (isPrimitive(type))
            {
            */
                

                foreach (var range in ListRange)
                {
                    type = new ArrayType(range, type);
                }

               dimensionArray =  this.GetDimensione((ArrayType) type);
            dimensionArray.Reverse();

                string dimensionCode = "";
            
                foreach (var variable in dimensionArray)
                {
                    dimensionCode = dimensionCode+"[" + variable + "]";
                }
                
                return  this.TypeArray+ " "+this.Name+ "="+" "+"new" + " " + this.TypeArray + dimensionCode+";" ;
            
           
        }
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public List<string> GetRange(List<Range>list )
        {
             List<string>supRanges = new List<string>();
            foreach (var range in list)
            {    
                supRanges.Add(range.Super.GenerateCode());
            }

            return null;
        }
    }
}
