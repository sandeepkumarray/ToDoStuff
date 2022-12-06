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
        public Content(string contentType, bool isPublic) : this()
        {
            content_type = contentType;
            is_public = isPublic;
        }
        public Content()
        {
            categories = new List<Category>();
            references = new List<Category>();
        }

        [JsonProperty("content_type")]
        public string content_type { get; set; }

        [JsonProperty("is_public")]
        public bool is_public { get; set; }

        [JsonProperty("categories")]
        public List<Category> categories { get; set; }

        [JsonProperty("references")]
        public List<Category> references { get; set; }
    }

    public partial class Category
    {
        public Category()
        {
            Attributes = new List<BaseAttribute>();
        }

        public Category(int order, string label, string icon, string name, bool _is_active = true, bool _is_hidden = false) : this()
        {
            Order = order;
            Label = label;
            Icon = icon;
            Name = name; 
            is_active = _is_active;
            is_hidden = _is_hidden;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("is_active")]
        public bool is_active { get; set; }

        [JsonProperty("is_hidden")]
        public bool is_hidden { get; set; }

        [JsonProperty("attributes")]
        public List<BaseAttribute> Attributes { get; set; }
    }

    public partial class Attribute : BaseAttribute
    {
        public Attribute()
        {

        }
        public Attribute(string _field_name, string _field_label, string _field_type, string _help_text, string _reference_table = null, bool _is_active = true)
        {
            field_name = _field_name;
            field_label = _field_label;
            field_type = _field_type;
            help_text = _help_text;
            reference_table = _reference_table;
            is_active = _is_active;
            control_type = DeriveControlType();
        }

        public Attribute(string _field_name, string _field_label, string _field_type, string _help_text, bool _auto_increament, bool _allow_null = true, bool _is_active = true, bool _is_hidden = false, bool _is_user_defined = false, string _reference_table = "")
        {
            field_name = _field_name;
            field_label = _field_label;
            field_type = _field_type;
            help_text = _help_text;
            is_active = _is_active;
            is_hidden = _is_hidden;
            is_user_defined = _is_user_defined;
            allow_null = _allow_null;
            auto_increament = _auto_increament;
            reference_table = _reference_table;
            control_type = DeriveControlType();
        }

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

    public class BaseAttribute
    {
        [JsonIgnore]
        public string[] intList = new string[] { "boolean", "number", "int", "integer", "smallint unsigned", "mediumint", "bigint", "int unsigned", "integer unsigned", "bit" };
        [JsonIgnore]
        public string[] VarcharList = new string[] { "binary", "varbinary", "tinyblob", "blob", "mediumblob", "longblob", "char byte", "char", "varchar", "tinytext", "text", "mediumtext", "longtext", "set", "enum", "nchar", "national char", "nvarchar", "national varchar", "character varying" };

        [JsonProperty("field_name")]
        public string field_name { get; set; }

        [JsonProperty("field_label")]
        public string field_label { get; set; }

        [JsonProperty("field_type")]
        public string field_type { get; set; }

        [JsonProperty("help_text")]
        public string help_text { get; set; }

        [JsonProperty("control_type")]
        public string control_type { get; set; }

        [JsonProperty("reference_table")]
        public string reference_table { get; set; }

        public BaseAttribute()
        {

        }

        public BaseAttribute(string _field_name, string _field_label, string _field_type, string _help_text, string _reference_table = null)
        {
            field_name = _field_name;
            field_label = _field_label;
            field_type = _field_type;
            help_text = _help_text;
            reference_table = _reference_table;
            control_type = DeriveControlType();
        }

        public string DeriveControlType()
        {
            string type = "text";

            if (this.field_name.ToLower() == "universe")
            {
                type = "select";
            }
            else
            {
                if (this.field_type.In(intList))
                {
                    type = "number";
                }
                if (this.field_type.In(VarcharList))
                {
                    type = "textarea";
                }
                else if (this.field_type == "tinyint")
                {
                    type = "checkbox";
                }
            }

            return type;
        }

    }
}
