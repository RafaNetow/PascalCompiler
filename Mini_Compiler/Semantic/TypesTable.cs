using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Tree;

namespace Mini_Compiler.Semantic
{
    class TypesTable
    {
        private Dictionary<string, BaseType> _table;
        private static TypesTable _instance;

        private TypesTable()
        {
            _table = new Dictionary<string, BaseType>();
            _table.Add("integer", new IntType());
            _table.Add("string", new StringType());
            _table.Add("real ", new RealType());
            _table.Add("char", new CharType());
            _table.Add("boolean",new BooleanType());
            _table.Add("Const", new ConstType());
            //_table.Add("function", new FunctionType());
            
        }


        public static TypesTable Instance => _instance ?? (_instance = new TypesTable());


        public void RegisterType(string name, BaseType baseType)
        {
            if (_table.ContainsKey(name))
            {
                throw new SemanticException($"Type :{name} exists.");
            }

            _table.Add(name, baseType);
        }

        public BaseType GetType(string name)
        {
            if (_table.ContainsKey(name))
            {
                return _table[name];
            }

            throw new SemanticException($"Type :{name} doesn't exists.");
        }


        public bool Contains(string name)
        {
            return _table.ContainsKey(name);
        }
    }

    


    internal class SemanticException : Exception
    {
        public SemanticException(string message) : base(message)
        {


        }
    }
}
