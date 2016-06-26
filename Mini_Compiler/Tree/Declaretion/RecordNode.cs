using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;

namespace Mini_Compiler.Tree.Declaretion
{
    class RecordNode : TypeDeclaretionNode
    {
        public List<RecordProperties> RecordProperties;

        public override void ValidateSemantic()
        {
            foreach (var recordPropertiese in RecordProperties)
            {
                recordPropertiese.Type.Name = recordPropertiese.ListId[0].Value;
                recordPropertiese.Type.ValidateSemantic();


            }
            TypesTable.Instance.RegisterType(Name, new RecordType { RecordProperties = RecordProperties });
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }
}
