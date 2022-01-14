using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model.Book
{
    public class Attribute
    {
        public string name { get; set; }
        public string label { get; set; }
        public string field_type { get; set; }
        public string help_text { get; set; }
        public string description { get; set; }
    }

    public class Category
    {
        public string label { get; set; }
        public string icon { get; set; }
        public IList<Attribute> attributes { get; set; }
    }

    public class BookCategoryModel
    {
        public Category overview { get; set; }
        public Category occupants { get; set; }
        public Category design { get; set; }
        public Category purpose { get; set; }
        public Category location { get; set; }
        public Category neighborhood { get; set; }
        public Category financial { get; set; }
        public Category amenities { get; set; }
        public Category history { get; set; }
        public Category gallery { get; set; }
        public Category notes { get; set; }
    }


}
