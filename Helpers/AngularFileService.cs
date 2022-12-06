using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Model;
using ToDoStuff.Model.Angular;

namespace ToDoStuff.Helpers
{
    public class AngularFileService
    {
        public string FilePath { get; set; }
        public DataStoreTable table { get; set; }

        AngularClassFileSetting CurrentSetting { get; set; }

        public AngularFileService()
        {

        }

        public AngularFileService(DataStoreTable Table)
        {
            table = Table;
        }

        public String GenerateAngularModuleClass()
        {
            AngularClassModel angularClassModel = new AngularClassModel();
            angularClassModel.ClassName = CurrentSetting.ClassName;
            angularClassModel.InheritedClass = CurrentSetting.InheritedClass;

            angularClassModel.AngularClassFileSetting = CurrentSetting;

            angularClassModel.ClassProperties = new ObservableCollectionFast<ClassProperty>();
            //angularServiceClassModel.ClassProperties.Add(new ClassProperty("procedureName", "string"));
            angularClassModel.ClassMethods = CurrentSetting.ClassMethods;

            string classData = angularClassModel.GenerateCSharpClassData(false);
            return classData;
        }

        public AngularFileService GenerateAngularComponentPackage()
        {
            List<Route> mainMenuRoutes = new List<Route>();
            foreach (var table in new List<DataStoreTable>())
            {
                Route contentModuleRoute = new Route();
                contentModuleRoute.Path = table.TableName;
                contentModuleRoute.Data = new Data() { Title = table.TableName.ToCamelCase() };
                contentModuleRoute.LoadChildren = "import('./views/content/" + table.TableName + "/" + table.TableName + ".module').then((m) => m." + table.TableName.ToCamelCase() + "Module)";
                contentModuleRoute.CanActivate = "[AuthGuard]";
                mainMenuRoutes.Add(contentModuleRoute);

                //create table name folder
                var dir = Directory.CreateDirectory("/" + table.TableName);

                //module
                #region module
                AngularClassFileSetting setting = new AngularClassFileSetting();
                setting.ClassName = (table.TableName.ToLower() + ".module").ToCamelCase('.');
                setting.AngularClassType = AngularClassType.Module;
                setting.IsClassNameCamelCasing = false;
                setting.IsIncludeDefaultConstructor = false;
                setting.IsIncludeNameSpace = false;
                setting.IsIncludeUsings = false;
                setting.Declarations = new List<string>();
                setting.Declarations.Add(table.TableName.ToCamelCase() + "Component");
                setting.Declarations.Add("View" + table.TableName.ToCamelCase() + "Component");
                setting.Declarations.Add("Edit" + table.TableName.ToCamelCase() + "Component");

                setting.Imports = new List<string>();
                setting.Imports.Add(table.TableName.ToCamelCase() + "RoutingModule");

                setting.UserDefinedUsings = new ObservableCollectionFast<string>();
                setting.UserDefinedUsings.Add("import { " + table.TableName.ToCamelCase() + "Component } from './" + table.TableName + ".component';");
                setting.UserDefinedUsings.Add("import { View" + table.TableName.ToCamelCase() + "Component } from './view-" + table.TableName + "/view-" + table.TableName + ".component';");
                setting.UserDefinedUsings.Add("import { Edit" + table.TableName.ToCamelCase() + "Component } from './edit-" + table.TableName + "/edit-" + table.TableName + ".component';");
                setting.UserDefinedUsings.Add("import { " + table.TableName.ToCamelCase() + "RoutingModule } from './" + table.TableName + "-routing.module';");

                GenerateAngularModuleClass();
                #endregion

                //routing-module
                #region routing-module
                setting = new AngularClassFileSetting();
                setting.ClassName = (table.TableName.ToLower() + "-routing-module").ToCamelCase('-');
                setting.AngularClassType = AngularClassType.RoutingModule;
                setting.IsClassNameCamelCasing = false;
                setting.IsIncludeDefaultConstructor = false;
                setting.IsIncludeNameSpace = false;
                setting.IsIncludeUsings = false;

                setting.AngularRoute = new AngularRoute();
                var mainRoot = new Route()
                {
                    Path = "",
                    Data = new Data() { Title = table.TableName },
                    Children = GetRouteChildren(table)
                };
                setting.AngularRoute.Routes.Add(mainRoot);

                setting.UserDefinedUsings = new ObservableCollectionFast<string>();
                setting.UserDefinedUsings.Add("import { " + table.TableName.ToCamelCase() + "Component } from './" + table.TableName + ".component';");
                setting.UserDefinedUsings.Add("import { View" + table.TableName.ToCamelCase() + "Component } from './view-" + table.TableName + "/view-" + table.TableName + ".component';");
                setting.UserDefinedUsings.Add("import { Edit" + table.TableName.ToCamelCase() + "Component } from './edit-" + table.TableName + "/edit-" + table.TableName + ".component';");

                GenerateAngularModuleClass();
                #endregion

                //component.ts
                #region component.ts
                setting = new AngularClassFileSetting();
                setting.ClassName = (table.TableName.ToLower() + ".component").ToCamelCase('.');
                setting.AngularClassType = AngularClassType.Component;
                setting.IsClassNameCamelCasing = false;
                setting.IsIncludeDefaultConstructor = true;
                setting.IsIncludeNameSpace = false;
                setting.IsIncludeUsings = false;
                setting.InheritedClass = "OnInit";

                setting.Component = new AngularComponent();
                setting.Component.selector = "app-" + table.TableName + "";
                setting.Component.templateUrl = "./" + table.TableName + ".component.html";
                setting.Component.styleUrls = "./" + table.TableName + ".component.css";
                setting.ClassMethods = GenerateComponentClassMethods(table);
                GenerateAngularModuleClass();
                #endregion
                //component.html
                #region component.html
                FileUtility.SaveDataToFile(Path.Combine(dir.FullName, table.TableName.ToLower() + ".component" + ".html"),
                    "<p>" + table.TableName + " works!</p>", true);
                #endregion
                //component.css
                #region component.css
                FileUtility.SaveDataToFile(Path.Combine(dir.FullName, table.TableName.ToLower() + ".component" + ".css"),
                    "", true);
                #endregion

                //create sub-folder for edit
                var dirEdit = Directory.CreateDirectory(dir.FullName + "/edit-" + table.TableName);
                //edit-component.ts
                #region edit-component.ts
                setting = new AngularClassFileSetting();
                setting.ClassName = "Edit" + (table.TableName.ToLower() + ".component").ToCamelCase('.');
                setting.AngularClassType = AngularClassType.Component;
                setting.IsClassNameCamelCasing = false;
                setting.IsIncludeDefaultConstructor = true;
                setting.IsIncludeNameSpace = false;
                setting.IsIncludeUsings = false;
                setting.InheritedClass = "OnInit";

                setting.Component = new AngularComponent();
                setting.Component.selector = "app-edit-" + table.TableName + "";
                setting.Component.templateUrl = "./edit-" + table.TableName + ".component.html";
                setting.Component.styleUrls = "./edit-" + table.TableName + ".component.css";
                setting.ClassMethods = GenerateComponentClassMethods(table);
                GenerateAngularModuleClass();
                    #endregion
                //edit-component.html
                #region edit-component.html
                FileUtility.SaveDataToFile(Path.Combine(dirEdit.FullName, "edit-" + table.TableName.ToLower() + ".component" + ".html"),
                    "<p>" + "edit-" + table.TableName + " works!</p>", true);
                #endregion
                //edit-component.css
                #region edit-component.css
                FileUtility.SaveDataToFile(Path.Combine(dirEdit.FullName, "edit-" + table.TableName.ToLower() + ".component" + ".css"),
                    "", true);
                #endregion

                //create sub-folder for view
                var dirView = Directory.CreateDirectory(dir.FullName + "/view-" + table.TableName);
                //view-component.ts
                #region view-component.ts
                setting = new AngularClassFileSetting();
                setting.ClassName = "View" + (table.TableName.ToLower() + ".component").ToCamelCase('.');
                setting.AngularClassType = AngularClassType.Component;
                setting.IsClassNameCamelCasing = false;
                setting.IsIncludeDefaultConstructor = true;
                setting.IsIncludeNameSpace = false;
                setting.IsIncludeUsings = false;
                setting.InheritedClass = "OnInit";

                setting.Component = new AngularComponent();
                setting.Component.selector = "app-view-" + table.TableName + "";
                setting.Component.templateUrl = "./view-" + table.TableName + ".component.html";
                setting.Component.styleUrls = "./view-" + table.TableName + ".component.css";
                setting.ClassMethods = GenerateComponentClassMethods(table);
                GenerateAngularModuleClass();
                #endregion
                //view-component.html
                #region view-component.html
                FileUtility.SaveDataToFile(Path.Combine(dirView.FullName, "view-" + table.TableName.ToLower() + ".component" + ".html"),
                    "<p>" + "view-" + table.TableName + " works!</p>", true);
                #endregion
                //view-component.css
                #region view-component.css
                FileUtility.SaveDataToFile(Path.Combine(dirView.FullName, "view-" + table.TableName.ToLower() + ".component" + ".css"),
                    "", true);
                #endregion
            }

            #region app-routing.module
            AngularClassFileSetting AppRoutingSetting = new AngularClassFileSetting();
            AppRoutingSetting.ClassName = "AppRoutingModule";
            AppRoutingSetting.AngularClassType = AngularClassType.RoutingModule;
            AppRoutingSetting.IsClassNameCamelCasing = false;
            AppRoutingSetting.IsIncludeDefaultConstructor = false;
            AppRoutingSetting.IsIncludeNameSpace = false;
            AppRoutingSetting.IsIncludeUsings = false;

            AppRoutingSetting.AngularRoute = new AngularRoute();
            var dashboardRoute = new Route()
            {
                Path = "dashboard",
                RedirectTo = "dashboard",
                PathMatch = "full"
            };

            var mainAppRoot = new Route()
            {
                Path = "",
                Component = "LayoutComponent",
                Data = new Data() { Title = "Home" },
                Children = mainMenuRoutes
            };

            AppRoutingSetting.AngularRoute.Routes.Add(dashboardRoute);
            AppRoutingSetting.AngularRoute.Routes.Add(mainAppRoot);

            AppRoutingSetting.UserDefinedUsings = new ObservableCollectionFast<string>();
            AppRoutingSetting.UserDefinedUsings.Add("import { LayoutComponent } from './containers';");
            AppRoutingSetting.UserDefinedUsings.Add("import { Page404Component } from './views/pages/page404/page404.component';");
            AppRoutingSetting.UserDefinedUsings.Add("import { Page500Component } from './views/pages/page500/page500.component';");
            AppRoutingSetting.UserDefinedUsings.Add("import { LoginComponent } from './views/pages/login/login.component';");
            AppRoutingSetting.UserDefinedUsings.Add("import { RegisterComponent } from './views/pages/register/register.component';");
            AppRoutingSetting.UserDefinedUsings.Add("import { AuthGuard } from './utility/AuthGuard';");

            GenerateAngularModuleClass();
            #endregion
            return this;
        }

        public string GenerateFromTemplateFile(string templateName)
        {
            string htmlContent = string.Empty;
            string template = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "Angular", "component" , templateName);

            htmlContent = File.ReadAllText(template);

            htmlContent = htmlContent.Replace("[TABLE_NAME]", table.TableName.Trim().ToCamelCase());//Characters
            htmlContent = htmlContent.Replace("[TABLE_NAME_SMALL]", table.TableName.ToLower());//characters
            htmlContent = htmlContent.Replace("[TABLE_NAME_S]", table.TableNameWithoutTrailS);//Character
            htmlContent = htmlContent.Replace("[TABLE_NAME_S_SMALL]", table.TableNameWithoutTrailS.ToLower());//character
            return htmlContent;
        }

        private List<ClassMethodModel> GenerateComponentClassMethods(DataStoreTable table)
        {
            List<ClassMethodModel> Methods = new List<ClassMethodModel>();
            ClassMethodModel ngOnInitMethod = new ClassMethodModel("", "void", "", "ngOnInit");
            Methods.Add(ngOnInitMethod);
            return Methods;
        }

        private List<Route> GetRouteChildren(DataStoreTable table)
        {
            List<Route> routes = new List<Route>();

            Route contentRoute = new Route();
            contentRoute.Path = "";
            contentRoute.Component = table.TableName.ToCamelCase() + "Component";
            contentRoute.Data = new Data() { Title = table.TableName };
            contentRoute.CanActivate = "[AuthGuard]";
            routes.Add(contentRoute);

            Route viewRoute = new Route();
            viewRoute.Path = ":id";
            viewRoute.Component = "View" + table.TableName.ToCamelCase() + "Component";
            viewRoute.Data = new Data() { Title = "View " + table.TableNameWithoutTrailS };
            viewRoute.CanActivate = "[AuthGuard]";
            routes.Add(viewRoute);

            Route editRoute = new Route();
            editRoute.Path = ":id/edit";
            editRoute.Component = "Edit" + table.TableName.ToCamelCase() + "Component";
            editRoute.Data = new Data() { Title = "Edit " + table.TableNameWithoutTrailS };
            editRoute.CanActivate = "[AuthGuard]";
            routes.Add(editRoute);

            return routes;
        }

        public void SaveToFile(string filePath, string fileContents)
        {
            FileUtility.SaveDataToFile(filePath, fileContents, true);
        }

        public AngularFileService GetModuleSettings()
        {
            #region module
            AngularClassFileSetting setting = new AngularClassFileSetting();
            setting.ClassName = (table.TableName.ToLower() + ".module").ToCamelCase('.');
            setting.AngularClassType = AngularClassType.Module;
            setting.IsClassNameCamelCasing = false;
            setting.IsIncludeDefaultConstructor = false;
            setting.IsIncludeNameSpace = false;
            setting.IsIncludeUsings = false;
            setting.Declarations = new List<string>();
            setting.Declarations.Add(table.TableName.ToCamelCase() + "Component");
            setting.Declarations.Add("View" + table.TableName.ToCamelCase() + "Component");
            setting.Declarations.Add("Edit" + table.TableName.ToCamelCase() + "Component");

            setting.Imports = new List<string>();
            setting.Imports.Add(table.TableName.ToCamelCase() + "RoutingModule");

            setting.UserDefinedUsings = new ObservableCollectionFast<string>();
            setting.UserDefinedUsings.Add("import { " + table.TableName.ToCamelCase() + "Component } from './" + table.TableName + ".component';");
            setting.UserDefinedUsings.Add("import { View" + table.TableName.ToCamelCase() + "Component } from './view-" + table.TableName + "/view-" + table.TableName + ".component';");
            setting.UserDefinedUsings.Add("import { Edit" + table.TableName.ToCamelCase() + "Component } from './edit-" + table.TableName + "/edit-" + table.TableName + ".component';");
            setting.UserDefinedUsings.Add("import { " + table.TableName.ToCamelCase() + "RoutingModule } from './" + table.TableName + "-routing.module';");
            CurrentSetting = setting;
            return this;
            #endregion
        }

        public AngularFileService GetRoutingModuleSettings()
        {
            //routing-module
            #region routing-module
            AngularClassFileSetting setting = new AngularClassFileSetting();
            setting.ClassName = (table.TableName.ToLower() + "-routing-module").ToCamelCase('-');
            setting.AngularClassType = AngularClassType.RoutingModule;
            setting.IsClassNameCamelCasing = false;
            setting.IsIncludeDefaultConstructor = false;
            setting.IsIncludeNameSpace = false;
            setting.IsIncludeUsings = false;

            setting.AngularRoute = new AngularRoute();
            var mainRoot = new Route()
            {
                Path = "",
                Data = new Data() { Title = table.TableName },
                Children = GetRouteChildren(table)
            };
            setting.AngularRoute.Routes.Add(mainRoot);

            setting.UserDefinedUsings = new ObservableCollectionFast<string>();
            setting.UserDefinedUsings.Add("import { " + table.TableName.ToCamelCase() + "Component } from './" + table.TableName + ".component';");
            setting.UserDefinedUsings.Add("import { View" + table.TableName.ToCamelCase() + "Component } from './view-" + table.TableName + "/view-" + table.TableName + ".component';");
            setting.UserDefinedUsings.Add("import { Edit" + table.TableName.ToCamelCase() + "Component } from './edit-" + table.TableName + "/edit-" + table.TableName + ".component';");
            CurrentSetting = setting;
            return this;
            #endregion
        }

        public AngularFileService GetcomponentSettings()
        {
            #region component.ts
            AngularClassFileSetting setting = new AngularClassFileSetting();
            setting.ClassName = (table.TableName.ToLower() + ".component").ToCamelCase('.');
            setting.AngularClassType = AngularClassType.Component;
            setting.IsClassNameCamelCasing = false;
            setting.IsIncludeDefaultConstructor = true;
            setting.IsIncludeNameSpace = false;
            setting.IsIncludeUsings = false;
            setting.InheritedClass = "OnInit";

            setting.Component = new AngularComponent();
            setting.Component.selector = "app-" + table.TableName + "";
            setting.Component.templateUrl = "./" + table.TableName + ".component.html";
            setting.Component.styleUrls = "./" + table.TableName + ".component.css";
            setting.ClassMethods = GenerateComponentClassMethods(table);
            CurrentSetting = setting;
            return this;
            #endregion
        }
    }
}
