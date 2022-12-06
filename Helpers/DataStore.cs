using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Model;

namespace ToDoStuff.Helpers
{
    public class DataStore
    {
        public string Database { get; set; }
        public List<DataStoreTable> Tables { get; set; }
        public List<string> blockColumnList = new List<string>() { "id", "created_at", "updated_at", "user_id", "archived_at", "deleted_at" };

        public DataStore()
        {
            Tables = new List<DataStoreTable>();
        }

        public DataStoreTable this[string tableName]
        {
            get
            {
                // get the item for that index.
                return Tables.Find(t=> t.TableName == tableName);
            }
            set
            {
                // set the item for this index. value will be of type Thing.
                Tables.Add(value);
            }
        }
    }

    public class DataStoreTable
    {
        public string TableName { get; set; }
        public List<ClassProperty> ColumnList { get; set; }
        public string TableNameWithoutTrailS { get; set; }
        public bool HasTrailingS { get; set; }


        public DataStoreTable(string tableName, List<ClassProperty> columnList, int trailing_name_count = 1)
        {
            TableName = tableName;
            ColumnList = columnList;
            HasTrailingS = TableName.Trim().Substring(TableName.Trim().Length - 1, 1).ToLower() == "s" ? true : false;
            TableNameWithoutTrailS = TableName.Trim().Substring(0, TableName.Trim().Length - trailing_name_count).ToCamelCase();
        }
    }

    public class DataStoreAngularComponent: DataStoreTable
    {
        public Component Component { get; set; }
        public DataStoreAngularComponent(string tableName, List<ClassProperty> columnList, int trailing_name_count = 1) : base(tableName, columnList, trailing_name_count)
        {
        }

    }
}
