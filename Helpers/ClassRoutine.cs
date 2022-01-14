using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Model;

namespace ToDoStuff
{
    public class ClassRoutine
    {
        public string Database { get; set; }
        public string TableName { get; set; }
        public List<ClassProperty> ColumnList { get; set; }
        public CSharpClass ControllerClass { get; set; }
        public CSharpClass APIServiceClientClass { get; set; }

        string TableNameWithoutTrailS { get; set; }

        public ClassRoutine(string tableName, List<ClassProperty> columnList)
        {
            TableName = tableName;
            ColumnList = columnList;
            TableNameWithoutTrailS = TableName.Trim().Substring(0, TableName.Trim().Length - 1).ToCamelCase();
        }

        public ClassRoutine GenerateControllerClass()
        {
            try
            {
                ControllerClass = new CSharpClass(TableName.Trim() + "_Controller");

                ControllerClass.CSharpClassFileSettings.Attributes.Add("[Authorize]");
                ControllerClass.CSharpClassFileSettings.Attributes.Add("[Route(\"" + TableName.Trim() + "\")]");

                ControllerClass.CSharpClassFileSettings.IsClassNameCamelCasing = true;
                ControllerClass.CSharpClassFileSettings.IsIncludeNameSpace = true;
                ControllerClass.CSharpClassFileSettings.IsIncludeUsings = true;
                ControllerClass.CSharpClassFileSettings.NameSpace = "My.World.Web.Controllers";

                ControllerClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.IO;");
                ControllerClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Threading.Tasks;");
                ControllerClass.CSharpClassFileSettings.UserDefinedUsings.Add("using AutoMapper;");
                ControllerClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Microsoft.AspNetCore.Authorization;");
                ControllerClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Microsoft.AspNetCore.Http;");
                ControllerClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Microsoft.AspNetCore.Mvc;");
                ControllerClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.Models;");
                ControllerClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Web.Services;");
                ControllerClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Newtonsoft.Json;");
                ControllerClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Web.ViewModel;");

                ControllerClass.CSharpClassFileSettings.IsIncludeParametrizedConstructor = true;
                ControllerClass.CSharpClassFileSettings.Parameters = new System.Collections.ObjectModel.ObservableCollection<ClassProperty>();
                ControllerClass.CSharpClassFileSettings.Parameters.Add(new ClassProperty("i" + TableName.Trim().ToCamelCase() + "ApiService", "I" + TableName.Trim().ToCamelCase() + "ApiService") { });
                ControllerClass.CSharpClassFileSettings.Parameters.Add(new ClassProperty("iUniversesApiService", "IUniversesApiService") { });

                StringBuilder sbContructorContent = new StringBuilder();
                sbContructorContent.AppendLine("_i" + TableName.Trim().ToCamelCase() + "ApiService = i" + TableName.Trim().ToCamelCase() + "ApiService;");
                sbContructorContent.AppendLine("\t\t\t_iUniversesApiService = iUniversesApiService;");
                ControllerClass.CSharpClassFileSettings.ParameterizedConstructorContent = sbContructorContent.ToString();

                ControllerClass.ClassMethods = CreateControllerMethods();
                ControllerClass.ClassProperties = new System.Collections.ObjectModel.ObservableCollection<ClassProperty>();

                ControllerClass.ClassProperties.Add(new ClassProperty("_i" + TableName.Trim().ToCamelCase() + "ApiService", "readonly I" + TableName.Trim().ToCamelCase() + "ApiService"));
                ControllerClass.ClassProperties.Add(new ClassProperty("_iUniversesApiService", "readonly IUniversesApiService"));

                ControllerClass.InheritedClass = "Controller";

                string classData = ControllerClass.GenerateCSharpClassData(false);

                FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    Database.Trim(), "Controller", ControllerClass.ClassName + ".cs"),
                    classData, true);
            }
            catch (Exception)
            {
                throw;
            }
            return this;
        }

        private List<ClassMethodModel> CreateControllerMethods()
        {
            List<ClassMethodModel> methods = new List<ClassMethodModel>();
            List<string> methodTypes = new List<string>(new[] { "Index", "View", "Delete" });
            List<string> blockColumnList = new List<string>() { "id", "created_at", "updated_at", "user_id", "archived_at", "deleted_at" };
            StringBuilder sb = new StringBuilder();

            #region GetContent
            ClassMethodModel GetRawContent = new ClassMethodModel("private", "string", "", "GetRawContent");
            GetRawContent.Parameters = new List<ClassProperty>();
            GetRawContent.Parameters.Add(new ClassProperty("_rawContent", "string"));
            sb.AppendLine("\t\t\tusing (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t    return reader.ReadToEndAsync().Result;");
            sb.AppendLine("\t\t\t}");
            GetRawContent.MethodBody = sb.ToString();
            methods.Add(GetRawContent);
            #endregion

            #region Index
            ClassMethodModel indexAction = new ClassMethodModel("public", "IActionResult", "", "Index");

            indexAction.Attributes.Add("[Route(\"Index\")]");
            
            sb = new StringBuilder();
            sb.AppendLine("\t\t\tvar accountID = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == \"UserID\")?.Value);");
            sb.AppendLine("\t\t\tvar " + TableName.Trim() + " = _i" + TableName.Trim().ToCamelCase() + "ApiService.GetAll" + TableName.Trim().ToCamelCase() + "(accountID);");
            sb.AppendLine("\t\t\treturn View(" + TableName.Trim() + ");");
            indexAction.MethodBody = sb.ToString();
            methods.Add(indexAction);
            #endregion

            #region View
            ClassMethodModel viewAction = new ClassMethodModel("public", "IActionResult", "", "View" + TableName.Trim().ToCamelCase());

            sb = new StringBuilder();

            viewAction.Attributes.Add("[HttpGet]");
            viewAction.Attributes.Add("[Route(\"{Id}\")]");

            viewAction.Parameters = new List<ClassProperty>();
            viewAction.Parameters.Add(new ClassProperty("Id", "string"));


            sb.AppendLine("\t\t\t" + TableName.Trim().ToCamelCase() + "Model model = new " + TableName.Trim().ToCamelCase() + "Model();");
            sb.AppendLine("\t\t\tmodel.user_id = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == \"UserID\")?.Value);");
            sb.AppendLine("\t\t\tmodel.id = Convert.ToInt32(Id);");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\tTempData[\"" + TableNameWithoutTrailS + "ID\"] = Id;");
            sb.AppendLine("\t\t\tViewData[\"" + TableNameWithoutTrailS + "ID\"] = Id;");
            sb.AppendLine("\t\t\tHttpContext.Session.SetString(\"" + TableNameWithoutTrailS + "ID\", Id);");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t" + TableName.Trim().ToCamelCase() + "ViewModel " + TableName.Trim() + "ViewModel = new " + TableName.Trim().ToCamelCase() + "ViewModel();");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel." + TableName.Trim() + "Model = _i" + TableName.Trim().ToCamelCase() + "ApiService.Get" + TableName.Trim().ToCamelCase() + "(model);");

            sb.AppendLine("\t\t\tif (" + TableName.Trim() + "ViewModel." + TableName.Trim() + "Model == null)");
            sb.AppendLine("\t\t\treturn RedirectToAction(\"Index\", \"NotFound\");");

            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.UniversesList = _iUniversesApiService.GetAllUniverses(model.user_id);");
            sb.AppendLine("\t\t\treturn View(" + TableName.Trim() + "ViewModel);");
            viewAction.MethodBody = sb.ToString();
            methods.Add(viewAction);

            #endregion

            #region Preview
            ClassMethodModel previewAction = new ClassMethodModel("public", "IActionResult", "", "Preview" + TableName.Trim().ToCamelCase());

            sb = new StringBuilder();

            previewAction.Attributes.Add("[HttpGet]");
            previewAction.Attributes.Add("[Route(\"Preview/{Id}\")]");

            previewAction.Parameters = new List<ClassProperty>();
            previewAction.Parameters.Add(new ClassProperty("Id", "string"));


            sb.AppendLine("\t\t\t" + TableName.Trim().ToCamelCase() + "Model model = new " + TableName.Trim().ToCamelCase() + "Model();");
            sb.AppendLine("\t\t\tmodel.user_id = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == \"UserID\")?.Value);");
            sb.AppendLine("\t\t\tmodel.id = Convert.ToInt32(Id);");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\tTempData[\"" + TableNameWithoutTrailS + "ID\"] = Id;");
            sb.AppendLine("\t\t\tViewData[\"" + TableNameWithoutTrailS + "ID\"] = Id;");
            sb.AppendLine("\t\t\tHttpContext.Session.SetString(\"" + TableNameWithoutTrailS + "ID\", Id);");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t" + TableName.Trim().ToCamelCase() + "ViewModel " + TableName.Trim() + "ViewModel = new " + TableName.Trim().ToCamelCase() + "ViewModel();");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel." + TableName.Trim() + "Model = _i" + TableName.Trim().ToCamelCase() + "ApiService.Get" + TableName.Trim().ToCamelCase() + "(model);");

            sb.AppendLine("\t\t\tif (" + TableName.Trim() + "ViewModel." + TableName.Trim() + "Model == null)");
            sb.AppendLine("\t\t\treturn RedirectToAction(\"Index\", \"NotFound\");");

            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.UniversesList = _iUniversesApiService.GetAllUniverses(model.user_id);");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\tTransformData(" + TableName.Trim() + "ViewModel." + TableName.Trim() + "Model);");
            sb.AppendLine("\t\t\treturn View(" + TableName.Trim() + "ViewModel);");
            previewAction.MethodBody = sb.ToString();
            methods.Add(previewAction);

            #endregion

            #region Delete
            ClassMethodModel deleteAction = new ClassMethodModel("public", "IActionResult", "", "Delete" + TableName.Trim().ToCamelCase());

            sb = new StringBuilder();

            deleteAction.Attributes.Add("[Route(\"Delete/{Id}\")]");

            deleteAction.Parameters = new List<ClassProperty>();
            deleteAction.Parameters.Add(new ClassProperty("Id", "string"));

            sb.AppendLine("\t\t\t" + TableName.Trim().ToCamelCase() + "Model model = new " + TableName.Trim().ToCamelCase() + "Model();");
            sb.AppendLine("\t\t\tmodel.user_id = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == \"UserID\")?.Value);");
            sb.AppendLine("\t\t\tmodel.id = Convert.ToInt32(Id);");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\tvar result = _i" + TableName.Trim().ToCamelCase() + "ApiService.Delete" + TableName.Trim().ToCamelCase() + "(model);");
            sb.AppendLine("\t\t\treturn RedirectToAction(\"Index\");");

            deleteAction.MethodBody = sb.ToString();
            methods.Add(deleteAction);
            #endregion

            #region TransformData
            ClassMethodModel transformDataAction = new ClassMethodModel("public", "void", "", "TransformData");

            sb = new StringBuilder();

            transformDataAction.Parameters = new List<ClassProperty>();
            transformDataAction.Parameters.Add(new ClassProperty("model", TableName.Trim().ToCamelCase() + "Model"));

            sb.AppendLine("\t\t\tif (model != null)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\t");
            foreach (var col in ColumnList)
            {
                if (col.PropName.NotIn(blockColumnList))
                {
                    if (col.PropType == "String" && col.PropName.ToLower() != "name")
                    {
                        sb.AppendLine("\t\t\t\tmodel." + col.PropName + "  = model." + col.PropName + " == null ? model." + col.PropName + " : model." + col.PropName + ".Replace(\"[MYWORLD]\", Helpers.Utility.CurrentDomain);");
                    }
                }
            }
            sb.AppendLine("\t\t\t}");

            transformDataAction.MethodBody = sb.ToString();
            methods.Add(transformDataAction);

            #endregion

            #region SaveProperties
            int columnCount = 0;

            var finalColumnList = from c in ColumnList
                                  where c.PropName.NotIn(blockColumnList)
                                  select c;

            foreach (var col in finalColumnList)
            {
                ClassMethodModel method = new ClassMethodModel("public", "IActionResult", "", "Save" + col.PropName);

                if (columnCount++ == 0)
                    method.LeadingLine = "#region Save Properties Methods";
                if (columnCount == finalColumnList.Count())
                    method.TrailingLine = "#endregion ";

                sb = new StringBuilder();

                method.Attributes.Add("[HttpPost]");

                sb.AppendLine("\t\t\tstring _rawContent = null;");
                sb.AppendLine("\t\t\t_rawContent = GetRawContent(_rawContent);");
                sb.AppendLine("\t\t\tResponseModel<string> response = new ResponseModel<string>();");

                sb.AppendLine("\t\t\tvar type = \"" + col.PropName + "\";");

                if (col.PropType == "String" && col.PropName.ToLower() != "name")
                {
                    method.Parameters = new List<ClassProperty>();
                    ClassProperty propData = new ClassProperty("id", "string");
                    method.Parameters.Add(propData);

                    method.Attributes.Add("[Route(\"{id}/Save" + col.PropName + "\")]");
                    sb.AppendLine("\t\t\tvar " + TableNameWithoutTrailS + "ID = Convert.ToInt64(id);");
                    sb.AppendLine("\t\t\tvar value = Convert.ToString(_rawContent);");
                }
                else
                {
                    method.Attributes.Add("[Route(\"Save" + col.PropName + "\")]");
                    sb.AppendLine("\t\t\tdynamic obj = JsonConvert.DeserializeObject(_rawContent);");
                    sb.AppendLine("\t\t\tvar " + TableNameWithoutTrailS + "ID = Convert.ToInt64(obj[\"" + TableNameWithoutTrailS + "ID\"].Value);");
                    if (col.PropType == "Boolean")
                        sb.AppendLine("\t\t\tvar value = Convert.ToBoolean(obj[\"value\"].Value);");
                    else
                        sb.AppendLine("\t\t\tvar value = Convert.ToString(obj[\"value\"].Value);");

                }

                sb.AppendLine("\t\t\t");
                sb.AppendLine("\t\t\t" + TableName.Trim().ToCamelCase() + "Model model = new " + TableName.Trim().ToCamelCase() + "Model();");
                sb.AppendLine("\t\t\tmodel.id = " + TableNameWithoutTrailS + "ID;");
                sb.AppendLine("\t\t\tmodel._id = " + TableNameWithoutTrailS + "ID;");
                sb.AppendLine("\t\t\tmodel.column_type = type;");

                if (col.PropType == "Boolean")
                    sb.AppendLine("\t\t\tmodel.column_value = Convert.ToInt32(value);");
                else
                    sb.AppendLine("\t\t\tmodel.column_value = value;");

                sb.AppendLine("\t\t\tresponse = _i" + TableName.Trim().ToCamelCase() + "ApiService.Save" + TableNameWithoutTrailS + "(model);");
                sb.AppendLine("\t\t\treturn Json(response);");

                method.MethodBody = sb.ToString();
                methods.Add(method);

            }
            #endregion

            return methods;
        }

        public ClassRoutine GenerateDALClass()
        {
            DALClassTemplate cSharpClass = new DALClassTemplate(TableName);
            cSharpClass.ClassName = (TableName).ToCamelCase() + "DAL";
            cSharpClass.InheritedClass = "BaseDAL";
            cSharpClass.ClassProperties = new ObservableCollection<ClassProperty>(ColumnList);
            cSharpClass.CSharpClassFileSettings.IsClassNameCamelCasing = false;
            cSharpClass.CSharpClassFileSettings.IsIncludeDefaultConstructor = true;
            cSharpClass.CSharpClassFileSettings.IsIncludeNameSpace = true;
            cSharpClass.CSharpClassFileSettings.IsIncludeUsings = true;
            cSharpClass.CSharpClassFileSettings.NameSpace = "My.World.Api.DataAccess";

            cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.Models;");
            cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using MySql.Data.MySqlClient;");
            cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Configuration;");
            cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Data;");

            cSharpClass.UpdateClassProperties();
            cSharpClass.TableName = TableName.ToCamelCase();
            cSharpClass.Constructors = CreateApiServiceConstructors((TableName).ToCamelCase() + "DAL");

            cSharpClass.ClassMethods = new List<ClassMethodModel>();
            cSharpClass.ClassMethods.Add(new DeleteMethodModel() { TableName = (TableName).ToCamelCase(), ClassProperties = cSharpClass.ClassProperties }.Initialize());
            cSharpClass.ClassMethods.Add(new SelectMethodModel() { TableName = (TableName).ToCamelCase(), ClassProperties = cSharpClass.ClassProperties }.Initialize());
            cSharpClass.ClassMethods.Add(new SelectAllForIDModel() { TableName = (TableName).ToCamelCase(), ClassProperties = cSharpClass.ClassProperties }.Initialize());
            cSharpClass.ClassMethods.Add(new InsertMethodModel() { TableName = (TableName).ToCamelCase(), ClassProperties = cSharpClass.ClassProperties }.Initialize());

            cSharpClass.ClassProperties = null;
            string classData = cSharpClass.GenerateCSharpClassData(false);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "DataAccess", (TableName).ToCamelCase() + "DAL" + ".cs"),
                classData, true);
            return this;
        }

        private List<ClassMethodModel> CreateApiServiceConstructors(string tableName, bool hasBody = true)
        {
            List<ClassMethodModel> Constructors = new List<ClassMethodModel>();
            ClassMethodModel method = new ClassMethodModel(hasBody ? "public" : "", "", "", tableName);
            method.Parameters = new List<ClassProperty>();
            ClassProperty propData = new ClassProperty("dbContext", "DBContext");
            method.Parameters.Add(propData);

            if (hasBody)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("\t\t\t			this.dbContext = dbContext;");
                method.MethodBody = sb.ToString();
            }
            Constructors.Add(method);

            return Constructors;
        }

        public ClassRoutine GenerateAPIServiceClass()
        {
            CSharpClass cSharpInterface = new CSharpClass(TableName.Trim() + "_Service");

            cSharpInterface.CSharpClassFileSettings.IsClassNameCamelCasing = true;
            cSharpInterface.CSharpClassFileSettings.IsIncludeNameSpace = true;
            cSharpInterface.CSharpClassFileSettings.IsIncludeUsings = true;
            cSharpInterface.CSharpClassFileSettings.NameSpace = "My.World.Api.Services";

            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings = new System.Collections.ObjectModel.ObservableCollection<string>();
            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Web;");
            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.Models;");

            cSharpInterface.CSharpClassFileSettings.IsInterface = true;

            cSharpInterface.ClassMethods = new List<ClassMethodModel>();

            cSharpInterface.ClassMethods.Add(new AddServiceMethodModel(true) { TableName = (TableName).ToCamelCase() }.Initialize());
            cSharpInterface.ClassMethods.Add(new GetServiceMethodModel(true) { TableName = (TableName).ToCamelCase() }.Initialize());
            cSharpInterface.ClassMethods.Add(new DeleteServiceMethodModel(true) { TableName = (TableName).ToCamelCase() }.Initialize());
            cSharpInterface.ClassMethods.Add(new GetAllServiceMethodModel(true) { TableName = (TableName).ToCamelCase() }.Initialize());
            cSharpInterface.ClassMethods.Add(new SaveServiceMethodModel(true) { TableName = (TableName).ToCamelCase() }.Initialize());

            string interfaceData = cSharpInterface.GenerateCSharpClassData(false);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "Services", "I" + cSharpInterface.ClassName + ".cs"),
                interfaceData, true);

            CSharpClass cSharpClass = new CSharpClass(TableName.Trim() + "_Service");
            cSharpClass.InheritedClass = "I" + cSharpInterface.ClassName;
            cSharpClass.CSharpClassFileSettings.IsIncludeDefaultConstructor = true;
            cSharpClass.CSharpClassFileSettings.IsClassNameCamelCasing = true;
            cSharpClass.CSharpClassFileSettings.IsIncludeNameSpace = true;
            cSharpClass.CSharpClassFileSettings.IsIncludeUsings = true;
            cSharpClass.CSharpClassFileSettings.NameSpace = "My.World.Api.Services";

            cSharpClass.CSharpClassFileSettings.UserDefinedUsings = new System.Collections.ObjectModel.ObservableCollection<string>();
            cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Web;");
            cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.Models;");
            cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.DataAccess;");

            cSharpClass.ClassMethods = new List<ClassMethodModel>();

            cSharpClass.ClassMethods.Add(new AddServiceMethodModel(false) { TableName = (TableName).ToCamelCase() }.Initialize());
            cSharpClass.ClassMethods.Add(new GetServiceMethodModel(false) { TableName = (TableName).ToCamelCase() }.Initialize());
            cSharpClass.ClassMethods.Add(new DeleteServiceMethodModel(false) { TableName = (TableName).ToCamelCase() }.Initialize());
            cSharpClass.ClassMethods.Add(new GetAllServiceMethodModel(false) { TableName = (TableName).ToCamelCase() }.Initialize());
            cSharpClass.ClassMethods.Add(new SaveServiceMethodModel(false) { TableName = (TableName).ToCamelCase() }.Initialize());

            cSharpClass.Constructors = CreateApiServiceConstructors(cSharpInterface.ClassName);
            cSharpClass.ClassProperties = new ObservableCollection<ClassProperty>();
            ClassProperty classProperty = new ClassProperty("dbContext", "DBContext");
            cSharpClass.ClassProperties.Add(classProperty);

            string classData = cSharpClass.GenerateCSharpClassData(false);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "Services", cSharpClass.ClassName + ".cs"),
                classData, true);

            return this;
        }

        public ClassRoutine GenerateAPIServiceClientClass()
        {
            CSharpClass cSharpInterface = new CSharpClass(TableName.Trim().ToCamelCase() + "_API_Service");

            cSharpInterface.CSharpClassFileSettings.IsClassNameCamelCasing = true;
            cSharpInterface.CSharpClassFileSettings.IsIncludeNameSpace = true;
            cSharpInterface.CSharpClassFileSettings.IsIncludeUsings = true;
            cSharpInterface.CSharpClassFileSettings.NameSpace = "My.World.Web.Services";

            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings = new System.Collections.ObjectModel.ObservableCollection<string>();
            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Web;");
            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.Models;");
            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Threading.Tasks;");

            cSharpInterface.CSharpClassFileSettings.IsInterface = true;
            cSharpInterface.ClassMethods = CreateApiServiceMethodsForClient(TableName.ToCamelCase(), new List<string>(new[] { "Get", "Delete", "GetAll", "Save" }), false);
            string interfaceData = cSharpInterface.GenerateCSharpClassData(false);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "APIServices", "I" + cSharpInterface.ClassName + ".cs"),
                interfaceData, true);

            APIServiceClientClass = new CSharpClass(TableName.Trim().ToCamelCase() + "_API_Service");
            APIServiceClientClass.InheritedClass = "BaseAPIService, I" + cSharpInterface.ClassName;
            APIServiceClientClass.CSharpClassFileSettings.IsClassNameCamelCasing = true;
            APIServiceClientClass.CSharpClassFileSettings.IsIncludeNameSpace = true;
            APIServiceClientClass.CSharpClassFileSettings.IsIncludeUsings = true;
            APIServiceClientClass.CSharpClassFileSettings.NameSpace = "My.World.Web.Services";

            APIServiceClientClass.CSharpClassFileSettings.UserDefinedUsings = new System.Collections.ObjectModel.ObservableCollection<string>();
            APIServiceClientClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Web;");
            APIServiceClientClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.Models;");
            APIServiceClientClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Threading.Tasks;");
            APIServiceClientClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Newtonsoft.Json;");

            APIServiceClientClass.ClassMethods = CreateApiServiceMethodsForClient(TableName.ToCamelCase(), new List<string>(new[] { "Add", "Get", "Delete", "GetAll", "Save" }));

            string classData = APIServiceClientClass.GenerateCSharpClassData(false);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "APIServices", APIServiceClientClass.ClassName + ".cs"),
                classData, true);
            return this;
        }

        private List<ClassMethodModel> CreateApiServiceMethodsForClient(string tableName, List<string> methodTypes, bool hasBody = true)
        {
            List<ClassMethodModel> Methods = new List<ClassMethodModel>();

            if (methodTypes == null || methodTypes.Count == 0)
                methodTypes = new List<string>(new[] { "Add", "Update", "Get", "Delete", "GetAll" });

            foreach (var mtype in methodTypes)
            {
                string ResponseModelType = "string";
                string methodType = "POST";

                if (mtype == "Get")
                    ResponseModelType = tableName + "Model";

                if (mtype == "GetAll")
                    ResponseModelType = "List<" + tableName + "Model>";

                if (mtype == "Save")
                    ResponseModelType = "string";

                if (mtype == "GetAll")
                    methodType = "GET";
                else
                    methodType = "POST";

                ClassMethodModel method = new ClassMethodModel(hasBody ? "public" : "",
                    (mtype == "Save" ? "ResponseModel<" + ResponseModelType + ">" : ResponseModelType),
                    "", mtype + (mtype == "Save" ? TableNameWithoutTrailS : tableName));

                method.Parameters = new List<ClassProperty>();

                if (!mtype.Contains("GetAll"))
                {
                    ClassProperty propData = new ClassProperty("model", tableName + "Model");
                    method.Parameters.Add(propData);
                }
                else
                {
                    ClassProperty propDataUser = new ClassProperty("UserId", "long");
                    method.Parameters.Add(propDataUser);
                }

                if (hasBody)
                {
                    StringBuilder sb = new StringBuilder();

                    if (mtype == "GetAll")
                        sb.AppendLine("\t\t\t			" + ResponseModelType + " " + tableName.ToLower() + "Model = new " + ResponseModelType + "();");
                    else
                        sb.AppendLine("\t\t\t			" + ResponseModelType + " " + tableName.ToLower() + "Model = null;");

                    sb.AppendLine("\t\t\t			RestHttpClient client = new RestHttpClient();");
                    sb.AppendLine("\t\t\t			client.Host = MyWorldApiUrl;");

                    if (mtype == "GetAll")
                        sb.AppendLine("\t\t\t			client.ApiUrl = \"" + mtype + tableName + "/\" + UserId;");
                    else if (mtype == "Save")
                        sb.AppendLine("\t\t\t			client.ApiUrl = \"" + mtype + TableNameWithoutTrailS + "\";");
                    else
                        sb.AppendLine("\t\t\t			client.ApiUrl = \"" + mtype + tableName + "\";");

                    sb.AppendLine("\t\t\t			client.ServiceMethod = Method." + methodType + ";");

                    if (mtype != "GetAll")
                        sb.AppendLine("\t\t\t			client.RequestBody = model;");

                    if (mtype == "GetAll")
                        sb.AppendLine("\t\t\t			string jsonResult = client.GETAsync();");
                    else
                        sb.AppendLine("\t\t\t			string jsonResult = client.GetResponseAsync();");

                    sb.AppendLine("\t\t\t			ResponseModel<" + ResponseModelType + "> response = JsonConvert.DeserializeObject<ResponseModel<" + ResponseModelType + ">>(jsonResult);");

                    if (mtype != "Save")
                    {
                        sb.AppendLine("\t\t\t			" + tableName.ToLower() + "Model = response.Value;");
                        sb.AppendLine("\t\t\t			return " + tableName.ToLower() + "Model;");
                    }
                    else
                    {
                        sb.AppendLine("\t\t\t			return response;");
                    }

                    method.MethodBody = sb.ToString();
                }
                Methods.Add(method);
            }
            return Methods;
        }

        public ClassRoutine GenerateCSHtmls()
        {
            string IndexTemplate = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "IndexTemplate.txt");
            string ViewTemplate = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "ViewTemplate.txt");
            string PreviewTemplate = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "PreviewTemplate.txt");

            var indexTemplateText = File.ReadAllText(IndexTemplate);

            indexTemplateText = indexTemplateText.Replace("[TABLE_NAME]", TableName.Trim().ToCamelCase());
            indexTemplateText = indexTemplateText.Replace("[TABLE_NAME_S]", TableNameWithoutTrailS);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "Views", TableName.Trim().ToCamelCase(), "Index.cshtml"), indexTemplateText, true);

            var viewTemplateText = File.ReadAllText(ViewTemplate);
            var previewTemplateText = File.ReadAllText(PreviewTemplate);

            StringBuilder sb = new StringBuilder();
            StringBuilder sbPreview = new StringBuilder();

            List<string> blockColumnList = new List<string>() { "id", "created_at", "updated_at", "user_id", "archived_at", "deleted_at" };

            foreach (var col in ColumnList)
            {
                if (col.PropName.NotIn(blockColumnList))
                {
                    sb.AppendLine("\t\t\t\t\t\t\t\t\t<div class=\"form-group row\">");
                    sb.AppendLine("\t\t\t\t\t\t\t\t\t	<label asp-for=\"@Model." + TableName.Trim() + "Model." + col.PropName + "\" class=\"col-md-3 col-form-label\" for=\"text-input\"></label>");
                    sb.AppendLine("\t\t\t\t\t\t\t\t\t	<div class=\"col-md-9\">");

                    sbPreview.AppendLine("\t\t\t\t\t\t\t\t\t<div class=\"form-group row\">");
                    sbPreview.AppendLine("\t\t\t\t\t\t\t\t\t	<label asp-for=\"@Model." + TableName.Trim() + "Model." + col.PropName + "\" class=\"col-md-3 col-form-label\" for=\"text-input\"></label>");
                    sbPreview.AppendLine("\t\t\t\t\t\t\t\t\t	<div class=\"col-md-9\">");

                    if (col.PropType == "String" && col.PropName.ToLower() != "name")
                    {
                        sb.AppendLine("\t\t\t\t\t\t\t\t\t		<div id=\"" + col.PropName + "\" class=\"form-control editorNative\"></div>");
                        sbPreview.AppendLine("\t\t\t\t\t\t\t\t\t		<div id=\"" + col.PropName + "\" class=\"editorNative\"></div>");
                    }
                    else if (col.PropName.ToLower() == "universe")
                    {
                        sb.AppendLine("\t\t\t\t\t\t\t\t\t		<select id=\"ddlUniverseList\" class=\"form-control\" style=\"min-width:150px\"");
                        sb.AppendLine("\t\t\t\t\t\t\t\t\t		asp-items=\"@(new SelectList(@Model.UniversesList, \"id\", \"name\", Model." + TableName.Trim() + "Model.Universe))\"");
                        sb.AppendLine("\t\t\t\t\t\t\t\t\t		onchange=\"UniverseSelected();\">");
                        sb.AppendLine("\t\t\t\t\t\t\t\t\t		</select>");

                        sbPreview.AppendLine("\t\t\t\t\t\t\t\t\t		<select id=\"ddlUniverseList\" class=\"form-control\" style=\"min-width:150px\" disabled=\"true\"");
                        sbPreview.AppendLine("\t\t\t\t\t\t\t\t\t		asp-items=\"@(new SelectList(@Model.UniversesList, \"id\", \"name\", Model." + TableName.Trim() + "Model.Universe))\">");
                        sbPreview.AppendLine("\t\t\t\t\t\t\t\t\t		</select>");
                    }
                    else
                    {
                        sb.AppendLine("\t\t\t\t\t\t\t\t\t		<input asp-for=\"@Model." + TableName.Trim() + "Model." + col.PropName + "\" class=\"form-control\"/>");

                        sbPreview.AppendLine("\t\t\t\t\t\t\t\t\t		<input style=\"border: 0;\" asp-for=\"@Model." + TableName.Trim() + "Model." + col.PropName + "\" readonly/>");
                    }

                    sb.AppendLine("\t\t\t\t\t\t\t\t\t		<span asp-validation-for=\"@Model." + TableName.Trim() + "Model." + col.PropName + "\" class=\"text-danger\"></span>");
                    sb.AppendLine("\t\t\t\t\t\t\t\t\t	</div>");
                    sb.AppendLine("\t\t\t\t\t\t\t\t\t</div>");
                    sb.AppendLine("\t\t\t");

                    sbPreview.AppendLine("\t\t\t\t\t\t\t\t\t	</div>");
                    sbPreview.AppendLine("\t\t\t\t\t\t\t\t\t</div>");
                    sbPreview.AppendLine("\t\t\t");

                }
            }

            StringBuilder sbJS = new StringBuilder();
            StringBuilder sbFunJS = new StringBuilder();

            StringBuilder sbJSPreview = new StringBuilder();
            StringBuilder sbFunJSPreview = new StringBuilder();

            foreach (var col in ColumnList)
            {
                if (col.PropType == "String" && col.PropName.ToLower() != "name")
                {
                    sbFunJS.AppendLine("\t\t\tfunction set" + col.PropName + "Body() {");
                    sbFunJS.AppendLine("\t\t\t    " + col.PropName + "_editor.clipboard.dangerouslyPasteHTML(htmlDecode('@Model." + TableName.Trim() + "Model." + col.PropName + "'));");
                    sbFunJS.AppendLine("\t\t\t    $('#" + col.PropName + "').focusout(function () {");
                    sbFunJS.AppendLine("\t\t\t        postSaveEditorData('" + col.PropName + "', " + col.PropName + "_editor.root.innerHTML);");
                    sbFunJS.AppendLine("\t\t\t    });");
                    sbFunJS.AppendLine("\t\t\t};");
                    sbFunJS.AppendLine("");

                    sbFunJSPreview.AppendLine("\t\t\tfunction set" + col.PropName + "Body() {");
                    sbFunJSPreview.AppendLine("\t\t\t    html = htmlDecode('@Model." + TableName.Trim() + "Model." + col.PropName + "');");
                    sbFunJSPreview.AppendLine("\t\t\t    $('#" + col.PropName + "').append(html);");
                    sbFunJSPreview.AppendLine("\t\t\t};");
                    sbFunJSPreview.AppendLine("");
                }
            }

            var finalColumnList = from c in ColumnList
                                  where c.PropName.NotIn(blockColumnList)
                                  select c;
            foreach (var col in finalColumnList)
            {
                if (col.PropName.ToLower() == "universe")
                {
                    //sbJS.AppendLine("\t\t\tloadUniverseList(@Model." + TableName.Trim() + "Model" + ".Universe);");
                    continue;
                }

                if (col.PropType != "String" || col.PropName.ToLower() == "name")
                {
                    sbJS.AppendLine("\t\t\t$('#" + TableName.Trim() + "Model_" + col.PropName + "').blur(function() {");
                    sbJS.AppendLine("\t\t\t    postSaveData('" + col.PropName + "', $('#" + TableName.Trim() + "Model_" + col.PropName + "').val());");
                    sbJS.AppendLine("\t\t\t});");
                    sbJS.AppendLine("");

                }
                else
                {
                    sbJSPreview.AppendLine("\t\t\tset" + col.PropName + "Body();");
                }
            }

            viewTemplateText = viewTemplateText.Replace("[TABLE_NAME]", TableName.Trim().ToCamelCase());
            viewTemplateText = viewTemplateText.Replace("[TABLE_NAME_S]", TableNameWithoutTrailS);
            viewTemplateText = viewTemplateText.Replace("[PROP_FIELDS]", sb.ToString());
            viewTemplateText = viewTemplateText.Replace("[PROP_CALL_JS]", sbJS.ToString());
            viewTemplateText = viewTemplateText.Replace("[PROP_FUNC_JS]", sbFunJS.ToString());

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "Views", TableName.Trim().ToCamelCase(), "View" + TableName.Trim().ToCamelCase() + ".cshtml"), viewTemplateText, true);

            previewTemplateText = previewTemplateText.Replace("[TABLE_NAME]", TableName.Trim().ToCamelCase());
            previewTemplateText = previewTemplateText.Replace("[TABLE_NAME_S]", TableNameWithoutTrailS);
            previewTemplateText = previewTemplateText.Replace("[PROP_FIELDS]", sbPreview.ToString());
            previewTemplateText = previewTemplateText.Replace("[PROP_CALL_JS]", sbJSPreview.ToString());
            previewTemplateText = previewTemplateText.Replace("[PROP_FUNC_JS]", sbFunJSPreview.ToString());

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "Views", TableName.Trim().ToCamelCase(), "Preview" + TableName.Trim().ToCamelCase() + ".cshtml"), previewTemplateText, true);
            return this;
        }

        public ClassRoutine GenerateJSFiles()
        {
            string JSTemplate = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "JSTemplate.js");

            var JSTemplateText = File.ReadAllText(JSTemplate);
            StringBuilder sbFieldEditor = new StringBuilder();
            StringBuilder sbInitEditor = new StringBuilder();
            StringBuilder sbEditorBody = new StringBuilder();

            foreach (var col in ColumnList)
            {
                if (col.PropType == "String" && col.PropName.ToLower() != "name")
                {
                    sbFieldEditor.AppendLine("var " + col.PropName + "_editor;");
                    sbInitEditor.AppendLine("\t" + col.PropName + "_editor = createEditor(\"#" + col.PropName + "\");");
                    sbEditorBody.AppendLine("\tset" + col.PropName + "Body();");
                }
            }
            JSTemplateText = JSTemplateText.Replace("[FIELD_EDITOR]", sbFieldEditor.ToString());
            JSTemplateText = JSTemplateText.Replace("[INITIALISE_EDITOR]", sbInitEditor.ToString());
            JSTemplateText = JSTemplateText.Replace("[SET_BODY_EDITOR]", sbEditorBody.ToString());

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "js", TableName.Trim().ToCamelCase() + "Editor.js"), JSTemplateText, true);
            return this;
        }

        public ClassRoutine GenerateViewModelFiles()
        {
            CSharpClass viewModelClientClass = new CSharpClass(TableName.Trim().ToCamelCase() + "_View_Model");
            viewModelClientClass.CSharpClassFileSettings.IsClassNameCamelCasing = true;
            viewModelClientClass.CSharpClassFileSettings.IsIncludeNameSpace = true;
            viewModelClientClass.CSharpClassFileSettings.IsIncludeUsings = true;
            viewModelClientClass.CSharpClassFileSettings.NameSpace = "My.World.Web.ViewModel";

            viewModelClientClass.CSharpClassFileSettings.UserDefinedUsings = new System.Collections.ObjectModel.ObservableCollection<string>();
            viewModelClientClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Web;");
            viewModelClientClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.Models;");
            viewModelClientClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Threading.Tasks;");

            viewModelClientClass.ClassProperties = new ObservableCollection<ClassProperty>();
            ClassProperty modelProperty = new ClassProperty(TableName.Trim() + "Model", TableName.Trim().ToCamelCase() + "Model");
            viewModelClientClass.ClassProperties.Add(modelProperty);

            ClassProperty universeListProperty = new ClassProperty("UniversesList", "List<UniversesModel>");
            viewModelClientClass.ClassProperties.Add(universeListProperty);

            viewModelClientClass.IsJsonProperty = true;
            viewModelClientClass.UpdateClassProperties();

            string classData = viewModelClientClass.GenerateCSharpClassData(false);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "ViewModel", viewModelClientClass.ClassName + ".cs"),
                classData, true);
            return this;
        }
    }
}
