using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ToDoStuff.Helpers
{
    class YamlToJsonConverter
    {
        public  static string YamlToJson(string yamlString)
        {
            var r = new StringReader(yamlString);
            var deserializer = new Deserializer();
            var yamlObject = deserializer.Deserialize(r);

            var w = new StringWriter();
            var serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.Serialize(w, yamlObject);

            return w.ToString();
        }
    }
}
