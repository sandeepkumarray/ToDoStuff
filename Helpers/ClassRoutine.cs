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

        List<string> blockColumnList = new List<string>() { "id", "created_at", "updated_at", "user_id", "archived_at", "deleted_at" };

        string TableNameWithoutTrailS { get; set; }

        public ClassRoutine(string tableName, List<ClassProperty> columnList)
        {
            TableName = tableName;
            ColumnList = columnList;
            var hasTrailingS = TableName.Trim().Substring(TableName.Trim().Length - 1, 1).ToLower() == "s" ? true : false;
            if (hasTrailingS)
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
                ControllerClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Microsoft.Extensions.Configuration;");
                ControllerClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Web;");

                ControllerClass.CSharpClassFileSettings.IsIncludeParametrizedConstructor = true;
                ControllerClass.CSharpClassFileSettings.Parameters = new ObservableCollectionFast<ClassProperty>();
                ControllerClass.CSharpClassFileSettings.Parameters.Add(new ClassProperty("i" + TableName.Trim().ToCamelCase() + "ApiService", "I" + TableName.Trim().ToCamelCase() + "ApiService") { });
                if (TableName.Trim() != "universes")
                    ControllerClass.CSharpClassFileSettings.Parameters.Add(new ClassProperty("iUniversesApiService", "IUniversesApiService") { });

                ControllerClass.CSharpClassFileSettings.Parameters.Add(new ClassProperty("iUsersApiService", "IUsersApiService") { });
                ControllerClass.CSharpClassFileSettings.Parameters.Add(new ClassProperty("iContenttypesApiService", "IContentTypesApiService") { });
                ControllerClass.CSharpClassFileSettings.Parameters.Add(new ClassProperty("iObjectBucketApiService", "IObjectBucketApiService") { });
                ControllerClass.CSharpClassFileSettings.Parameters.Add(new ClassProperty("config", "IConfiguration") { });

                StringBuilder sbContructorContent = new StringBuilder();
                sbContructorContent.AppendLine("_i" + TableName.Trim().ToCamelCase() + "ApiService = i" + TableName.Trim().ToCamelCase() + "ApiService;");
                if (TableName.Trim() != "universes")
                    sbContructorContent.AppendLine("\t\t\t_iUniversesApiService = iUniversesApiService;");
                sbContructorContent.AppendLine("\t\t\t_iUsersApiService = iUsersApiService;");
                sbContructorContent.AppendLine("\t\t\t_iContenttypesApiService = iContenttypesApiService;");
                sbContructorContent.AppendLine("\t\t\t_iObjectBucketApiService = iObjectBucketApiService;");
                sbContructorContent.AppendLine("\t\t\t_config = config;");
                ControllerClass.CSharpClassFileSettings.ParameterizedConstructorContent = sbContructorContent.ToString();

                ControllerClass.ClassMethods = CreateControllerMethods();
                ControllerClass.ClassProperties = new ObservableCollectionFast<ClassProperty>();

                ControllerClass.ClassProperties.Add(new ClassProperty("_i" + TableName.Trim().ToCamelCase() + "ApiService", "readonly I" + TableName.Trim().ToCamelCase() + "ApiService"));
                if (TableName.Trim() != "universes")
                    ControllerClass.ClassProperties.Add(new ClassProperty("_iUniversesApiService", "readonly IUniversesApiService"));
                ControllerClass.ClassProperties.Add(new ClassProperty("_iUsersApiService", "readonly IUsersApiService"));
                ControllerClass.ClassProperties.Add(new ClassProperty("_iContenttypesApiService", "readonly IContentTypesApiService"));
                ControllerClass.ClassProperties.Add(new ClassProperty("_iObjectBucketApiService", "readonly IObjectBucketApiService"));
                ControllerClass.ClassProperties.Add(new ClassProperty("_config", "readonly IConfiguration"));

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

            //indexAction.Attributes.Add("[Route(\"Index\")]");

            sb = new StringBuilder();
            sb.AppendLine("\t\t\tvar accountID = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == \"UserID\")?.Value);");

            sb.AppendLine("\t\t\t_iObjectBucketApiService.SetObjectStorageSecrets(accountID);");
            sb.AppendLine("\t\t\tstring imageFormat = HttpContext.Session.GetString(\"ContentImageUrlFormat\");");

            sb.AppendLine("\t\t\tvar " + TableName.Trim() + " = _i" + TableName.Trim().ToCamelCase() + "ApiService.GetAll" + TableName.Trim().ToCamelCase() + "(accountID);");

            sb.AppendLine("\t\t\t" + TableName.Trim() + ".ForEach(b =>");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t    if (!string.IsNullOrEmpty(b.object_name))");
            sb.AppendLine("\t\t\t    {");
            sb.AppendLine("\t\t\t        b.image_url = imageFormat");
            sb.AppendLine("\t\t\t        .Replace(\"{bucketName}\", _iObjectBucketApiService.objectStorageKeysModel.bucketName)");
            sb.AppendLine("\t\t\t        .Replace(\"{objectName}\", b.object_name);");
            sb.AppendLine("\t\t\t    }");
            sb.AppendLine("\t\t\t    else");
            sb.AppendLine("\t\t\t    {");
            sb.AppendLine("\t\t\t        b.image_url = imageFormat");
            sb.AppendLine("\t\t\t          .Replace(\"{bucketName}\", \"my-world-main\")");
            sb.AppendLine("\t\t\t          .Replace(\"{objectName}\", \"cards/" + TableName.Trim().ToCamelCase() + ".png\");");
            sb.AppendLine("\t\t\t    }");
            sb.AppendLine("\t\t\t});");

            sb.AppendLine("\t\t\treturn View(" + TableName.Trim() + ");");
            indexAction.MethodBody = sb.ToString();
            methods.Add(indexAction);
            #endregion

            #region View
            ClassMethodModel viewAction = new ClassMethodModel("public", "IActionResult", "", "View" + TableName.Trim().ToCamelCase());

            sb = new StringBuilder();

            viewAction.Attributes.Add("[HttpGet]");
            viewAction.Attributes.Add("[Route(\"{Id}/edit\")]");

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
            sb.AppendLine("\t\t\t" + TableName.Trim().ToCamelCase() + "ViewModel " + TableName.Trim() + "ViewModel = new " + TableName.Trim().ToCamelCase() + "ViewModel(_iObjectBucketApiService);");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel." + TableName.Trim() + "Model = _i" + TableName.Trim().ToCamelCase() + "ApiService.Get" + TableName.Trim().ToCamelCase() + "(model);");

            sb.AppendLine("\t\t\tif (" + TableName.Trim() + "ViewModel." + TableName.Trim() + "Model == null)");
            sb.AppendLine("\t\t\t\treturn RedirectToAction(\"Index\", \"NotFound\");");

            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.UniversesList = _iUniversesApiService.GetAllUniverses(model.user_id);");

            sb.AppendLine("\t\t\tvar contentTemplate = _iUsersApiService.GetUsersContentTemplate(new UsersModel() { id = model.user_id });");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.ContentTemplate = contentTemplate.Contents.Find(c => c.content_type == \"" + TableName.Trim() + "\");");
            sb.AppendLine("\t\t\tContentTypesModel contentTypesModel = _iContenttypesApiService.GetContentTypes(new ContentTypesModel() { name = \"" + TableName.Trim().ToCamelCase() + "\" });");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.headerBackgroundColor = contentTypesModel.primary_color;");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.headerBackgroundColor = contentTypesModel.sec_color;");
            sb.AppendLine("\t\t\t_iObjectBucketApiService.SetObjectStorageSecrets(model.user_id);");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.ContentObjectModelList = _iObjectBucketApiService.GetAllContentObjectAttachments(Convert.ToInt64(Id), \"" + TableName.Trim() + "\");");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.ContentObjectModelList.ForEach(o => ");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t    var publicUrl = \"http://\" + _iObjectBucketApiService.objectStorageKeysModel.endpoint");
            sb.AppendLine("\t\t\t                + '/' + _iObjectBucketApiService.objectStorageKeysModel.bucketName + '/' + _config.GetValue<string>(\"BucketEnv\") + '/' + o.object_name; ");
            sb.AppendLine("\t\t\t    o.file_url = HttpUtility.UrlPathEncode(publicUrl); ");
            sb.AppendLine("\t\t\t}); ");
            sb.AppendLine("\t\t\tvar existing_total_size = " + TableName.Trim() + "ViewModel.ContentObjectModelList.Sum(f => f.object_size);");
            sb.AppendLine("\t\t\tvar AllowedTotalContentSize = Convert.ToInt64(HttpContext.Session.GetString(\"AllowedTotalContentSize\"));");
            sb.AppendLine("\t\t\tvar remainingSize = AllowedTotalContentSize - existing_total_size;");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.RemainingContentSize = Helpers.Utility.SizeSuffix(remainingSize);");
            sb.AppendLine("\t\t\treturn View(" + TableName.Trim() + "ViewModel); ");

            viewAction.MethodBody = sb.ToString();
            methods.Add(viewAction);

            #endregion

            #region Preview
            ClassMethodModel previewAction = new ClassMethodModel("public", "IActionResult", "", "Preview" + TableName.Trim().ToCamelCase());

            sb = new StringBuilder();

            previewAction.Attributes.Add("[HttpGet]");
            previewAction.Attributes.Add("[Route(\"{Id}\")]");

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
            sb.AppendLine("\t\t\t" + TableName.Trim().ToCamelCase() + "ViewModel " + TableName.Trim() + "ViewModel = new " + TableName.Trim().ToCamelCase() + "ViewModel(_iObjectBucketApiService);");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel." + TableName.Trim() + "Model = _i" + TableName.Trim().ToCamelCase() + "ApiService.Get" + TableName.Trim().ToCamelCase() + "(model);");

            sb.AppendLine("\t\t\tif (" + TableName.Trim() + "ViewModel." + TableName.Trim() + "Model == null)");
            sb.AppendLine("\t\t\treturn RedirectToAction(\"Index\", \"NotFound\");");

            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.UniversesList = _iUniversesApiService.GetAllUniverses(model.user_id);");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\tTransformData(" + TableName.Trim() + "ViewModel." + TableName.Trim() + "Model);");

            sb.AppendLine("\t\t\tvar contentTemplate = _iUsersApiService.GetUsersContentTemplate(new UsersModel() { id = model.user_id });");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.ContentTemplate = contentTemplate.Contents.Find(c => c.content_type == \"" + TableName.Trim() + "\");");
            sb.AppendLine("\t\t\tContentTypesModel contentTypesModel = _iContenttypesApiService.GetContentTypes(new ContentTypesModel() { name = \"" + TableName.Trim().ToCamelCase() + "\" });");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.headerBackgroundColor = contentTypesModel.primary_color;");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.headerBackgroundColor = contentTypesModel.sec_color;");

            sb.AppendLine("\t\t\t_iObjectBucketApiService.SetObjectStorageSecrets(model.user_id);");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.ContentObjectModelList = _iObjectBucketApiService.GetAllContentObjectAttachments(Convert.ToInt64(Id), \"" + TableName.Trim() + "\");");
            sb.AppendLine("\t\t\t" + TableName.Trim() + "ViewModel.ContentObjectModelList.ForEach(o => ");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t    var publicUrl = \"http://\" + _iObjectBucketApiService.objectStorageKeysModel.endpoint");
            sb.AppendLine("\t\t\t                + '/' + _iObjectBucketApiService.objectStorageKeysModel.bucketName + '/' + _config.GetValue<string>(\"BucketEnv\") + '/' + o.object_name; ");
            sb.AppendLine("\t\t\t    o.file_url = HttpUtility.UrlPathEncode(publicUrl); ");
            sb.AppendLine("\t\t\t}); ");
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
            ClassMethodModel transformDataAction = new ClassMethodModel("private", "void", "", "TransformData");

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

            #region UploadAttachment

            ClassMethodModel uploadAttachment = new ClassMethodModel("public", "IActionResult", "", "UploadAttachment");

            sb = new StringBuilder();

            uploadAttachment.Attributes.Add("[HttpPost]");
            uploadAttachment.Attributes.Add("[Route(\"UploadAttachment\")]");

            uploadAttachment.Parameters = new List<ClassProperty>();
            uploadAttachment.Parameters.Add(new ClassProperty("files", "List<IFormFile>"));

            sb.AppendLine("\t\t\tvar accountID = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == \"UserID\")?.Value);");
            sb.AppendLine("\t\t\tstring content_Id = HttpContext.Session.GetString(\"" + TableNameWithoutTrailS + "ID\");");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\tvar ContentObjectModelList = _iObjectBucketApiService.GetAllContentObjectAttachments(Convert.ToInt64(content_Id), \"" + TableName.Trim() + "\");");
            sb.AppendLine("\t\t\tvar existing_total_size = ContentObjectModelList.Sum(f => f.object_size);");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\tvar rq_files = Request.Form.Files;");
            sb.AppendLine("\t\t\tvar upload_file_size = rq_files.Sum(f => f.Length);");
            sb.AppendLine("\t\t\tvar total_size = upload_file_size + existing_total_size;");
            sb.AppendLine("\t\t\tvar AllowedTotalContentSize = Convert.ToInt64(HttpContext.Session.GetString(\"AllowedTotalContentSize\"));");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\tif (total_size <= AllowedTotalContentSize)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t	if (rq_files != null)");
            sb.AppendLine("\t\t\t	{");
            sb.AppendLine("\t\t\t		foreach (var file in rq_files)");
            sb.AppendLine("\t\t\t		{");
            sb.AppendLine("\t\t\t			using (var ms = new MemoryStream())");
            sb.AppendLine("\t\t\t			{");
            sb.AppendLine("\t\t\t				ContentObjectModel model = new ContentObjectModel();");
            sb.AppendLine("\t\t\t				model.object_type = file.ContentType;");
            sb.AppendLine("\t\t\t				model.object_name = file.FileName;");
            sb.AppendLine("\t\t\t				model.object_size = file.Length;");
            sb.AppendLine("\t\t\t				model.bucket_folder = _config.GetValue<string>(\"BucketEnv\");");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t				file.CopyTo(ms);");
            sb.AppendLine("\t\t\t				model.file = ms;");
            sb.AppendLine("\t\t\t				model.file.Seek(0, 0);");
            sb.AppendLine("\t\t\t				_iObjectBucketApiService.SetObjectStorageSecrets(accountID);");
            sb.AppendLine("\t\t\t				var response = _iObjectBucketApiService.UploadObject(model).Result;");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t				if (!string.IsNullOrEmpty(response.Value))");
            sb.AppendLine("\t\t\t				{");
            sb.AppendLine("\t\t\t					ContentObjectAttachmentModel contentObjectAttachmentModel = new ContentObjectAttachmentModel();");
            sb.AppendLine("\t\t\t					contentObjectAttachmentModel.object_id = Convert.ToInt64(response.Value);");
            sb.AppendLine("\t\t\t					contentObjectAttachmentModel.content_id = Convert.ToInt64(content_Id);");
            sb.AppendLine("\t\t\t					contentObjectAttachmentModel.content_type = \"" + TableName.Trim() + "\";");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t					_iObjectBucketApiService.AddContentObjectAttachment(contentObjectAttachmentModel);");
            sb.AppendLine("\t\t\t				}");
            sb.AppendLine("\t\t\t			}");
            sb.AppendLine("\t\t\t		}");
            sb.AppendLine("\t\t\t	}");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\telse");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t	return BadRequest(new { message = \"You have Exceeded the maximum allowed size of 50 MB per content to upload images.\" });");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\treturn Ok();");


            uploadAttachment.MethodBody = sb.ToString();
            methods.Add(uploadAttachment);
            #endregion

            #region DeleteAttachment

            ClassMethodModel deleteAttachment = new ClassMethodModel("public", "IActionResult", "", "DeleteAttachment");

            sb = new StringBuilder();

            deleteAttachment.Attributes.Add("[Route(\"DeleteAttachment\")]");

            deleteAttachment.Parameters = new List<ClassProperty>();
            deleteAttachment.Parameters.Add(new ClassProperty("objectId", "long"));
            deleteAttachment.Parameters.Add(new ClassProperty("objectName", "string"));

            sb.AppendLine("\t\t\tvar accountID = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == \"UserID\")?.Value);");
            sb.AppendLine("\t\t\tstring content_Id = HttpContext.Session.GetString(\"" + TableNameWithoutTrailS + "ID\");");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\tContentObjectAttachmentModel contentObjectAttachmentModel = new ContentObjectAttachmentModel();");
            sb.AppendLine("\t\t\tcontentObjectAttachmentModel.object_id = objectId;");
            sb.AppendLine("\t\t\tcontentObjectAttachmentModel.content_id = Convert.ToInt64(content_Id);");
            sb.AppendLine("\t\t\tcontentObjectAttachmentModel.content_type = \"" + TableName.Trim() + "\";");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\tvar bucket_folder = _config.GetValue<string>(\"BucketEnv\");");
            sb.AppendLine("\t\t\tContentObjectModel contentObjectModel = new ContentObjectModel();");
            sb.AppendLine("\t\t\tcontentObjectModel.object_id = objectId;");
            sb.AppendLine("\t\t\tcontentObjectModel.object_name = bucket_folder + \" / \" + objectName;");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t_iObjectBucketApiService.SetObjectStorageSecrets(accountID);");
            sb.AppendLine("\t\t\t_iObjectBucketApiService.DeleteObject(contentObjectModel);");
            sb.AppendLine("\t\t\t_iObjectBucketApiService.DeleteContentObjectAttachment(contentObjectAttachmentModel);");
            sb.AppendLine("\t\t\t_iObjectBucketApiService.DeleteContentObject(contentObjectModel);");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\treturn RedirectToAction(\"View" + TableName.Trim().ToCamelCase() + "\", \"" + TableName.Trim() + "\", new { id = content_Id }, \"Gallery_panel\");");

            deleteAttachment.MethodBody = sb.ToString();
            methods.Add(deleteAttachment);
            #endregion

            return methods;
        }

        public ClassRoutine GenerateDALClass()
        {
            DALClassTemplate cSharpClass = new DALClassTemplate(TableName);
            cSharpClass.ClassName = (TableName).ToCamelCase() + "DAL";
            cSharpClass.InheritedClass = "BaseDAL";
            cSharpClass.ClassProperties = new ObservableCollectionFast<ClassProperty>(ColumnList);
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
            cSharpClass.Constructors = CreateApiServiceConstructors(TableName.ToCamelCase() + "DAL");

            cSharpClass.ClassMethods = new List<ClassMethodModel>();
            cSharpClass.ClassMethods.Add(new DeleteMethodModel() { TableName = TableName, ClassProperties = cSharpClass.ClassProperties }.Initialize());
            cSharpClass.ClassMethods.Add(new SelectMethodModel() { TableName = TableName, ClassProperties = cSharpClass.ClassProperties }.Initialize());
            cSharpClass.ClassMethods.Add(new SelectAllForIDModel() { TableName = TableName, ClassProperties = cSharpClass.ClassProperties }.Initialize());
            cSharpClass.ClassMethods.Add(new InsertMethodModel() { TableName = TableName, ClassProperties = cSharpClass.ClassProperties }.Initialize());
            cSharpClass.ClassMethods.Add(new UpdateMethodModel() { TableName = TableName, ClassProperties = cSharpClass.ClassProperties }.Initialize());

            cSharpClass.ClassProperties = null;
            string classData = cSharpClass.GenerateCSharpClassData(false);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "DataAccess", TableName.ToCamelCase() + "DAL" + ".cs"),
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

            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings = new ObservableCollectionFast<string>();
            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Web;");
            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.Models;");

            cSharpInterface.CSharpClassFileSettings.IsInterface = true;

            cSharpInterface.ClassMethods = new List<ClassMethodModel>();

            cSharpInterface.ClassMethods.Add(new AddServiceMethodModel(true) { TableName = TableName.ToCamelCase() }.Initialize());
            cSharpInterface.ClassMethods.Add(new GetServiceMethodModel(true) { TableName = TableName.ToCamelCase() }.Initialize());
            cSharpInterface.ClassMethods.Add(new DeleteServiceMethodModel(true) { TableName = TableName.ToCamelCase() }.Initialize());
            cSharpInterface.ClassMethods.Add(new GetAllServiceMethodModel(true) { TableName = TableName.ToCamelCase() }.Initialize());
            cSharpInterface.ClassMethods.Add(new SaveServiceMethodModel(true) { TableName = TableName.ToCamelCase() }.Initialize());
            cSharpInterface.ClassMethods.Add(new UpdateServiceMethodModel(true) { TableName = TableName.ToCamelCase() }.Initialize());

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

            cSharpClass.CSharpClassFileSettings.UserDefinedUsings = new ObservableCollectionFast<string>();
            cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Web;");
            cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.Models;");
            cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.DataAccess;");

            cSharpClass.ClassMethods = new List<ClassMethodModel>();

            cSharpClass.ClassMethods.Add(new AddServiceMethodModel(false) { TableName = TableName.ToCamelCase() }.Initialize());
            cSharpClass.ClassMethods.Add(new GetServiceMethodModel(false) { TableName = TableName.ToCamelCase() }.Initialize());
            cSharpClass.ClassMethods.Add(new DeleteServiceMethodModel(false) { TableName = TableName.ToCamelCase() }.Initialize());
            cSharpClass.ClassMethods.Add(new GetAllServiceMethodModel(false) { TableName = TableName.ToCamelCase() }.Initialize());
            cSharpClass.ClassMethods.Add(new SaveServiceMethodModel(false) { TableName = TableName.ToCamelCase() }.Initialize());
            cSharpClass.ClassMethods.Add(new UpdateServiceMethodModel(false) { TableName = TableName.ToCamelCase() }.Initialize());

            cSharpClass.Constructors = CreateApiServiceConstructors(cSharpInterface.ClassName);
            cSharpClass.ClassProperties = new ObservableCollectionFast<ClassProperty>();
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
            CSharpClass cSharpInterface = new CSharpClass(TableName.Trim() + "_API_Service");

            cSharpInterface.CSharpClassFileSettings.IsClassNameCamelCasing = true;
            cSharpInterface.CSharpClassFileSettings.IsIncludeNameSpace = true;
            cSharpInterface.CSharpClassFileSettings.IsIncludeUsings = true;
            cSharpInterface.CSharpClassFileSettings.NameSpace = "My.World.Web.Services";

            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings = new ObservableCollectionFast<string>();
            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Web;");
            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.Models;");
            cSharpInterface.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Threading.Tasks;");

            cSharpInterface.CSharpClassFileSettings.IsInterface = true;
            cSharpInterface.ClassMethods = CreateApiServiceMethodsForClient(TableName.ToCamelCase(), new List<string>(new[] { "Get", "Delete", "GetAll", "Save" }), false);
            string interfaceData = cSharpInterface.GenerateCSharpClassData(false);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "APIServices", "I" + cSharpInterface.ClassName + ".cs"),
                interfaceData, true);

            APIServiceClientClass = new CSharpClass(TableName.Trim() + "_API_Service");
            APIServiceClientClass.InheritedClass = "BaseAPIService, I" + cSharpInterface.ClassName;
            APIServiceClientClass.CSharpClassFileSettings.IsClassNameCamelCasing = true;
            APIServiceClientClass.CSharpClassFileSettings.IsIncludeNameSpace = true;
            APIServiceClientClass.CSharpClassFileSettings.IsIncludeUsings = true;
            APIServiceClientClass.CSharpClassFileSettings.NameSpace = "My.World.Web.Services";

            APIServiceClientClass.CSharpClassFileSettings.UserDefinedUsings = new ObservableCollectionFast<string>();
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
                    "", mtype + (mtype == "Save" ? (TableNameWithoutTrailS == null ? tableName : TableNameWithoutTrailS) : tableName));

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
                        sb.AppendLine("			" + ResponseModelType + " " + tableName.ToLower() + "Model = new " + ResponseModelType + "();");
                    else
                        sb.AppendLine("			" + ResponseModelType + " " + tableName.ToLower() + "Model = null;");

                    sb.AppendLine("			RestHttpClient client = new RestHttpClient();");
                    sb.AppendLine("			client.Host = MyWorldContentApiUrl;");

                    if (mtype == "GetAll")
                        sb.AppendLine("			client.ApiUrl = \"" + tableName + "/" + mtype + tableName + "/\" + UserId;");
                    else if (mtype == "Save")
                        sb.AppendLine("			client.ApiUrl = \"" + tableName + "/" + mtype + (TableNameWithoutTrailS == null ? tableName : TableNameWithoutTrailS) + "\";");
                    else
                        sb.AppendLine("			client.ApiUrl = \"" + tableName + "/" + mtype + tableName + "\";");

                    sb.AppendLine("			client.ServiceMethod = Method." + methodType + ";");

                    if (mtype != "GetAll")
                        sb.AppendLine("			client.RequestBody = model;");

                    if (mtype == "GetAll")
                        sb.AppendLine("			string jsonResult = client.GETAsync();");
                    else
                        sb.AppendLine("			string jsonResult = client.GetResponseAsync();");

                    sb.AppendLine("			ResponseModel<" + ResponseModelType + "> response = JsonConvert.DeserializeObject<ResponseModel<" + ResponseModelType + ">>(jsonResult);");

                    if (mtype != "Save")
                    {
                        sb.AppendLine("			" + tableName.ToLower() + "Model = response.Value;");
                        sb.AppendLine("			return " + tableName.ToLower() + "Model;");
                    }
                    else
                    {
                        sb.AppendLine("			return response;");
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
            string ViewTemplate = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "ViewTemplate.html");
            string PreviewTemplate = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "PreviewTemplate.html");

            var indexTemplateText = File.ReadAllText(IndexTemplate);

            indexTemplateText = indexTemplateText.Replace("[TABLE_NAME]", TableName.Trim().ToCamelCase());
            indexTemplateText = indexTemplateText.Replace("[TABLE_NAME_S]", TableNameWithoutTrailS);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "Views", TableName.Trim().ToCamelCase(), "Index.cshtml"), indexTemplateText, true);

            var viewTemplateText = File.ReadAllText(ViewTemplate);
            var previewTemplateText = File.ReadAllText(PreviewTemplate);

            StringBuilder sb = new StringBuilder();
            StringBuilder sbPreview = new StringBuilder();


            StringBuilder sbJS = new StringBuilder();
            StringBuilder sbFunJS = new StringBuilder();

            StringBuilder sbJSPreview = new StringBuilder();
            StringBuilder sbFunJSPreview = new StringBuilder();

            var finalColumnList = from c in ColumnList
                                  where c.PropName.NotIn(blockColumnList)
                                  select c;
            foreach (var col in finalColumnList)
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

                    sbFunJSPreview.AppendLine("\t\tfunction set" + col.PropName + "Body() {");
                    sbFunJSPreview.AppendLine("\t\t    html = htmlDecode('@Model." + TableName.Trim() + "Model." + col.PropName + "');");
                    sbFunJSPreview.AppendLine("\t\t    $('#" + col.PropName + "').append(html);");
                    sbFunJSPreview.AppendLine("\t\t};");
                    sbFunJSPreview.AppendLine("");
                }
                else
                {
                    sbFunJS.AppendLine("\t\t\tfunction set" + col.PropName + "Body() {");
                    sbFunJS.AppendLine("\t\t\t    $('#" + TableName.Trim() + "Model_" + col.PropName + "').val('@Model." + TableName.Trim() + "Model." + col.PropName + "');");
                    sbFunJS.AppendLine("\t\t\t};");
                    sbFunJS.AppendLine("");

                    sbFunJSPreview.AppendLine("\t\tfunction set" + col.PropName + "Body() {");
                    sbFunJSPreview.AppendLine("\t\t    $('#" + TableName.Trim() + "Model_" + col.PropName + "').val('@Model." + TableName.Trim() + "Model." + col.PropName + "');");
                    sbFunJSPreview.AppendLine("\t\t};");
                    sbFunJSPreview.AppendLine("");
                }
            }

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

                sbJSPreview.AppendLine("\t\t\tset" + col.PropName + "Body();");
            }

            viewTemplateText = viewTemplateText.Replace("[TABLE_NAME]", TableName.Trim().ToCamelCase());
            viewTemplateText = viewTemplateText.Replace("[TABLE_NAME_SMALL]", TableName.Trim());
            viewTemplateText = viewTemplateText.Replace("[TABLE_NAME_S]", TableNameWithoutTrailS);
            viewTemplateText = viewTemplateText.Replace("[PROP_FIELDS]", sb.ToString());
            viewTemplateText = viewTemplateText.Replace("[PROP_CALL_JS]", sbJS.ToString());
            viewTemplateText = viewTemplateText.Replace("[PROP_FUNC_JS]", sbFunJS.ToString());

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "Views", TableName.Trim().ToCamelCase(), "View" + TableName.Trim().ToCamelCase() + ".cshtml"), viewTemplateText, true);

            previewTemplateText = previewTemplateText.Replace("[TABLE_NAME]", TableName.Trim().ToCamelCase());
            previewTemplateText = previewTemplateText.Replace("[TABLE_NAME_SMALL]", TableName.Trim());
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

            var finalColumnList = from c in ColumnList
                                  where c.PropName.NotIn(blockColumnList)
                                  select c;
            foreach (var col in finalColumnList)
            {
                if (col.PropType == "String" && col.PropName.ToLower() != "name")
                {
                    sbFieldEditor.AppendLine("var " + col.PropName + "_editor;");
                    sbInitEditor.AppendLine("\t" + col.PropName + "_editor = createEditor(\"#" + col.PropName + "\");");
                }
                sbEditorBody.AppendLine("\tset" + col.PropName + "Body();");
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

            viewModelClientClass.CSharpClassFileSettings.UserDefinedUsings = new ObservableCollectionFast<string>();
            viewModelClientClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Web;");
            viewModelClientClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.Models;");
            viewModelClientClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Threading.Tasks;");
            viewModelClientClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Web.Services;");

            viewModelClientClass.CSharpClassFileSettings.IsIncludeParametrizedConstructor = true;
            viewModelClientClass.CSharpClassFileSettings.Parameters = new ObservableCollectionFast<ClassProperty>();
            viewModelClientClass.CSharpClassFileSettings.Parameters.Add(new ClassProperty("_iObjectBucketApiService", "IObjectBucketApiService") { });

            StringBuilder sbContructorContent = new StringBuilder();
            sbContructorContent.AppendLine("iObjectBucketApiService = _iObjectBucketApiService;");

            viewModelClientClass.CSharpClassFileSettings.ParameterizedConstructorContent = sbContructorContent.ToString();
            viewModelClientClass.CSharpClassFileSettings.IsIncludeDefaultConstructor = true;

            viewModelClientClass.ClassProperties = new ObservableCollectionFast<ClassProperty>();
            ClassProperty modelProperty = new ClassProperty(TableName.Trim() + "Model", TableName.Trim().ToCamelCase() + "Model");
            viewModelClientClass.ClassProperties.Add(modelProperty);

            ClassProperty iObjectBucketApiServiceProperty = new ClassProperty("iObjectBucketApiService", "IObjectBucketApiService", "private");
            viewModelClientClass.ClassProperties.Add(iObjectBucketApiServiceProperty);

            ClassProperty universeListProperty = new ClassProperty("UniversesList", "List<UniversesModel>");
            viewModelClientClass.ClassProperties.Add(universeListProperty);

            ClassProperty contentTemplateProperty = new ClassProperty("ContentTemplate", "Content");
            viewModelClientClass.ClassProperties.Add(contentTemplateProperty);

            ClassProperty headerBackgroundColorProperty = new ClassProperty("headerBackgroundColor", "string");
            viewModelClientClass.ClassProperties.Add(headerBackgroundColorProperty);

            ClassProperty headerForegroundColorProperty = new ClassProperty("headerForegroundColor", "string");
            viewModelClientClass.ClassProperties.Add(headerForegroundColorProperty);

            ClassProperty _contentObjectModelListProperty = new ClassProperty("_contentObjectModelList", "List<ContentObjectModel>", "private");
            viewModelClientClass.ClassProperties.Add(_contentObjectModelListProperty);

            ClassProperty ContentObjectModelListProperty = new ClassProperty("ContentObjectModelList", "List<ContentObjectModel>");
            ClassProperty RemainingContentSizeProperty = new ClassProperty("RemainingContentSize", "string");

            StringBuilder getterSetterBody = new StringBuilder();
            getterSetterBody.AppendLine("\t\t");
            getterSetterBody.AppendLine("\t\t{");
            getterSetterBody.AppendLine("\t\t    get");
            getterSetterBody.AppendLine("\t\t\t{");
            getterSetterBody.AppendLine("\t\t        return _contentObjectModelList;");
            getterSetterBody.AppendLine("\t\t    }");
            getterSetterBody.AppendLine("\t\t    set");
            getterSetterBody.AppendLine("\t\t\t{");
            getterSetterBody.AppendLine("\t\t        _contentObjectModelList = value;");
            getterSetterBody.AppendLine("\t\t        if (_contentObjectModelList != null)");
            getterSetterBody.AppendLine("\t\t        {");
            getterSetterBody.AppendLine("\t\t            foreach (var contentObject in _contentObjectModelList)");
            getterSetterBody.AppendLine("\t\t            {");
            getterSetterBody.AppendLine("\t\t                var publicUrl = \"http://\" + iObjectBucketApiService.objectStorageKeysModel.endpoint");
            getterSetterBody.AppendLine("\t\t                    + '/' + iObjectBucketApiService.objectStorageKeysModel.bucketName + '/' + contentObject.object_name;");
            getterSetterBody.AppendLine("\t\t");
            getterSetterBody.AppendLine("\t\t                contentObject.file_url = HttpUtility.UrlPathEncode(publicUrl);");
            getterSetterBody.AppendLine("\t\t            }");
            getterSetterBody.AppendLine("\t\t        }");
            getterSetterBody.AppendLine("\t\t    }");
            getterSetterBody.AppendLine("\t\t}");
            //ContentObjectModelListProperty.GetterSetterBody = getterSetterBody.ToString();

            viewModelClientClass.ClassProperties.Add(ContentObjectModelListProperty);
            viewModelClientClass.ClassProperties.Add(RemainingContentSizeProperty);

            viewModelClientClass.IsJsonProperty = true;
            viewModelClientClass.AddGetterSetter = true;
            viewModelClientClass.UpdateClassProperties();

            string classData = viewModelClientClass.GenerateCSharpClassData(false);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Database.Trim(), "ViewModel", viewModelClientClass.ClassName + ".cs"),
                classData, true);
            return this;
        }

        public ClassRoutine GenerateStartupClass(ObservableCollectionFast<SQLTable> SQLTableList)
        {
            try
            {
                CSharpClass StartupClass = new CSharpClass("Startup");

                StartupClass.CSharpClassFileSettings.IsClassNameCamelCasing = true;
                StartupClass.CSharpClassFileSettings.IsIncludeNameSpace = true;
                StartupClass.CSharpClassFileSettings.IsIncludeUsings = true;
                StartupClass.CSharpClassFileSettings.NameSpace = "RD.Projects";

                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Microsoft.AspNetCore.Builder;");
                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Microsoft.AspNetCore.Hosting;");
                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Microsoft.AspNetCore.Http;");
                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Microsoft.Extensions.Configuration;");
                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Microsoft.Extensions.DependencyInjection;");
                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Microsoft.Extensions.Hosting;");
                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using Microsoft.Extensions.Logging;");
                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using RD.Projects.DataAccess;");
                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using RD.Projects.Service;");
                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System;");
                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Collections.Generic;");
                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Linq;");
                StartupClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Threading.Tasks;");

                StartupClass.CSharpClassFileSettings.IsIncludeParametrizedConstructor = true;
                StartupClass.CSharpClassFileSettings.Parameters = new ObservableCollectionFast<ClassProperty>();
                StartupClass.CSharpClassFileSettings.Parameters.Add(new ClassProperty("configuration", "IConfiguration") { });
                StartupClass.CSharpClassFileSettings.Parameters.Add(new ClassProperty("environment", "IWebHostEnvironment") { });

                StringBuilder sbContructorContent = new StringBuilder();
                sbContructorContent.AppendLine("\t\t\tConfiguration = configuration;");
                sbContructorContent.AppendLine("\t\t\tEnvironment = environment;");
                sbContructorContent.AppendLine("\t\t\t");
                sbContructorContent.AppendLine("\t\t\tvar builder = new ConfigurationBuilder()");
                sbContructorContent.AppendLine("\t\t\t    .AddJsonFile(\"appsettings.json\", optional: false, reloadOnChange: true)");
                sbContructorContent.AppendLine("\t\t\t    .AddJsonFile($\"appsettings.{environment.EnvironmentName}.json\", optional: true)");
                sbContructorContent.AppendLine("\t\t\t    .AddEnvironmentVariables();");
                sbContructorContent.AppendLine("\t\t\tConfiguration = builder.Build();");

                StartupClass.CSharpClassFileSettings.ParameterizedConstructorContent = sbContructorContent.ToString();

                StartupClass.ClassProperties = new ObservableCollectionFast<ClassProperty>();
                StartupClass.ClassProperties.Add(new ClassProperty("_logger", "ILogger<Startup>", "private"));
                StartupClass.ClassProperties.Add(new ClassProperty("Environment", "IWebHostEnvironment", "public"));
                StartupClass.ClassProperties.Add(new ClassProperty("Configuration", "IConfiguration", "public"));
                StartupClass.ClassProperties.Add(new ClassProperty("HttpContextAccessor", "IHttpContextAccessor", "public"));

                StartupClass.ClassMethods = new List<ClassMethodModel>();

                #region "ConfigureServices Method"
                ClassMethodModel ConfigureServicesMethod = new ClassMethodModel("public", "void", "", "ConfigureServices");
                ConfigureServicesMethod.Attributes = new List<string>();
                ConfigureServicesMethod.Attributes.Add("// This method gets called by the runtime. Use this method to add services to the container.");

                ConfigureServicesMethod.Parameters = new List<ClassProperty>();
                ConfigureServicesMethod.Parameters.Add(new ClassProperty("services", "IServiceCollection"));

                StringBuilder sbConfigureServicesBody = new StringBuilder();
                sbConfigureServicesBody.AppendLine("\t\t\tservices.AddControllersWithViews();");
                sbConfigureServicesBody.AppendLine("\t\t\tservices.AddMvc(options => options.EnableEndpointRouting = false).AddSessionStateTempDataProvider();");
                sbConfigureServicesBody.AppendLine("\t\t\tIMvcBuilder builder = services.AddRazorPages();");
                sbConfigureServicesBody.AppendLine("\t\t\t");
                sbConfigureServicesBody.AppendLine("\t\t\t#if DEBUG");
                sbConfigureServicesBody.AppendLine("\t\t\tif (Environment.IsDevelopment())");
                sbConfigureServicesBody.AppendLine("\t\t\t{");
                sbConfigureServicesBody.AppendLine("\t\t\t	builder.AddRazorRuntimeCompilation();");
                sbConfigureServicesBody.AppendLine("\t\t\t}");
                sbConfigureServicesBody.AppendLine("\t\t\t#endif");
                sbConfigureServicesBody.AppendLine("\t\t\tservices.AddCors();");
                sbConfigureServicesBody.AppendLine("\t\t\tservices.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();");
                sbConfigureServicesBody.AppendLine("\t\t\t");
                sbConfigureServicesBody.AppendLine("\t\t\tvar key = Encoding.ASCII.GetBytes(Configuration.GetValue<String>(\"AppSettings:Secret\"));");
                sbConfigureServicesBody.AppendLine("\t\t\t");
                sbConfigureServicesBody.AppendLine("\t\t\tservices.AddAuthentication(x =>");
                sbConfigureServicesBody.AppendLine("\t\t\t{");
                sbConfigureServicesBody.AppendLine("\t\t\t	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;");
                sbConfigureServicesBody.AppendLine("\t\t\t	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;");
                sbConfigureServicesBody.AppendLine("\t\t\t})");
                sbConfigureServicesBody.AppendLine("\t\t\t.AddJwtBearer(x =>");
                sbConfigureServicesBody.AppendLine("\t\t\t{");
                sbConfigureServicesBody.AppendLine("\t\t\t   x.Events = new JwtBearerEvents");
                sbConfigureServicesBody.AppendLine("\t\t\t   {");
                sbConfigureServicesBody.AppendLine("\t\t\t	   OnTokenValidated = context =>");
                sbConfigureServicesBody.AppendLine("\t\t\t	   {");
                sbConfigureServicesBody.AppendLine("\t\t\t		   var userService = context.HttpContext.RequestServices.GetRequiredService<IUsersApiService>();");
                sbConfigureServicesBody.AppendLine("\t\t\t		   var userId = int.Parse(context.Principal.Identity.Name);");
                sbConfigureServicesBody.AppendLine("\t\t\t		   var user = userService.GetUsers(new UsersModel() { id = userId });");
                sbConfigureServicesBody.AppendLine("\t\t\t		   if (user == null)");
                sbConfigureServicesBody.AppendLine("\t\t\t		   {");
                sbConfigureServicesBody.AppendLine("\t\t\t			   context.Fail(\"Unauthorized\");");
                sbConfigureServicesBody.AppendLine("\t\t\t		   }");
                sbConfigureServicesBody.AppendLine("\t\t\t		   return Task.CompletedTask;");
                sbConfigureServicesBody.AppendLine("\t\t\t	   }");
                sbConfigureServicesBody.AppendLine("\t\t\t   };");
                sbConfigureServicesBody.AppendLine("\t\t\t   x.RequireHttpsMetadata = false;");
                sbConfigureServicesBody.AppendLine("\t\t\t   x.SaveToken = true;");
                sbConfigureServicesBody.AppendLine("\t\t\t   x.TokenValidationParameters = new TokenValidationParameters");
                sbConfigureServicesBody.AppendLine("\t\t\t   {");
                sbConfigureServicesBody.AppendLine("\t\t\t	   ValidateIssuerSigningKey = true,");
                sbConfigureServicesBody.AppendLine("\t\t\t	   IssuerSigningKey = new SymmetricSecurityKey(key),");
                sbConfigureServicesBody.AppendLine("\t\t\t	   ValidateIssuer = false,");
                sbConfigureServicesBody.AppendLine("\t\t\t	   ValidateAudience = false");
                sbConfigureServicesBody.AppendLine("\t\t\t   };");
                sbConfigureServicesBody.AppendLine("\t\t\t});");

                sbConfigureServicesBody.AppendLine("\t\t\t");
                sbConfigureServicesBody.AppendLine("\t\t\tstring MyWorldApiUrl = Configuration.GetValue<string>(\"MyWorldApiUrl\");");
                sbConfigureServicesBody.AppendLine("\t\t\tstring MyWorldContentApiUrl = Configuration.GetValue<string>(\"MyWorldContentApiUrl\");");

                sbConfigureServicesBody.AppendLine("\t\t\t");
                foreach (var sqlTable in SQLTableList)
                {
                    sbConfigureServicesBody.AppendLine("\t\t\tservices.AddScoped<I" + sqlTable.Name.Trim().ToCamelCase() + "ApiService" + ", " + sqlTable.Name.Trim().ToCamelCase() + "ApiService" + ">(pr => new " + sqlTable.Name.Trim().ToCamelCase() + "ApiService" + "() { MyWorldApiUrl = MyWorldApiUrl, MyWorldContentApiUrl = MyWorldContentApiUrl });");
                }

                sbConfigureServicesBody.AppendLine("\t\t\t");
                sbConfigureServicesBody.AppendLine("\t\t\tservices.Configure<CookiePolicyOptions>(options =>");
                sbConfigureServicesBody.AppendLine("\t\t\t{");
                sbConfigureServicesBody.AppendLine("\t\t\t	options.CheckConsentNeeded = context => true; // consent required");
                sbConfigureServicesBody.AppendLine("\t\t\t	options.MinimumSameSitePolicy = SameSiteMode.None;");
                sbConfigureServicesBody.AppendLine("\t\t\t});");
                sbConfigureServicesBody.AppendLine("\t\t\t");
                sbConfigureServicesBody.AppendLine("\t\t\tservices.AddSession(options =>");
                sbConfigureServicesBody.AppendLine("\t\t\t{");
                sbConfigureServicesBody.AppendLine("\t\t\t	options.IdleTimeout = TimeSpan.FromSeconds(90000);");
                sbConfigureServicesBody.AppendLine("\t\t\t	options.Cookie.HttpOnly = true;");
                sbConfigureServicesBody.AppendLine("\t\t\t	options.Cookie.IsEssential = true;");
                sbConfigureServicesBody.AppendLine("\t\t\t});");
                ConfigureServicesMethod.MethodBody = sbConfigureServicesBody.ToString();
                #endregion

                #region Configure Method
                ClassMethodModel ConfigureMethod = new ClassMethodModel("public", "void", "", "Configure");
                ConfigureMethod.Attributes = new List<string>();
                ConfigureMethod.Attributes.Add("// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.");

                ConfigureMethod.Parameters = new List<ClassProperty>();
                ConfigureMethod.Parameters.Add(new ClassProperty("app", "IApplicationBuilder"));
                ConfigureMethod.Parameters.Add(new ClassProperty("env", "IWebHostEnvironment"));
                ConfigureMethod.Parameters.Add(new ClassProperty("loggerFactory", "ILoggerFactory"));
                ConfigureMethod.Parameters.Add(new ClassProperty("httpContextAccessor", "IHttpContextAccessor"));


                StringBuilder sbConfigureMethodBody = new StringBuilder();
                sbConfigureMethodBody.AppendLine("\t\t\tvar log4Net = loggerFactory.AddLog4Net();");
                sbConfigureMethodBody.AppendLine("\t\t\t_logger = log4Net.CreateLogger<Startup>();");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\t_logger.LogInformation(\"Current Environment \" + env.EnvironmentName);");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\tif (env.IsDevelopment())");
                sbConfigureMethodBody.AppendLine("\t\t\t{");
                sbConfigureMethodBody.AppendLine("\t\t\t	app.UseDeveloperExceptionPage();");
                sbConfigureMethodBody.AppendLine("\t\t\t}");
                sbConfigureMethodBody.AppendLine("\t\t\telse");
                sbConfigureMethodBody.AppendLine("\t\t\t{");
                sbConfigureMethodBody.AppendLine("\t\t\t	app.UseExceptionHandler(\"/Home/Error\");");
                sbConfigureMethodBody.AppendLine("\t\t\t	app.UseHsts();");
                sbConfigureMethodBody.AppendLine("\t\t\t}");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\tapp.UseStaticFiles();");
                sbConfigureMethodBody.AppendLine("\t\t\tapp.UseSession();");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\tapp.Use(async (context, next) =>");
                sbConfigureMethodBody.AppendLine("\t\t\t{");
                sbConfigureMethodBody.AppendLine("\t\t\t	var JWToken = context.Session.GetString(AppConstants.JWTTOKEN);");
                sbConfigureMethodBody.AppendLine("\t\t\t	if (!string.IsNullOrEmpty(JWToken))");
                sbConfigureMethodBody.AppendLine("\t\t\t	{");
                sbConfigureMethodBody.AppendLine("\t\t\t		context.Request.Headers.Add(\"Authorization\", \"Bearer\" + JWToken);");
                sbConfigureMethodBody.AppendLine("\t\t\t	}");
                sbConfigureMethodBody.AppendLine("\t\t\t	await next();");
                sbConfigureMethodBody.AppendLine("\t\t\t});");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\tapp.UseStatusCodePages(async context =>");
                sbConfigureMethodBody.AppendLine("\t\t\t{");
                sbConfigureMethodBody.AppendLine("\t\t\t	var response = context.HttpContext.Response;");
                sbConfigureMethodBody.AppendLine("\t\t\t	if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||");
                sbConfigureMethodBody.AppendLine("\t\t\t		response.StatusCode == (int)HttpStatusCode.Forbidden)");
                sbConfigureMethodBody.AppendLine("\t\t\t		response.Redirect(\"/Account/Login\");");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\t	if (response.StatusCode == (int)HttpStatusCode.NotFound)");
                sbConfigureMethodBody.AppendLine("\t\t\t		response.Redirect(\"/Home/Error\");");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\t});");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\tapp.UseRouting();");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\tapp.UseCors(x => x");
                sbConfigureMethodBody.AppendLine("\t\t\t	.AllowAnyOrigin()");
                sbConfigureMethodBody.AppendLine("\t\t\t	.AllowAnyMethod()");
                sbConfigureMethodBody.AppendLine("\t\t\t	.AllowAnyHeader());");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\tapp.UseAuthentication();");
                sbConfigureMethodBody.AppendLine("\t\t\tapp.UseAuthorization();");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\tapp.Use(async (context, next) =>");
                sbConfigureMethodBody.AppendLine("\t\t\t{");
                sbConfigureMethodBody.AppendLine("\t\t\t	string host = context.Request.Host.Value;");
                sbConfigureMethodBody.AppendLine("\t\t\t	string scheme = context.Request.Scheme;");
                sbConfigureMethodBody.AppendLine("\t\t\t	string domain = scheme + \"://\" + host;");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\t	Helpers.Utility.CurrentDomain = domain;");
                sbConfigureMethodBody.AppendLine("\t\t\t");
                sbConfigureMethodBody.AppendLine("\t\t\t	await next();");
                sbConfigureMethodBody.AppendLine("\t\t\t});");
                sbConfigureMethodBody.AppendLine("\t\t\t");

                sbConfigureMethodBody.AppendLine("\t\t\tapp.UseEndpoints(endpoints =>");
                sbConfigureMethodBody.AppendLine("\t\t\t{");
                sbConfigureMethodBody.AppendLine("\t\t\t    endpoints.MapControllerRoute(");
                sbConfigureMethodBody.AppendLine("\t\t\t        name: \"default\",");
                sbConfigureMethodBody.AppendLine("\t\t\t        pattern: \"{controller=Dashboard}/{action=Index}/{id?}\");");
                sbConfigureMethodBody.AppendLine("\t\t\t});");

                ConfigureMethod.MethodBody = sbConfigureMethodBody.ToString();
                #endregion

                StartupClass.ClassMethods.Add(ConfigureServicesMethod);
                StartupClass.ClassMethods.Add(ConfigureMethod);

                string classData = StartupClass.GenerateCSharpClassData(false);

                FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    Database.Trim(), "Project", StartupClass.ClassName + ".cs"),
                    classData, true);
            }
            catch (Exception)
            {
                throw;
            }
            return this;
        }
    }
}
