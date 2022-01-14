using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    public partial class ContentTemplate
    {
        public ContentTemplate()
        {
            Contents = new List<Content>();
        }
        [JsonProperty("template_name")]
        public string TemplateName { get; set; }

        [JsonProperty("contents")]
        public List<Content> Contents { get; set; }
    }

    public partial class Content
    {
        public Content(string contentType, bool isPublic) :this()
        {
            content_type = contentType;
            is_public = isPublic;
        }
        public Content()
        {
            categories = new List<Category>();
        }

        [JsonProperty("content_type")]
        public string content_type { get; set; }

        [JsonProperty("is_public")]
        public bool is_public { get; set; }

        [JsonProperty("categories")]
        public List<Category> categories { get; set; }
    }

    public partial class Category
    {
        public Category()
        {
            Attributes = new List<Attribute>();
        }
        public Category(int order, string label, string icon, string name):this()
        {
            Order = order;
            Label = label;
            Icon = icon;
            Name = name;
        }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }
    }

    public partial class Attribute
    {
        public Attribute()
        {

        }
        public Attribute(string _field_name, string _field_label, string _field_type, bool _auto_increament,  bool _allow_null = true, bool _is_active = true, bool _is_hidden = false, bool _is_user_defined = false)
        {
            field_name = _field_name;
            field_label = _field_label;
            field_type = _field_type;
            is_active = _is_active;
            is_hidden = _is_hidden;
            is_user_defined = _is_user_defined;
            allow_null = _allow_null;
            auto_increament = _auto_increament;
        }

        [JsonProperty("field_name")]
        public string field_name { get; set; }

        [JsonProperty("field_label")]
        public string field_label { get; set; }

        [JsonProperty("field_type")]
        public string field_type { get; set; }

        [JsonProperty("is_active")]
        public bool is_active { get; set; }

        [JsonProperty("is_hidden")]
        public bool is_hidden { get; set; }

        [JsonProperty("is_user_defined")]
        public bool is_user_defined { get; set; }

        [JsonProperty("allow_null")]
        public bool allow_null { get; set; }

        [JsonProperty("auto_increament")]
        public bool auto_increament { get; set; }
    }

}
