using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic.Types;

namespace Mini_Compiler.Semantic
{
    class SymbolTable
    {
        private Dictionary<string, BaseType> _table;
        private static SymbolTable _instance;


        private SymbolTable()
        {
            _table = new Dictionary<string, BaseType>();

        }


        public static SymbolTable Instance => _instance ?? (_instance = new SymbolTable());


        public void DeclareVariable(string name, string typeName)
        {
            if (_table.ContainsKey(name))
            {
                throw new SemanticException($"Variable  :{name} exists.");
            }

            if (TypesTable.Instance.Contains(name))
                throw new SemanticException($"  :{name} iz a taippp.");

            _table.Add(name, TypesTable.Instance.GetType(typeName));
        }

        public BaseType GetVariable(string name)
        {
            if (_table.ContainsKey(name))
            {
                return _table[name];
            }

            throw new SemanticException($"Variable :{name} doesn't exists.");
        }


        public void DeclareVariable(string value, string typeName, List<int> dimensions)
        {
            if (dimensions.Count == 0)
            {
                DeclareVariable(value, typeName);
            }
            else
            {
                var type = TypesTable.Instance.GetType(typeName);
                dimensions.Reverse(0, dimensions.Count);
                foreach (var dimension in dimensions)
                {

                    type = new ArrayType(dimension, type);

                }
                if (_table.ContainsKey(value))
                {
                    throw new SemanticException($"Variable  :{value} exists.");
                }

                if (TypesTable.Instance.Contains(value))
                    throw new SemanticException($"  :{value} iz a taippp.");

                _table.Add(value, type);
            }
        }
    }

    public class ArrayType : BaseType
    {
        public int Dimension { get; set; }
        public BaseType Type { get; set; }

        public ArrayType(int dimension, BaseType type)
        {
            Dimension = dimension;
            Type = type;
        }

        public override bool IsAssignable(BaseType otherType)
        {
            if (otherType is ArrayType)
            {
                var paramArray = (ArrayType)otherType;
                if (Dimension == paramArray.Dimension && Type.IsAssignable(paramArray.Type))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
