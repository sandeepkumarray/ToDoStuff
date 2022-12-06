using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Helpers;
using ToDoStuff.Model;
using ToDoStuff.Model.Angular;

namespace ToDoStuff.Engine
{
    public class AngularProjectEngine
    {
        public DataStore DataStore { get; set; }
        public string RootFilePath { get; set; }
        public List<AngularModule> Modules { get; set; }

        public AngularProjectSetting angularProjectSetting = null;

        public AngularProjectEngine()
        {
            Modules = new List<AngularModule>();
            this.angularProjectSetting = GetDefaultSettings();
            DataStore = new DataStore()
            {
                Database = this.angularProjectSetting.project_name,
            };
        }

        public AngularProjectEngine(AngularProjectSetting _angularProjectSetting) : this()
        {
            this.angularProjectSetting = _angularProjectSetting;
        }

        public AngularProjectEngine(string angularProjectSettingJson) : this()
        {
            this.angularProjectSetting = AngularProjectSetting.FromJson(angularProjectSettingJson);
        }

        public AngularProjectEngine SetDataStore(DataStore dataStore)
        {
            this.DataStore = dataStore;
            RootFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                     DataStore.Database.Trim(), this.angularProjectSetting.component_base_path);
            if (this.angularProjectSetting.components.Any())
            {
                foreach (var component in this.angularProjectSetting.components)
                {
                    this.DataStore[component.component_name] = new DataStoreAngularComponent(component.component_name, null)
                    {
                        Component = component
                    };
                }
            }

            return this;
        }

        public AngularProjectEngine Initialize()
        {
            foreach (DataStoreAngularComponent table in DataStore.Tables)
            {
                string tableName = this.angularProjectSetting.data_store_settings.is_use_trailing_name ? table.TableNameWithoutTrailS : table.TableName;
                AngularModule module = new AngularModule(table.TableName.ToLower() + ".module", AngularClassType.Module);
                module.ModuleFolderName = table.TableName.ToLower();
                module.ModuleFolderPath = this.angularProjectSetting.module_settings.module_path;
                module.FileContents = GetAngularFileContentsFromType(table.TableName, AngularClassType.Module);
                module.Components = GetModuleComponents(table, module);
                this.Modules.Add(module);

                AngularModule routingModule = new AngularModule(table.TableName.ToLower() + "-routing.module", AngularClassType.RoutingModule);
                routingModule.ModuleFolderName = table.TableName.ToLower();
                routingModule.ModuleFolderPath = this.angularProjectSetting.module_settings.module_path;
                routingModule.FileContents = GetAngularFileContentsFromType(table.TableName, AngularClassType.RoutingModule);
                this.Modules.Add(routingModule);
            }

            return this;
        }

        private List<AngularModuleComponent> GetModuleComponents(DataStoreAngularComponent table, AngularModule module)
        {
            List<AngularModuleComponent> angularModuleComponents = new List<AngularModuleComponent>();
            //main component
            {
                AngularModuleComponent mainComponent = new AngularModuleComponent(table.TableName.ToLower(), AngularComponentType.ModuleComponent);
                mainComponent.ComponentFolderName = table.TableName.ToLower();
                mainComponent.ComponentFolderPath = Path.Combine(module.ModuleFolderPath, module.ModuleFolderName);

                AngularFile styleFile = new AngularFile(table.TableName.ToLower() + ".component", AngularClassType.CSS);

                AngularFile htmlFile = new AngularFile(table.TableName.ToLower() + ".component", AngularClassType.Html);
                //htmlFile.FileContents = new AngularFileService(table).GenerateFromTemplateFile("{name}.component.html");

                AngularFile tsFile = new AngularFile(table.TableName.ToLower() + ".component", AngularClassType.ts);
                tsFile.FileContents = GetAngularFileContentsFromType(table.TableName, AngularClassType.Component);

                mainComponent.Files = new List<AngularFile>() { styleFile, htmlFile, tsFile };
                angularModuleComponents.Add(mainComponent);
            }

            if (this.angularProjectSetting.module_settings.sub_components.Any())
            {
                foreach (var component_name in this.angularProjectSetting.module_settings.sub_components)
                {
                    AngularModuleComponent component = new AngularModuleComponent(component_name.ToLower() + "-" + table.TableName.ToLower(), AngularComponentType.Component);
                    component.ComponentFolderName = component_name.ToLower() + "-" + table.TableName.ToLower();
                    component.ComponentFolderPath = Path.Combine(module.ModuleFolderPath, module.ModuleFolderName, component.ComponentFolderName);

                    AngularFile styleFile = new AngularFile(component_name.ToLower() + "-" + table.TableName.ToLower() + ".component", AngularClassType.SCSS);

                    AngularFile htmlFile = new AngularFile(component_name.ToLower() + "-" + table.TableName.ToLower() + ".component", AngularClassType.Html);

                    htmlFile.FileContents = "<p>" + component_name.ToLower() + "-" + table.TableName.ToLower() + " works</p>";

                    AngularFile tsFile = new AngularFile(component_name.ToLower() + "-" + table.TableName.ToLower() + ".component", AngularClassType.ts);
                    tsFile.FileContents = GetAngularFileContentsFromType(component_name.ToLower() + "-" + table.TableName.ToLower(), AngularClassType.Component);
                    component.Files = new List<AngularFile>() { styleFile, htmlFile, tsFile };
                    angularModuleComponents.Add(component);
                }
            }

            return angularModuleComponents;
        }

        public AngularProjectEngine Process()
        {
            AngularPackageFileList modulesFilesList = new AngularPackageFileList();

            foreach (AngularModule module in Modules)
            {
                string moduleFile = (Path.Combine(RootFilePath, module.ModuleFolderPath, module.ModuleFolderName, module.ModuleName + GetFileExtension(module.ModuleType)));
                modulesFilesList.Add(new AngularPackageFile()
                {
                    FilePath = moduleFile,
                    Contents = module.FileContents
                });

                foreach (var component in module.Components)
                {
                    foreach (var cfile in component.Files)
                    {
                        string componentFile = Path.Combine(RootFilePath, component.ComponentFolderPath, cfile.FileName + GetFileExtension(cfile.FileType));
                        modulesFilesList.Add(new AngularPackageFile()
                        {
                            FilePath = componentFile,
                            Contents = cfile.FileContents
                        });
                    }
                }
            }

            modulesFilesList.Create();
            return this;
        }

        public string GetAngularFileContentsFromType(string componentName, AngularClassType angularClassType)
        {
            string content = string.Empty;

            switch (angularClassType)
            {
                case AngularClassType.Component:
                    AngularClassFileSetting setting = new AngularClassFileSetting();
                    #region component.ts
                    setting = new AngularClassFileSetting();
                    setting.ClassName = componentName.ToCamelCase().ToCamelCase('-') + "Component";
                    setting.AngularClassType = AngularClassType.Component;
                    setting.IsClassNameCamelCasing = false;
                    setting.IsIncludeDefaultConstructor = true;
                    setting.IsIncludeNameSpace = false;
                    setting.IsIncludeUsings = false;
                    setting.InheritedClass = "OnInit";

                    setting.Component = new AngularComponent();
                    setting.Component.selector = "app-" + componentName.ToLower() + "";
                    setting.Component.templateUrl = "./" + componentName.ToLower() + ".component.html";
                    setting.Component.styleUrls = "./" + componentName.ToLower() + ".component.css";
                    setting.ClassMethods = GenerateComponentClassMethods();

                    content = GenerateAngularModuleClass(setting);
                    break;
                #endregion
                case AngularClassType.Service:
                    break;
                case AngularClassType.RoutingModule:
                    #region routing-module
                    setting = new AngularClassFileSetting();
                    setting.ClassName = componentName.ToCamelCase().ToCamelCase('-') + "RoutingModule";
                    setting.AngularClassType = AngularClassType.RoutingModule;
                    setting.IsClassNameCamelCasing = false;
                    setting.IsIncludeDefaultConstructor = false;
                    setting.IsIncludeNameSpace = false;
                    setting.IsIncludeUsings = false;

                    setting.AngularRoute = new AngularRoute();
                    var mainRoot = new Route()
                    {
                        Path = "",
                        Data = new Data() { Title = componentName },
                        Children = GetRouteChildren(DataStore[componentName])
                    };
                    setting.AngularRoute.Routes.Add(mainRoot);

                    setting.UserDefinedUsings = new ObservableCollectionFast<string>();
                    setting.UserDefinedUsings.Add("import { " + componentName.ToCamelCase().ToCamelCase('-') + "Component } from './" + componentName.ToCamelCase().ToLower() + ".component';");

                    if (this.angularProjectSetting.module_settings.sub_components.Any())
                    {
                        foreach (var component_name in this.angularProjectSetting.module_settings.sub_components)
                        {
                            setting.UserDefinedUsings.Add("import { " + component_name + componentName.ToCamelCase().ToCamelCase('-') + "Component } from './" + component_name.ToLower() + "-" + componentName.ToCamelCase().ToLower() + "/" + component_name.ToLower() + "-" + componentName.ToCamelCase().ToLower() + ".component';");
                        }
                    }
                    content = GenerateAngularModuleClass(setting);
                    break;
                #endregion
                case AngularClassType.Module:
                    #region module
                    setting = new AngularClassFileSetting();
                    setting.ClassName = componentName.ToCamelCase().ToCamelCase('-') + "Module";
                    setting.AngularClassType = AngularClassType.Module;
                    setting.IsClassNameCamelCasing = false;
                    setting.IsIncludeDefaultConstructor = false;
                    setting.IsIncludeNameSpace = false;
                    setting.IsIncludeUsings = false;
                    setting.Declarations = new List<string>();
                    setting.Declarations.Add(componentName.ToCamelCase().ToCamelCase('-') + "Component");

                    setting.Imports = new List<string>();
                    setting.Imports.Add(componentName.ToCamelCase().ToCamelCase('-') + "RoutingModule");

                    setting.UserDefinedUsings = new ObservableCollectionFast<string>();
                    setting.UserDefinedUsings.Add("import { " + componentName.ToCamelCase().ToCamelCase('-') + "Component } from './" + componentName.ToCamelCase().ToLower() + ".component';");

                    setting.UserDefinedUsings.Add("import { " + componentName.ToCamelCase().ToCamelCase('-') + "RoutingModule } from './" + componentName.ToCamelCase().ToLower() + "-routing.module';");


                    if (this.angularProjectSetting.module_settings.sub_components.Any())
                    {
                        foreach (var component_name in this.angularProjectSetting.module_settings.sub_components)
                        {
                            setting.Declarations.Add(component_name + componentName.ToCamelCase().ToCamelCase('-') + "Component");
                            setting.UserDefinedUsings.Add("import { " + component_name + componentName.ToCamelCase().ToCamelCase('-') + "Component } from './edit-" + componentName.ToCamelCase().ToLower() + "/edit-" + componentName.ToCamelCase().ToLower() + ".component';");
                        }
                    }

                    content = GenerateAngularModuleClass(setting);
                    break;
                #endregion
                case AngularClassType.Model:
                    break;
                case AngularClassType.CSS:
                    break;
                case AngularClassType.SCSS:
                    break;
                case AngularClassType.Html:
                    break;
                case AngularClassType.ts:
                    break;
                default:
                    break;
            }

            return content;
        }

        public string GenerateAngularModuleClass(AngularClassFileSetting angularClassFileSetting)
        {
            AngularClassModel angularClassModel = new AngularClassModel();
            angularClassModel.ClassName = angularClassFileSetting.ClassName;
            angularClassModel.InheritedClass = angularClassFileSetting.InheritedClass;

            angularClassModel.AngularClassFileSetting = angularClassFileSetting;

            angularClassModel.ClassProperties = new ObservableCollectionFast<ClassProperty>();
            //angularServiceClassModel.ClassProperties.Add(new ClassProperty("procedureName", "string"));
            angularClassModel.ClassMethods = angularClassFileSetting.ClassMethods;

            string classData = angularClassModel.GenerateCSharpClassData(false);
            return classData;
        }

        private string GetFileExtension(AngularClassType moduleType)
        {
            string extension = ".ts";
            switch (moduleType)
            {
                case AngularClassType.Component:
                    extension = ".ts";
                    break;
                case AngularClassType.Service:
                    extension = ".ts";
                    break;
                case AngularClassType.RoutingModule:
                    extension = ".ts";
                    break;
                case AngularClassType.Module:
                    extension = ".ts";
                    break;
                case AngularClassType.Model:
                    extension = ".ts";
                    break;
                case AngularClassType.CSS:
                    extension = ".css";
                    break;
                case AngularClassType.SCSS:
                    extension = ".scss";
                    break;
                case AngularClassType.Html:
                    extension = ".html";
                    break;
                case AngularClassType.ts:
                    extension = ".ts";
                    break;
                default:
                    break;
            }
            return extension;
        }

        private List<Route> GetRouteChildren(DataStoreTable table)
        {
            List<Route> routes = new List<Route>();
            string tableName = this.angularProjectSetting.data_store_settings.is_use_trailing_name ? table.TableNameWithoutTrailS : table.TableName;

            Route contentRoute = new Route();
            contentRoute.Path = "";
            contentRoute.Component = table.TableName.ToCamelCase().ToCamelCase('-') + "Component";
            contentRoute.Data = new Data() { Title = table.TableName };
            contentRoute.CanActivate = "[AuthGuard]";
            routes.Add(contentRoute);

            if (this.angularProjectSetting.module_settings.sub_components.Any())
            {
                foreach (var component_name in this.angularProjectSetting.module_settings.sub_components)
                {
                    Route viewRoute = new Route();
                    viewRoute.Path = ":id";
                    viewRoute.Component = component_name.ToCamelCase().ToCamelCase('-') + table.TableName.ToCamelCase().ToCamelCase('-') + "Component";
                    viewRoute.Data = new Data() { Title = component_name + " " + tableName };
                    viewRoute.CanActivate = "[AuthGuard]";
                    routes.Add(viewRoute);
                }
            }

            return routes;
        }

        public AngularProjectSetting GetDefaultSettings()
        {
            return new AngularProjectSetting()
            {
                project_name = "project-name",
                component_base_path = "root",
                data_store_settings = new DataStoreSettings()
                {
                    table_name = "",
                    is_use_trailing_name = false,
                    trailing_name_count = 0,
                },
                module_settings = new ModuleSettings()
                {
                    module_path = "pages",
                    sub_components = new List<string>() { "Edit", "View" }
                },
                components = new List<Component>() {
                    new Component()
                    {
                        component_name = "MY_Work",
                        component_path = "Pages",
                        component_type = "Component",
                        sub_components =  new List<Component>() { 
                            new Component() { 
                                component_name = "View",
                            }},
                    }
                },
            };
        }

        private List<ClassMethodModel> GenerateComponentClassMethods()
        {
            List<ClassMethodModel> Methods = new List<ClassMethodModel>();
            ClassMethodModel ngOnInitMethod = new ClassMethodModel("", "void", "", "ngOnInit");
            Methods.Add(ngOnInitMethod);
            return Methods;
        }
    }
}
