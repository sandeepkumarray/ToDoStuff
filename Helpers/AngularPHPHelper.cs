using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Model;
using ToDoStuff.Model.Angular;
using ToDoStuff.Model.Angular.MethodModel;
using ToDoStuff.Model.PHP;

namespace ToDoStuff.Helpers
{
    public class AngularPHPHelper
    {
        public DataStore DataStore { get; set; }
        public string FileName { get; set; }
        public PhpFileModel PhpFileModel { get; set; }
        public string FileContent { get; private set; }

        public AngularPHPHelper(string FileName)
        {
            this.FileName = FileName;
        }

        public AngularPHPHelper SetDataStore(DataStore dataStore)
        {
            this.DataStore = dataStore;
            return this;
        }

        public AngularPHPHelper SetPhpFileContent()
        {
            PhpFileModel = new PhpFileModel();

            PhpFileModel.Headers.Add("header('Access-Control-Allow-Origin: *');");
            PhpFileModel.Headers.Add("header('Access-Control-Allow-Methods: GET, POST');");
            PhpFileModel.Headers.Add("header('Access-Control-Allow-Headers: X-Requested-With');");

            PhpIncludeModel config = new PhpIncludeModel("require_once", "config.php");
            PhpIncludeModel responseClass = new PhpIncludeModel("require_once", "responseClass.php");
            PhpIncludeModel logWriter = new PhpIncludeModel("require", "logWriter.php");

            PhpFileModel.IncludeList.AddRange(new PhpIncludeModel[] { config, responseClass, logWriter });

            PhpVariableModel dbResponse = new PhpVariableModel("response", "dbResponse");
            PhpVariableModel log = new PhpVariableModel("log", "logWriter");

            PhpFileModel.VariableList.AddRange(new PhpVariableModel[] { dbResponse, log });

            List<PhpSection> funSection = GetFunctionsFromeStore(DataStore);
            PhpFileModel.SectionList.AddRange(funSection);

            PhpSection phpSection_POST = new PhpSection();
            StringBuilder phpSection_POST_content = new StringBuilder();
            phpSection_POST_content.AppendLine("if ($_SERVER[\"REQUEST_METHOD\"] == \"POST\") {");
            phpSection_POST_content.AppendLine("\t$postdata = file_get_contents(\"php://input\");");
            phpSection_POST_content.AppendLine("\t$request = json_decode($postdata);");
            phpSection_POST_content.AppendLine("\t$data = $request->data;");
            phpSection_POST_content.AppendLine("\t$procedureName = $data->procedureName;");
            string sectionMethodsPOST = GetPOSTMethods(DataStore);
            phpSection_POST_content.AppendLine(sectionMethodsPOST);

            phpSection_POST_content.AppendLine("\tmysqli_close($link);");
            phpSection_POST_content.AppendLine("\techo json_encode($response->data);");
            phpSection_POST_content.AppendLine("}");
            phpSection_POST.SectionBody = phpSection_POST_content.ToString();
            PhpFileModel.SectionList.Add(phpSection_POST);

            PhpSection phpSection_GET = new PhpSection();
            StringBuilder phpSection_GET_content = new StringBuilder();
            phpSection_GET_content.AppendLine("if ($_SERVER[\"REQUEST_METHOD\"] == \"GET\") {");
            phpSection_GET_content.AppendLine("\t$procedureName = $_GET['procedureName'];");
            string sectionMethodsGET = GetGETMethods(DataStore);
            phpSection_GET_content.AppendLine(sectionMethodsGET);
            phpSection_GET_content.AppendLine("\t// Close connection");
            phpSection_GET_content.AppendLine("\tmysqli_close($link);");
            phpSection_GET_content.AppendLine("\techo json_encode($response->data);");
            phpSection_GET_content.AppendLine("}");
            phpSection_GET.SectionBody = phpSection_GET_content.ToString();
            PhpFileModel.SectionList.Add(phpSection_GET);
            return this;
        }

        private List<PhpSection> GetFunctionsFromeStore(DataStore dataStore)
        {
            List<PhpSection> result = new List<PhpSection>();
            foreach (var table in dataStore.Tables)
            {
                PhpSection phpSection = new PhpSection();
                StringBuilder content = new StringBuilder();

                PhpMethodModel getAllMethod = new SelectAllPhpMethodModel("getAll_" + table.TableName.ToCamelCase(), table.TableName, table.ColumnList, TypeOfSQLObject.SelectAll);
                content.AppendLine(getAllMethod.ToString());

                PhpMethodModel getMethod = new SelectPhpMethodModel("get_" + table.TableName.ToCamelCase(), table.TableName, table.ColumnList, TypeOfSQLObject.Select);
                content.AppendLine(getMethod.ToString());

                PhpMethodModel insertMethod = new InsertPhpMethodModel("add_" + table.TableName.ToCamelCase(), table.TableName, table.ColumnList, TypeOfSQLObject.Insert);
                content.AppendLine(insertMethod.ToString());

                PhpMethodModel deleteMethod = new DeletePhpMethodModel("delete_" + table.TableName.ToCamelCase(), table.TableName, table.ColumnList, TypeOfSQLObject.Delete);
                content.AppendLine(deleteMethod.ToString());

                PhpMethodModel updateMethod = new UpdatePhpMethodModel("update_" + table.TableName.ToCamelCase(), table.TableName, table.ColumnList, TypeOfSQLObject.Update);
                content.AppendLine(updateMethod.ToString());

                phpSection.SectionBody = content.ToString();
                result.Add(phpSection);
            }
            return result;
        }

        private string GetPOSTMethods(DataStore dataStore)
        {
            StringBuilder POSTMethods_contents = new StringBuilder();
            foreach (var table in dataStore.Tables)
            {
                PhpFunctionCondition saveFunction = new PhpFunctionCondition("add" + table.TableNameWithoutTrailS.ToCamelCase());
                POSTMethods_contents.AppendLine(saveFunction.ToString());

                PhpFunctionCondition updateFunction = new PhpFunctionCondition("update" + table.TableNameWithoutTrailS.ToCamelCase());
                POSTMethods_contents.AppendLine(updateFunction.ToString());

                PhpFunctionCondition deleteFunction = new PhpFunctionCondition("delete" + table.TableNameWithoutTrailS.ToCamelCase());
                POSTMethods_contents.AppendLine(deleteFunction.ToString());
            }
            return POSTMethods_contents.ToString();
        }

        private string GetGETMethods(DataStore dataStore)
        {
            StringBuilder GETMethods_contents = new StringBuilder();
            foreach (var table in dataStore.Tables)
            {
                PhpFunctionCondition getAllFunction = new PhpFunctionCondition("getAll" + table.TableName.ToCamelCase(), false);
                GETMethods_contents.AppendLine(getAllFunction.ToString());

                PhpFunctionCondition getFunction = new PhpFunctionCondition("get" + table.TableName.ToCamelCase(), false);
                GETMethods_contents.AppendLine(getFunction.ToString());

            }
            return GETMethods_contents.ToString();
        }

        public AngularPHPHelper GeneratePHPFile()
        {
            StringBuilder php_contents = new StringBuilder();
            php_contents.AppendLine("<?php");
            php_contents.AppendLine(this.PhpFileModel.ToString());
            php_contents.AppendLine("?>");
            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                DataStore.Database.Trim(), "php_includes", FileName),
                php_contents.ToString(), true);
            return this;
        }

        public AngularPHPHelper GeneratePostmanDoc(PostmanCollectionSetting collectionSetting)
        {
            PostmanCollectionModel postmanCollectionModel = new PostmanCollectionModel();
            //string filePath = @"C:\Users\sande\Desktop\my_book\My_Book_API.postman_collection.json";

            //postmanCollectionModel = JsonConvert.DeserializeObject<PostmanCollectionModel>(File.ReadAllText(filePath));

            postmanCollectionModel = new ToDoStuff.Model.PostmanCollectionModel();

            postmanCollectionModel.info = new ToDoStuff.Model.Info();
            postmanCollectionModel.info._postman_id = Guid.NewGuid().ToString();
            postmanCollectionModel.info.name = collectionSetting.CollectionName;
            postmanCollectionModel.info.schema = "https://schema.getpostman.com/json/collection/v2.1.0/collection.json";

            foreach (var table in DataStore.Tables)
            {
                ToDoStuff.Model.Item tableItem = new ToDoStuff.Model.Item();
                tableItem.description = string.Format(collectionSetting.Description, table.TableName.ToCamelCase());
                tableItem.name = table.TableName.ToCamelCase();
                tableItem.request = null;
                tableItem.response = null;

                List<string> methodTypes = new List<string>(new[] { "add", "update", "get", "delete", "getAll" });
                foreach (var mtype in methodTypes)
                {
                    List<Query> urlQuery = new List<Query>();
                    string ResponseModelType = "string";
                    string methodType = "POST";
                    string apiName = mtype + table.TableName.ToCamelCase();

                    if (mtype == "get")
                        ResponseModelType = table.TableName + "Model";

                    if (mtype == "getAll")
                        ResponseModelType = "List<" + table.TableName + "Model>";

                    if (mtype == "getAll" || mtype == "get")
                    {
                        Query procedureName = new Query() { key = "procedureName", value = mtype + table.TableName.ToCamelCase() };
                        Query userid = new Query() { key = "user_id", value = "" };

                        urlQuery.Add(procedureName);

                        if (mtype == "getAll")
                        {
                            urlQuery.Add(userid);
                        }
                        if (mtype == "get")
                        {
                            Query id = new Query() { key = "id", value = "" };
                            urlQuery.Add(userid);
                            urlQuery.Add(id);
                        }

                        methodType = "GET";
                    }
                    else
                    {
                        methodType = "POST";
                        apiName = mtype + table.TableNameWithoutTrailS.ToCamelCase();
                    }

                    JObject jsonObjData = new JObject();
                    JObject jsonObj = new JObject();
                    jsonObj.Add("procedureName", apiName);

                    foreach (var item in table.ColumnList)
                    {
                        jsonObj.Add(item.PropName, "");
                    }

                    jsonObjData.Add("data", jsonObj);

                    ToDoStuff.Model.Item apiItem = new ToDoStuff.Model.Item();
                    apiItem.description = null;
                    apiItem.item = null;
                    apiItem.name = apiName;
                    apiItem.request = new ToDoStuff.Model.Request();
                    apiItem.request.body = new ToDoStuff.Model.Body();
                    apiItem.request.body.mode = "raw";
                    apiItem.request.body.options = new ToDoStuff.Model.Options();
                    apiItem.request.body.options.raw = new ToDoStuff.Model.Raw();
                    apiItem.request.body.options.raw.language = "json";
                    apiItem.request.body.raw = jsonObjData.ToString();
                    apiItem.request.header = new List<Header>();
                    apiItem.request.header.Add(new Header() { key = "Content-Type", value = "application/json", type = "text" });
                    apiItem.request.@method = methodType;
                    apiItem.request.url = new Url();
                    apiItem.request.url.raw = "http://localhost/code_drops/my-world-app/php_includes/api_content.php";
                    apiItem.request.url.protocol = "http";
                    apiItem.request.url.host = collectionSetting.host;
                    //apiItem.request.url.host.AddRange(new[] { "www", "my-world", "com" });
                    //apiItem.request.url.host.AddRange(new[] { "localhost" });
                    //http://localhost/code_drops/my-world-app/
                    apiItem.request.url.path = collectionSetting.path;
                    //apiItem.request.url.path.AddRange(new[] { "code_drops", "my-world-app", "php_includes", "api_content.php" });

                    if (urlQuery.Count > 0)
                    {
                        apiItem.request.url.query = new List<Query>();
                        apiItem.request.url.query.AddRange(urlQuery);
                    }
                    apiItem.response = null;

                    tableItem.item.Add(apiItem);
                }

                postmanCollectionModel.item.Add(tableItem);
            }

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), DataStore.Database.Trim(), FileName),
               JsonConvert.SerializeObject(postmanCollectionModel), true);
            return this;
        }

        public AngularPHPHelper GenerateAngularServiceClass(AngularClassDataSetting methodSetting)
        {
            AngularClassModel angularServiceClassModel = new AngularClassModel();
            angularServiceClassModel.ClassName = Path.GetFileNameWithoutExtension(FileName).ToCamelCase('.');

            angularServiceClassModel.AngularClassFileSetting.AngularClassType = AngularClassType.Service;

            angularServiceClassModel.AngularClassFileSetting.IsClassNameCamelCasing = false;
            angularServiceClassModel.AngularClassFileSetting.IsIncludeDefaultConstructor = true;
            angularServiceClassModel.AngularClassFileSetting.IsIncludeNameSpace = true;
            angularServiceClassModel.AngularClassFileSetting.IsIncludeUsings = true;
            angularServiceClassModel.AngularClassFileSetting.UserDefinedUsings = new ObservableCollectionFast<string>();
            angularServiceClassModel.AngularClassFileSetting.UserDefinedUsings.AddRange(
                from t in DataStore.Tables
                select "import { " + t.TableName.ToCamelCase() + " } from '../model/" + t.TableName.ToCamelCase() + "'"
                );

            angularServiceClassModel.AngularClassFileSetting.IsIncludeParametrizedConstructor = true;
            angularServiceClassModel.AngularClassFileSetting.Parameters = new ObservableCollectionFast<ClassProperty>();
            angularServiceClassModel.AngularClassFileSetting.Parameters.Add(new ClassProperty("http", "HttpClient") { });
            angularServiceClassModel.AngularClassFileSetting.Parameters.Add(new ClassProperty("router", "Router") { });

            angularServiceClassModel.ClassMethods = new List<ClassMethodModel>();
            angularServiceClassModel.ClassMethods = CreateApiServiceMethods(methodSetting);

            angularServiceClassModel.ClassProperties = methodSetting.ClassProperties;

            string classData = angularServiceClassModel.GenerateCSharpClassData(false);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                DataStore.Database.Trim(), "service", FileName),
                classData, true);
            return this;
        }

        public AngularPHPHelper GenerateAngularModelClass()
        {
            foreach (var table in DataStore.Tables)
            {
                AngularClassModel angularServiceClassModel = new AngularClassModel();
                angularServiceClassModel.ClassName = table.TableName.ToCamelCase();

                angularServiceClassModel.AngularClassFileSetting.AngularClassType = AngularClassType.Model;

                angularServiceClassModel.AngularClassFileSetting.IsClassNameCamelCasing = false;
                angularServiceClassModel.AngularClassFileSetting.IsIncludeDefaultConstructor = false;
                angularServiceClassModel.AngularClassFileSetting.IsIncludeNameSpace = false;
                angularServiceClassModel.AngularClassFileSetting.IsIncludeUsings = false;
                angularServiceClassModel.InheritedClass = "BaseModel";
                angularServiceClassModel.AngularClassFileSetting.UserDefinedUsings = new ObservableCollectionFast<string>();
                angularServiceClassModel.AngularClassFileSetting.UserDefinedUsings.Add("import { BaseModel } from \"./BaseModel\";");


                angularServiceClassModel.ClassProperties = new ObservableCollectionFast<ClassProperty>();
                //angularServiceClassModel.ClassProperties.Add(new ClassProperty("procedureName", "string"));
                angularServiceClassModel.ClassProperties.AddRange(new ObservableCollectionFast<ClassProperty>(table.ColumnList));

                string classData = angularServiceClassModel.GenerateCSharpClassData(false);

                FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    DataStore.Database.Trim(), "model", table.TableName.ToCamelCase() + ".ts"),
                    classData, true);

            }
            return this;
        }

        private List<ClassMethodModel> CreateApiServiceMethods(AngularClassDataSetting methodSetting)
        {
            List<ClassMethodModel> classMethods = new List<ClassMethodModel>();
            foreach (var table in DataStore.Tables)
            {
                ClassMethodModel classGETMethod = new AngularServiceGETMethodModel("Observable<" + table.TableName.ToCamelCase() + ">", "get" + table.TableName.ToCamelCase())
                {
                    ClassProperties = new ObservableCollectionFast<ClassProperty>(table.ColumnList),
                    methodSetting = methodSetting,
                    TableName = table.TableName.ToCamelCase()
                }.Initialize();
                classMethods.Add(classGETMethod);

                ClassMethodModel classGETALLMethod = new AngularServiceGETALLMethodModel("Observable<" + table.TableName.ToCamelCase() + "[]" + ">", "getAll" + table.TableName.ToCamelCase())
                {
                    ClassProperties = new ObservableCollectionFast<ClassProperty>(table.ColumnList),
                    methodSetting = methodSetting,
                    TableName = table.TableName.ToCamelCase()
                }.Initialize();
                classMethods.Add(classGETALLMethod);

                ClassMethodModel classAddPOSTMethod = new AngularServicePOSTMethodModel("Observable<ResponseModel>", "add" + table.TableNameWithoutTrailS.ToCamelCase())
                {
                    ClassProperties = new ObservableCollectionFast<ClassProperty>(table.ColumnList),
                    methodSetting = methodSetting,
                    TableName = table.TableName.ToCamelCase()
                }.Initialize();
                classMethods.Add(classAddPOSTMethod);

                ClassMethodModel classDeletePOSTMethod = new AngularServicePOSTMethodModel("Observable<ResponseModel>", "delete" + table.TableNameWithoutTrailS.ToCamelCase())
                {
                    ClassProperties = new ObservableCollectionFast<ClassProperty>(table.ColumnList),
                    methodSetting = methodSetting,
                    TableName = table.TableName.ToCamelCase()
                }.Initialize();
                classMethods.Add(classDeletePOSTMethod);

                ClassMethodModel classUpdatePOSTMethod = new AngularServicePOSTMethodModel("Observable<ResponseModel>", "update" + table.TableNameWithoutTrailS.ToCamelCase())
                {
                    ClassProperties = new ObservableCollectionFast<ClassProperty>(table.ColumnList),
                    methodSetting = methodSetting,
                    TableName = table.TableName.ToCamelCase()
                }.Initialize();
                classMethods.Add(classUpdatePOSTMethod);
            }
            return classMethods;
        }

        public AngularPHPHelper GenerateAngularModuleClass(DataStoreTable table, AngularClassFileSetting angularClassFileSetting)
        {
            AngularClassModel angularClassModel = new AngularClassModel();
            angularClassModel.ClassName = angularClassFileSetting.ClassName;
            angularClassModel.InheritedClass = angularClassFileSetting.InheritedClass;

            angularClassModel.AngularClassFileSetting = angularClassFileSetting;

            angularClassModel.ClassProperties = new ObservableCollectionFast<ClassProperty>();
            //angularServiceClassModel.ClassProperties.Add(new ClassProperty("procedureName", "string"));
            angularClassModel.ClassMethods = angularClassFileSetting.ClassMethods;

            string classData = angularClassModel.GenerateCSharpClassData(false);
            this.FileContent = classData;
            return this;
        }

        public AngularPHPHelper GenerateAngularComponentPackage()
        {
            var baseFolderName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                       DataStore.Database.Trim(), "content");

            List<Route> mainMenuRoutes = new List<Route>();
            foreach (var table in DataStore.Tables)
            {
                Route contentModuleRoute = new Route();
                contentModuleRoute.Path = table.TableName;
                contentModuleRoute.Data = new Data() { Title = table.TableName.ToCamelCase() };
                contentModuleRoute.LoadChildren = "import('./views/content/" + table.TableName + "/" + table.TableName + ".module').then((m) => m." + table.TableName.ToCamelCase() + "Module)";
                contentModuleRoute.CanActivate = "[AuthGuard]";
                mainMenuRoutes.Add(contentModuleRoute);

                //create table name folder
                var dir = Directory.CreateDirectory(baseFolderName + "/" + table.TableName);

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

                GenerateAngularModuleClass(table, setting)
                    .SaveToFile(Path.Combine(dir.FullName, table.TableName.ToLower() + ".module" + ".ts"));
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

                GenerateAngularModuleClass(table, setting)
                    .SaveToFile(Path.Combine(dir.FullName, table.TableName.ToLower() + "-routing.module" + ".ts"));
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
                GenerateAngularModuleClass(table, setting)
                    .SaveToFile(Path.Combine(dir.FullName, table.TableName.ToLower() + ".component" + ".ts"));
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
                GenerateAngularModuleClass(table, setting)
                    .SaveToFile(Path.Combine(dirEdit.FullName, "edit-" + table.TableName.ToLower() + ".component" + ".ts"));
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
                GenerateAngularModuleClass(table, setting)
                    .SaveToFile(Path.Combine(dirView.FullName, "view-" + table.TableName.ToLower() + ".component" + ".ts"));
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

            GenerateAngularModuleClass(null, AppRoutingSetting)
                .SaveToFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    DataStore.Database.Trim(), "app", "app-routing.module" + ".ts"));
            #endregion
            return this;
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

        public void SaveToFile(string filePath)
        {
            FileUtility.SaveDataToFile(filePath, this.FileContent, true);
        }
    }
}
