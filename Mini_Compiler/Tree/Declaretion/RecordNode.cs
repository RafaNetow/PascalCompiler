using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Generate_Java;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;

namespace Mini_Compiler.Tree.Declaretion
{
    class RecordNode : TypeDeclaretionNode
    {
        public List<RecordProperties> RecordProperties;
        PascalToJava conver  = new PascalToJava();
        public override void ValidateSemantic()
        {
            foreach (var recordPropertiese in RecordProperties)
            {
                recordPropertiese.Type.Name = recordPropertiese.ListId[0].Value;
                recordPropertiese.Type.ValidateSemantic();


            }
            TypesTable.Instance.RegisterType(Name, new RecordType { RecordProperties = RecordProperties, name  = this.Name});
        }
       
        public override string GenerateCode()
        {
            string blockProperties = " ";

            string type = " ";
            foreach (var sentences in RecordProperties)
            {

                if (conver.convertToJava.ContainsKey(sentences.Type.GenerateCode()))
                {
                    type = conver.convertToJava[sentences.Type.GenerateCode()];


                }
                else
                {
                    type = sentences.Type.GenerateCode();
                }
                foreach (var id in sentences.ListId)
                {
                    if (sentences.Type is RecordNode)
                    {

                        blockProperties = blockProperties + " " + type;
                        blockProperties = blockProperties + " " + sentences.Type.Name + " " + id.Value + "xyz " + "=" + " " + "new" +
                                          " " + sentences.Type.Name + "();\n";

                    }
                    else
                    {
                        blockProperties = blockProperties + type + " " + id.Value + ";\n";
                    }
                }

            }
           
            foreach (var sentences in RecordProperties)
            {
                if (conver.convertToJava.ContainsKey(sentences.Type.GenerateCode()))
                {
                    type = conver.convertToJava[sentences.Type.GenerateCode()];
                }
                else
                {
                    type = sentences.Type.GenerateCode();
                }
                

            }


            return " class " + this.Name + "{" + blockProperties + "}";
        }


     
      /*
        public override string GenerateCode()
        {
            string blockProperties = " ";

            string type = " ";
            foreach (var sentences in RecordProperties)
            {
               
                if (conver.convertToJava.ContainsKey(sentences.Type.GenerateCode()))
                {
                    type = conver.convertToJava[sentences.Type.GenerateCode()];


                }
                else
                {
                    type = sentences.Type.GenerateCode();
                }
                foreach (var id in sentences.ListId)
                {
                    if (sentences.Type is RecordNode)
                    {

                        blockProperties = blockProperties + " "+type;
                        blockProperties = blockProperties +" "+ sentences.Type.Name + " " + id.Value+"xyz " + "=" + " " + "new" +
                                          " " + sentences.Type.Name + "();\n";

                    }
                    else
                    {
                        blockProperties = blockProperties + type + " " + id.Value + ";\n";
                    }
                }
         
            }
            string simpleConstructur = " public "+this.Name+"(){}\n";
            string simpleConstructur2 = " public " + this.Name + "(" + this.Name + " " + "other" + "){\n";
            foreach (var sentences in RecordProperties)
            {
                if (conver.convertToJava.ContainsKey(sentences.Type.GenerateCode()))
                {
                    type = conver.convertToJava[sentences.Type.GenerateCode()];
               }
                else
                {
                    type = sentences.Type.GenerateCode();
                }
                foreach (var id in sentences.ListId)
                {
                    if (sentences.Type is RecordNode)
                    {

                        blockProperties = blockProperties + " " + type;
                        blockProperties = blockProperties + " " + sentences.Type.Name + " " + id.Value + "xyz " + "=" + " " + "new" +
                                          " " + sentences.Type.Name + "();\n";

                    }
                    else
                    {
                        blockProperties = blockProperties + type + " " + id.Value + ";\n";
                    }
                }

            }


            return " class "+this.Name+"{"+ blockProperties+"}";
        }*/
    }
}
