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


    public class SQLTable
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }

    class ContentType
    {
        public ContentType()
        {

        }
        public ContentType(string name, List<string> categories)
        {
            Name = name;
            Categories = categories;
        }

        public string Name;
        public List<string> Categories;
    }
}
