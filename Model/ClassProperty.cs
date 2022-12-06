using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    public class ClassProperty
    {
        public List<string> GenericListTypeList { get; set; }

        public List<string> PropTypeList { get; set; }

        public string GenericListType { get; set; }

        public string PropName { get; set; }

        public string PropType { get; set; }

        public string DBDataType { get; set; }
        public string DB_ColumnType { get; set; }
        public string DB_ColumnKey { get; set; }

        public bool IsXmlElement { get; set; }

        public bool IsXmlAttribute { get; set; }

        public bool IsXmlText { get; set; }
        public bool IsJsonProperty { get; set; }

        public bool IsGenericListType { get; set; }
        public bool AddGetterSetter { get; set; }
        public string AccessType { get; set; } = "public";
        public string GetterSetterBody { get; set; } = " { get; set; }";

        public Dictionary<List<string>, string> DBTypetoCSharpType;
        public Dictionary<List<string>, string> DBTypetoAngularType;

        public ClassProperty()
        {
            Init();
        }

        public virtual void Init()
        {
            PropTypeList = new List<string>();
            PropTypeList.Add("bool");
            PropTypeList.Add("byte");
            PropTypeList.Add("sbyte");
            PropTypeList.Add("char");
            PropTypeList.Add("decimal");
            PropTypeList.Add("double");
            PropTypeList.Add("float");
            PropTypeList.Add("int");
            PropTypeList.Add("uint");
            PropTypeList.Add("long");
            PropTypeList.Add("ulong");
            PropTypeList.Add("object");
            PropTypeList.Add("short");
            PropTypeList.Add("ushort");
            PropTypeList.Add("string");

            GenericListTypeList = new List<string>();
            GenericListTypeList.Add("Comparer");
            GenericListTypeList.Add("EqualityComparer");
            GenericListTypeList.Add("HashSet");
            GenericListTypeList.Add("KeyedByTypeCollection");
            GenericListTypeList.Add("LinkedList");
            GenericListTypeList.Add("LinkedListNode");
            GenericListTypeList.Add("List");
            GenericListTypeList.Add("Queue");
            GenericListTypeList.Add("SortedSet");
            GenericListTypeList.Add("Stack");
            GenericListTypeList.Add("SynchronizedCollection");
            GenericListTypeList.Add("SynchronizedReadOnlyCollection");

            DBTypetoCSharpType = new Dictionary<List<string>, string>();
            DBTypetoCSharpType.Add(new List<string>() { "bool", "boolean", "bit" }, "Boolean");
            DBTypetoCSharpType.Add(new List<string>() { "bool", "boolean", "bit" }, "Boolean");
            DBTypetoCSharpType.Add(new List<string>() { "tinyint" }, "Boolean");
            DBTypetoCSharpType.Add(new List<string>() { "tinyint unsigned" }, "Byte");
            DBTypetoCSharpType.Add(new List<string>() { "smallint", "year" }, "Int16");
            DBTypetoCSharpType.Add(new List<string>() { "int", "integer", "smallint unsigned", "mediumint" }, "Int64");
            DBTypetoCSharpType.Add(new List<string>() { "bigint", "int unsigned", "integer unsigned", "bit" }, "Int64");
            DBTypetoCSharpType.Add(new List<string>() { "float" }, "Single");
            DBTypetoCSharpType.Add(new List<string>() { "double", "real" }, "Double");
            DBTypetoCSharpType.Add(new List<string>() { "decimal", "numeric", "dec", "fixed", "bigint unsigned", "float unsigned", "double unsigned", "serial" }, "Decimal");
            DBTypetoCSharpType.Add(new List<string>() { "date", "timestamp", "datetime" }, "DateTime");
            DBTypetoCSharpType.Add(new List<string>() { "datetimeoffset" }, "DateTimeOffset");
            DBTypetoCSharpType.Add(new List<string>() { "time" }, "TimeSpan");
            DBTypetoCSharpType.Add(new List<string>() { "char", "varchar", "tinytext", "text", "mediumtext", "longtext", "set", "enum", "nchar", "national char", "nvarchar", "national varchar", "character varying" }, "String");
            DBTypetoCSharpType.Add(new List<string>() { "binary", "varbinary", "tinyblob", "blob", "mediumblob", "longblob", "char byte" }, "String");
            DBTypetoCSharpType.Add(new List<string>() { "geometry2" }, "Data.Spatial.DbGeometry");
            DBTypetoCSharpType.Add(new List<string>() { "geometry2" }, "Data.Spatial.DbGeography");

            DBTypetoAngularType = new Dictionary<List<string>, string>();
            DBTypetoAngularType.Add(new List<string>() { "bool", "boolean", "bit" }, "boolean");
            DBTypetoAngularType.Add(new List<string>() { "bool", "boolean", "bit" }, "boolean");
            DBTypetoAngularType.Add(new List<string>() { "tinyint" }, "boolean");
            DBTypetoAngularType.Add(new List<string>() { "tinyint unsigned" }, "byte");
            DBTypetoAngularType.Add(new List<string>() { "smallint", "year" }, "number");
            DBTypetoAngularType.Add(new List<string>() { "int", "integer", "smallint unsigned", "mediumint" }, "number");
            DBTypetoAngularType.Add(new List<string>() { "bigint", "int unsigned", "integer unsigned", "bit" }, "number");
            DBTypetoAngularType.Add(new List<string>() { "float" }, "Single");
            DBTypetoAngularType.Add(new List<string>() { "double", "real" }, "number");
            DBTypetoAngularType.Add(new List<string>() { "decimal", "numeric", "dec", "fixed", "bigint unsigned", "float unsigned", "double unsigned", "serial" }, "number");
            DBTypetoAngularType.Add(new List<string>() { "date", "timestamp", "datetime" }, "Date");
            DBTypetoAngularType.Add(new List<string>() { "datetimeoffset" }, "Date");
            DBTypetoAngularType.Add(new List<string>() { "time" }, "Date");
            DBTypetoAngularType.Add(new List<string>() { "char", "varchar", "tinytext", "text", "mediumtext", "longtext", "set", "enum", "nchar", "national char", "nvarchar", "national varchar", "character varying" }, "string");
            DBTypetoAngularType.Add(new List<string>() { "binary", "varbinary", "tinyblob", "blob", "mediumblob", "longblob", "char byte" }, "string");
            DBTypetoAngularType.Add(new List<string>() { "geometry2" }, "Data.Spatial.DbGeometry");
            DBTypetoAngularType.Add(new List<string>() { "geometry2" }, "Data.Spatial.DbGeography");
        }

        internal string GetCsharpTypeFromDBType(string dataType)
        {
            string returnValue = "string";
            foreach (var item in DBTypetoCSharpType)
            {
                foreach (var dbt in item.Key)
                {
                    if (dbt == dataType)
                        returnValue = item.Value;
                }
            }
            return returnValue;
        }

        internal string GetAngularTypeFromDBType(string dataType)
        {
            string returnValue = "any";
            foreach (var item in DBTypetoAngularType)
            {
                foreach (var dbt in item.Key)
                {
                    if (dbt == dataType)
                        returnValue = item.Value;
                }
            }
            return returnValue;
        }
        public ClassProperty(string propName, string propType)
            : this()
        {
            PropName = propName;
            PropType = propType;
        }
        public ClassProperty(string propName, string propType, string accessType)
           : this(propName, propType)
        {
            AccessType = accessType;
        }
    }
}
