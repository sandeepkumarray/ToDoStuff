using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ToDoStuff
{
    public class SQLItemModel
    {
        public string TableName { get; set; }
        public List<DataColumn> Columns { get; set; }
        public string Size { get; set; }

        public SQLItemModel(string SheetName)
        {
            TableName = SheetName; 
            Columns = new List<DataColumn>();
        }
    }
}
