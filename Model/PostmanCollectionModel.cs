using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    public class PostmanCollectionSetting
    {
        public string CollectionName { get; set; }
        public string Description { get; set; }
        public List<string> path { get; set; }
        public List<string> host { get; set; }
    }

    public class PostmanCollectionModel
    {
        [JsonProperty("info")]
        public Info info { get; set; }

        [JsonProperty("item")]
        public List<Item> item { get; set; }

        [JsonProperty("setting")]
        PostmanCollectionSetting Setting { get; set; }
        public PostmanCollectionModel()
        {
            item = new List<Item>();
        }
        public PostmanCollectionModel(PostmanCollectionSetting postmanCollectionSetting)
        {
            item = new List<Item>();
            Setting = postmanCollectionSetting;
        }
        public static PostmanCollectionModel FromJson(string json) { return JsonConvert.DeserializeObject<PostmanCollectionModel>(json, Converter.Settings); }
    }
    public class Info
    {

        [JsonProperty("_postman_id")]
        public string _postman_id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("schema")]
        public string schema { get; set; }
    }

    public class Raw
    {

        [JsonProperty("language")]
        public string language { get; set; }
    }

    public class Options
    {

        [JsonProperty("raw")]
        public Raw raw { get; set; }
    }

    public class Body
    {

        [JsonProperty("mode")]
        public string mode { get; set; }

        [JsonProperty("raw")]
        public string raw { get; set; }

        [JsonProperty("options")]
        public Options options { get; set; }
    }

    public class Header
    {
        [JsonProperty("key")]
        public string key { get; set; }
        [JsonProperty("value")]
        public string value { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }
    }
    
    public class Query
    {
        [JsonProperty("key")]
        public string key { get; set; }
        [JsonProperty("value")]
        public string value { get; set; }
    }

    public class Url
    {
        [JsonProperty("raw")]
        public string raw { get; set; }

        [JsonProperty("protocol")]
        public string protocol { get; set; }

        [JsonProperty("host")]
        public List<string> host { get; set; }

        [JsonProperty("port")]
        public string port { get; set; }

        [JsonProperty("path")]
        public List<string> path { get; set; }

        [JsonProperty("query")]
        public List<Query> query { get; set; }
    }

    public class Request
    {

        [JsonProperty("method")]
        public string method { get; set; }

        [JsonProperty("header")]
        public List<Header> header { get; set; }

        [JsonProperty("body")]
        public Body body { get; set; }

        [JsonProperty("url")]
        public Url url { get; set; }
    }

    public class Item
    {

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("request")]
        public Request request { get; set; }

        [JsonProperty("response")]
        public List<object> response { get; set; }

        [JsonProperty("item")]
        public List<Item> item { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        public Item()
        {
            item = new List<Item>();
        }
    }

}
