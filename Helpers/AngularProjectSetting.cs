using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Model;

namespace ToDoStuff.Helpers
{
    public class DataStoreSettings
    {
        [JsonProperty("table_name")]
        public string table_name { get; set; }

        [JsonProperty("is_use_trailing_name")]
        public bool is_use_trailing_name { get; set; }

        [JsonProperty("trailing_name_count")]
        public int trailing_name_count { get; set; }
    }
    public class Component
    {
        [JsonProperty("component_name")]
        public string component_name { get; set; }

        [JsonProperty("component_path")]
        public string component_path { get; set; }

        [JsonProperty("component_type")]
        public string component_type { get; set; }

        [JsonProperty("sub_components")]
        public List<Component> sub_components { get; set; }

        [JsonProperty("services")]
        public List<string> services { get; set; }

        [JsonProperty("html_sections")]
        public List<HtmlSection> html_sections { get; set; }
    }
    public class ModuleSettings
    {
        [JsonProperty("module_path")]
        public string module_path { get; set; }

        [JsonProperty("sub_components")]
        public List<string> sub_components { get; set; }

    }

    public class HtmlSection
    {
        [JsonProperty("section_type")]
        public string section_type { get; set; }

        [JsonProperty("data_binding_model")]
        public string data_binding_model { get; set; }

        [JsonProperty("data_binding_properties")]
        public List<string> data_binding_properties { get; set; }
    }

    public class AngularProjectSetting : ViewModelBase
    {
        [JsonProperty("project_name")]
        public string project_name { get; set; }

        [JsonProperty("component_base_path")]
        public string component_base_path { get; set; }

        [JsonProperty("data_store_settings")]
        public DataStoreSettings data_store_settings { get; set; }

        [JsonProperty("module_settings")]
        public ModuleSettings module_settings { get; set; }

        [JsonProperty("components")]
        public List<Component> components { get; set; }

        public static AngularProjectSetting FromJson(string json) { return JsonConvert.DeserializeObject<AngularProjectSetting>(json, Converter.Settings); }
    }


}
