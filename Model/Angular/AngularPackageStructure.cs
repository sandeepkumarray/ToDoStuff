using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model.Angular
{
    public class AngularPackageStructure
    {
        public AngularPackageStructure()
        {
            Modules = new List<AngularModule>();
        }

        [JsonProperty("package_name")]
        public string PackageName { get; set; }

        [JsonProperty("modules")]
        public List<AngularModule> Modules { get; set; }

        public static AngularPackageStructure FromJson(string json) { return JsonConvert.DeserializeObject<AngularPackageStructure>(json, Converter.Settings); }
    }
    
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AngularModuleComponent
    {
        public AngularModuleComponent(string componentName, AngularComponentType angularComponent = AngularComponentType.ModuleComponent, AngularClassType componentType = AngularClassType.Component)
        {
            ComponentName = componentName;
            ComponentType = componentType;
            AngularComponentType = angularComponent;
            Files = new List<AngularFile>();
            OptionalFiles = new List<OptionalAngularFile>();
        }

        [JsonProperty("component_folder_path")]
        public string ComponentFolderPath { get; set; }

        [JsonProperty("component_folder_name")]
        public string ComponentFolderName { get; set; }

        [JsonProperty("component_name")]
        public string ComponentName { get; set; }

        [JsonProperty("component_type")]
        public AngularClassType ComponentType { get; set; }

        [JsonProperty("component_component_type")]
        public AngularComponentType AngularComponentType { get; set; }
        [JsonProperty("files")]
        public List<AngularFile> Files { get; set; }

        [JsonProperty("optional_files")]
        public List<OptionalAngularFile> OptionalFiles { get; set; }
    }

    public class AngularFile
    {
        public AngularFile(string fileName, AngularClassType fileType)
        {
            FileName = fileName;
            FileType = fileType;
        }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("file_type")]
        public AngularClassType FileType { get; set; }

        [JsonProperty("file_contents")]
        public string FileContents { get; set; }
    }

    public class AngularModule
    {
        public AngularModule(string moduleName, AngularClassType moduleType)
        {
            ModuleName = moduleName;
            ModuleType = moduleType;
            Components = new List<AngularModuleComponent>();
        }

        [JsonProperty("module_folder_path")]
        public string ModuleFolderPath { get; set; }

        [JsonProperty("module_folder_name")]
        public string ModuleFolderName { get; set; }

        [JsonProperty("module_name")]
        public string ModuleName { get; set; }

        [JsonProperty("file_contents")]
        public string FileContents { get; set; }

        [JsonProperty("module_type")]
        public AngularClassType ModuleType { get; set; }

        [JsonProperty("components")]
        public List<AngularModuleComponent> Components { get; set; }
    }

    public class OptionalAngularFile
    {
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("file_type")]
        public string FileType { get; set; }

        [JsonProperty("file_contents")]
        public string FileContents { get; set; }
    }
}
