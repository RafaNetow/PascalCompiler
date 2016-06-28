using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Sintactico;
using Mini_Compiler.Tree;

namespace Mini_Compiler.Semantic
{
    class SymbolTable
    {
       public Dictionary<string, BaseType> _table;
        private static SymbolTable _instance;



        private SymbolTable()
        {
            List<ParameterFunction> list = new List<ParameterFunction>();
            
            _table = new Dictionary<string, BaseType>();
           var typeString =  TypesTable.Instance.GetType("string");
            list.Add( new ParameterFunction {IsVar = false, Type = typeString });
           _table.Add("getformdata",new FunctionType (list, typeString));
            _table.Add("writeln", new ProceureType(list ));

        }


        public static SymbolTable Instance => _instance ?? (_instance = new SymbolTable());


        public void DeclareVariable(string name, string typeName)
        {
            if (_table.ContainsKey(name))
            {
                throw new SemanticException($"Variable  :{name} exists.");
            }

            if (TypesTable.Instance.Contains(name))
                throw new SemanticException($"  :{name} is a type.");

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


        public void DeclareVariable(string value, string typeName, List<Range> dimensions)
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

        public void DeclareVariable(string value, BaseType typeName, List<Range> dimensions)
        {
            
                var type = TypesTable.Instance.GetType(value);
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
                    throw new SemanticException($"  :{value} is a type.");

                _table.Add(value, type);
            }

        public void DeclareVariable(string name, BaseType type)
        {
            if (_table.ContainsKey(name))
            {
                throw new SemanticException($"Variable  :{name} exists.");
            }

            if (TypesTable.Instance.Contains(name))
                throw new SemanticException($"  :{name} iz a taippp.");

            _table.Add(name, type);
        }
    }

    internal class CallFuntionType : BaseType
    {
        public override bool IsAssignable(BaseType otherType)
        {
            throw new NotImplementedException();
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }

    public class ArrayType : BaseType
    {
        public Range Dimension { get; set; }
        public BaseType Type { get; set; }

        public ArrayType(Range dimension, BaseType type)
        {
            Dimension = dimension;
            Type = type;
        }

        public override bool IsAssignable(BaseType otherType)
        {
            if (otherType is ArrayType)
            {
                var paramArray = (ArrayType)otherType;
                if (paramArray.Dimension.Infe == Dimension.Infe && paramArray.Dimension.Super == Dimension.Super && Type.IsAssignable(paramArray))
                {
                    return true;
                }
            }

            return false;
        }

        public override string GenerateCode()
        {
            BaseType type = getType((ArrayType)this.Type);
            return type.GenerateCode();

        }
        public BaseType getType(ArrayType type)
        {
            BaseType dimensions;

            while (true)
            {


                if (isPrimitive(type.Type) || type.Type is RecordType)
                {
                    return dimensions = type.Type;

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
    }

}
