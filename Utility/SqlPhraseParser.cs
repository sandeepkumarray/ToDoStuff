using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff
{
    public class SqlPhraseParser
    {
        string _CreateTableTemplate = "CREATE TABLE `[TABLENAME]` (\r\n[COLUMNLIST]\r\n);\r\n\r\n";
        public string ParseModel(SQLItemModel sqlItemModel)
        {
            List<string> columns = new List<string>();
            _CreateTableTemplate = _CreateTableTemplate.Replace("[TABLENAME]", sqlItemModel.TableName);

            columns.Add("`id`" + " int");

            foreach (var item in sqlItemModel.Columns)
            {
                columns.Add("`" + item.ColumnName + "`" + " varchar(300)");
            }

            _CreateTableTemplate = _CreateTableTemplate.Replace("[COLUMNLIST]", string.Join(",\r\n", columns));

            return _CreateTableTemplate;
        }
    }
}
