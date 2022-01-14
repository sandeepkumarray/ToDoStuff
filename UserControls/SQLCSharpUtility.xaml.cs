using CreateAPIDoc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoStuff.Model;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using System.IO;
using System.Windows.Documents;
using Paragraph = MigraDoc.DocumentObjectModel.Paragraph;
using Table = MigraDoc.DocumentObjectModel.Tables.Table;
using My.World.Api.DataAccess;
using MySql.Data.MySqlClient;
using Attribute = ToDoStuff.Model.Attribute;

namespace ToDoStuff.UserControls
{
    /// <summary>
    /// Interaction logic for SQLCSharpUtility.xaml
    /// </summary>
    [ControlNameAttribute("SQL C# Utility")]
    public partial class SQLCSharpUtility : UserControl, IControlsInterface
    {
        string CSharpMethodSelected = "";
        bool IsExecuteAll = false;

        public SQLCSharpUtility()
        {
            InitializeComponent();

        }
        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }

        SQLConnector GetSQLConnector()
        {
            string server = txtServer.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string database = txtDatabase.Text.Trim();

            return new SQLConnector(server, username, password, database);
        }

        private void btnLoadTables_Click(object sender, RoutedEventArgs e)
        {
            List<string> blockTableList = new List<string>() { "character_birthtowns", "character_companions", "character_enemies", "character_floras", "character_friends", "character_items", "character_love_interests", "character_magics", "character_technologies", "content_plans", "content_types", "documents", "folders", "user_content_template", "user_details", "user_plan", "users" };

            SQLConnector sqlconn = GetSQLConnector();
            DataSet ds = sqlconn.GetTables();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                ObservableCollection<SQLTable> SQLTableList = new ObservableCollection<SQLTable>();
                DataTable dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    string tableName = dr["Tables_in_" + txtDatabase.Text.Trim()] == DBNull.Value ? default(string) : Convert.ToString(dr["Tables_in_" + txtDatabase.Text.Trim()]);

                    if (tableName.NotIn(blockTableList))
                    {
                        SQLTable sqlTable = new SQLTable();
                        sqlTable.Name = tableName;
                        SQLTableList.Add(sqlTable);
                    }
                }
                tbTableCount.Text = Convert.ToString(SQLTableList.Count);
                lstTables.ItemsSource = SQLTableList;
                lstTables.DisplayMemberPath = "Name";

            }
        }

        private void CreateClass(string TableName)
        {
            if (!string.IsNullOrEmpty(TableName))
            {
                SQLConnector sqlconn = GetSQLConnector();
                DataSet ds = sqlconn.ColumnList(TableName, txtDatabase.Text.Trim());
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    List<ClassProperty> ClassProperties = new List<ClassProperty>();
                    DataTable dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        ClassProperty prop = new ClassProperty();
                        prop.PropName = dr["COLUMN_NAME"] == DBNull.Value ? default(string) : Convert.ToString(dr["COLUMN_NAME"]);
                        string dataType = dr["DATA_TYPE"] == DBNull.Value ? default(string) : Convert.ToString(dr["DATA_TYPE"]);
                        prop.PropType = prop.GetCsharpTypeFromDBType(dataType);
                        ClassProperties.Add(prop);
                    }

                    CSharpUtility cSharpUtility = new CSharpUtility();
                    cSharpUtility.CreateCSharpClass(TableName, ClassProperties);
                }

            }
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (lstTables.SelectedItems != null && lstTables.SelectedItems.Count > 0)
                {

                    System.Collections.IList items = (System.Collections.IList)lstTables.SelectedItems;
                    var collection = items.Cast<SQLTable>();
                    ProcessForSelected(collection);
                }
                else
                {
                    string selectedTableName = Convert.ToString(((SQLTable)lstTables.SelectedValue).Name);

                    if (!string.IsNullOrEmpty(selectedTableName))
                        ProcessTable(selectedTableName);
                    else
                        MessageBox.Show("No object Selected to process!!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MessageBox.Show("Process Completed!!!");
        }

        private void btnProcessAll_Click(object sender, RoutedEventArgs e)
        {
            IsExecuteAll = true;
            IEnumerable<SQLTable> SQLTableList = (ObservableCollection<SQLTable>)lstTables.ItemsSource;
            ProcessForSelected(SQLTableList);
            MessageBox.Show("Process Completed!!!");
        }

        private void ProcessForSelected(IEnumerable<SQLTable> SQLTableList)
        {
            foreach (var tableName in SQLTableList)
            {
                ProcessTable(tableName.Name);
            }

            if (rbPostman.IsChecked == true)
            {
                if (chbAPICollectionJson.IsChecked == true)
                {
                    CreatePostmanApiJson(SQLTableList);
                }
            }

            if (rbJsonTmplt.IsChecked == true)
            {
                CreateJsonTemplates(SQLTableList);
            }
        }

        private void ProcessTable(string tableName)
        {
            List<ClassProperty> ClassProperties = new List<ClassProperty>();
            List<string> ColumnList = new List<string>();

            if (!string.IsNullOrEmpty(tableName))
            {
                SQLConnector sqlconn = GetSQLConnector();
                DataSet ds = sqlconn.ColumnList(tableName, txtDatabase.Text.Trim());

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        ClassProperty prop = new ClassProperty();
                        //prop.PropName = dr["COLUMN_NAME"] == DBNull.Value ? default(string) : Convert.ToString(dr["COLUMN_NAME"]).RemoveSpecialCharacters();
                        prop.PropName = dr["COLUMN_NAME"] == DBNull.Value ? default(string) : Convert.ToString(dr["COLUMN_NAME"]);
                        ColumnList.Add(prop.PropName);
                        string dataType = dr["DATA_TYPE"] == DBNull.Value ? default(string) : Convert.ToString(dr["DATA_TYPE"]);
                        prop.DBDataType = dataType;
                        prop.PropType = prop.GetCsharpTypeFromDBType(dataType);
                        ClassProperties.Add(prop);
                    }
                }
            }

            if (rbCSHARP.IsChecked == true)
            {
                if (chbCSharpClass.IsChecked == true)
                {
                    CSharpClass cSharpClass = new CSharpClass(tableName + "_model");
                    cSharpClass.ClassProperties = new ObservableCollection<ClassProperty>(ClassProperties);
                    cSharpClass.InheritedClass = "BaseModel";
                    cSharpClass.AddGetterSetter = true;
                    cSharpClass.CSharpClassFileSettings.IsClassNameCamelCasing = true;
                    cSharpClass.CSharpClassFileSettings.IsIncludeDefaultConstructor = true;
                    cSharpClass.CSharpClassFileSettings.IsIncludeNameSpace = true;
                    cSharpClass.CSharpClassFileSettings.IsIncludeUsings = true;
                    cSharpClass.CSharpClassFileSettings.NameSpace = string.IsNullOrEmpty(txtNamespace.Text) == true ? "My.Models" : txtNamespace.Text;

                    cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.ComponentModel;");
                    cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.ComponentModel.DataAnnotations;");

                    cSharpClass.IsJsonProperty = IsJsonProperty.IsChecked == true ? true : false;
                    cSharpClass.IsXmlAttribute = IsXmlAttribute.IsChecked == true ? true : false;
                    cSharpClass.IsXmlElement = IsXmlElement.IsChecked == true ? true : false;
                    cSharpClass.IsXmlText = IsXmlText.IsChecked == true ? true : false;
                    cSharpClass.UpdateClassProperties();

                    cSharpClass.UserDefinedPropertyAttributeList = GetUserdefinedAttributes();


                    cSharpClass.CSharpClassFileSettings.IsAdditionalCodeSnippet = chbCodeSnippet.IsChecked == true ? true : false;
                    cSharpClass.CSharpClassFileSettings.AdditionalCodeSnippet = txtCodeSnippet.Text.Trim().Replace("[CLASS_NAME]",
                        cSharpClass.CSharpClassFileSettings.IsClassNameCamelCasing ? cSharpClass.ClassName.ToCamelCase(cSharpClass.CSharpClassFileSettings.Separator) : cSharpClass.ClassName);

                    string classData = cSharpClass.GenerateCSharpClassData(false);

                    FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                        txtDatabase.Text.Trim(), "Models", cSharpClass.ClassName + ".cs"),
                        classData, true);
                }

                if (chbCSharpMethod.IsChecked == true)
                {

                    switch (CSharpMethodSelected)
                    {
                        case "ADODALCLASS":

                            DALClassTemplate cSharpClass = new DALClassTemplate(tableName);
                            cSharpClass.ClassName = (tableName).ToCamelCase() + "DAL";
                            cSharpClass.InheritedClass = "BaseDAL";
                            cSharpClass.ClassProperties = new ObservableCollection<ClassProperty>(ClassProperties);
                            cSharpClass.CSharpClassFileSettings.IsClassNameCamelCasing = true;
                            cSharpClass.CSharpClassFileSettings.IsIncludeDefaultConstructor = true;
                            cSharpClass.CSharpClassFileSettings.IsIncludeNameSpace = true;
                            cSharpClass.CSharpClassFileSettings.IsIncludeUsings = true;
                            cSharpClass.CSharpClassFileSettings.NameSpace = string.IsNullOrEmpty(txtNamespace.Text) == true ? "My.DataAccess" : txtNamespace.Text;

                            string userUsings = new TextRange(rtbUserUsings.Document.ContentStart, rtbUserUsings.Document.ContentEnd).Text;

                            foreach (var item in userUsings.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add(item);
                            }
                            //cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using My.World.Api.Models;");
                            //cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using MySql.Data.MySqlClient;");
                            //cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Configuration;");
                            //cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add("using System.Data;");

                            cSharpClass.IsJsonProperty = IsJsonProperty.IsChecked == true ? true : false;
                            cSharpClass.IsXmlAttribute = IsXmlAttribute.IsChecked == true ? true : false;
                            cSharpClass.IsXmlElement = IsXmlElement.IsChecked == true ? true : false;
                            cSharpClass.IsXmlText = IsXmlText.IsChecked == true ? true : false;
                            cSharpClass.UpdateClassProperties();
                            cSharpClass.TableName = tableName.ToCamelCase();
                            cSharpClass.Constructors = CreateApiServiceConstructors((tableName).ToCamelCase() + "DAL");

                            cSharpClass.CreateMethods();

                            cSharpClass.ClassProperties = null;
                            //cSharpClass.CSharpClassFileSettings.IsAdditionalCodeSnippet = chbCodeSnippet.IsChecked == true ? true : false;
                            //cSharpClass.CSharpClassFileSettings.AdditionalCodeSnippet = txtCodeSnippet.Text.Trim().Replace("[CLASS_NAME]",
                            //    cSharpClass.CSharpClassFileSettings.IsClassNameCamelCasing ? cSharpClass.ClassName.ToCamelCase(cSharpClass.CSharpClassFileSettings.Separator) : cSharpClass.ClassName);

                            string classData = cSharpClass.GenerateCSharpClassData(false);

                            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                                txtDatabase.Text.Trim(), "DataAccess", (tableName).ToCamelCase() + "DAL" + ".cs"),
                                classData, true);
                            break;
                        case "APICONTROLLERCLASS":
                            CreateApiControllerClass();
                            break;
                        case "APISERVICECLASS":
                            CreateApiServiceClass(tableName);
                            break;
                        case "APISERVICECLASSCLIENT":
                            CreateApiServiceClassForClient(tableName);
                            break;
                        case "RUNROUTINE":
                            ClassRoutine classRoutine = new ClassRoutine(tableName, ClassProperties);
                            classRoutine.Database = txtDatabase.Text.Trim();

                            classRoutine
                                .GenerateControllerClass()
                                .GenerateAPIServiceClientClass()
                                .GenerateCSHtmls()
                                .GenerateJSFiles()
                                .GenerateDALClass()
                                .GenerateAPIServiceClass()
                                .GenerateViewModelFiles();

                            break;
                        default:
                            break;
                    }
                }

                if (chbJsonFile.IsChecked == true)
                {
                    JObject jsonObj = new JObject();
                    foreach (var item in ColumnList)
                    {
                        jsonObj.Add(item, item);
                    }

                    FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), txtDatabase.Text.Trim(), tableName + ".json"),
                        jsonObj.ToString(), true);
                }

                if (chbDataSetClass.IsChecked == true)
                {
                    string Template = txtADODatasetTemplate.Text.Trim();
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in ClassProperties)
                    {
                        sb.AppendLine(Template.Replace("[Table_Name]", tableName).Replace("[COLUMN_NAME]", item.PropName).Replace("[DATA_TYPE]", item.PropType));
                    }

                    FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), txtDatabase.Text.Trim(), tableName + ".txt"),
                        sb.ToString(), false);
                }

                if (chbTemplate.IsChecked == true)
                {
                    string content = new TextRange(rtbTemplate.Document.ContentStart, rtbTemplate.Document.ContentEnd).Text;

                    if (!string.IsNullOrEmpty(content))
                    {
                        string server = txtServer.Text.Trim();
                        string username = txtUsername.Text.Trim();
                        string password = txtPassword.Text.Trim();
                        string database = txtDatabase.Text.Trim();

                        DBContext dbContext = new DBContext("port=3306;server=" + server + ";user id=" + username + ";password=" + password + ";database=" + database);
                        dbContext.cmd = new MySqlCommand();
                        dbContext.cmd.Connection = dbContext.GetConnection();
                        dbContext.cmd.CommandText = "SELECT * FROM `" + tableName + "`;";
                        DataSet ds = dbContext.ExecuteDataSet(dbContext.cmd);

                        StringBuilder sb = new StringBuilder();

                        if (content.Contains("[table_name]"))
                            content = content.Replace("[table_name]", tableName);

                        if (content.Contains("[TABLE_NAME]"))
                            content = content.Replace("[TABLE_NAME]", tableName.ToCamelCase());

                        sb.AppendLine(content);
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                        {
                            DataTable dt = ds.Tables[0];
                            foreach (DataRow dr in dt.Rows)
                            {
                                string template = content;
                                foreach (var col in ColumnList)
                                {
                                    template = template.Replace("[" + col + "]", Convert.ToString(dr[col]));
                                }
                                //sb.AppendLine(template);
                            }
                        }
                        FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), txtDatabase.Text.Trim(), "Template.txt"),
                            sb.ToString(), false);
                    }

                }

            }

            if (rbAngular.IsChecked == true)
            {
                if (chbAngularClass.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        StringBuilder sb = new StringBuilder();
                        string finalName = tableName.ToCamelCase();

                        sb.AppendLine("export class " + finalName.Substring(0, finalName.Length - 1) + " {");
                        foreach (var col in ColumnList)
                        {
                            sb.AppendLine("\tpublic " + col + "? : string;");
                        }
                        sb.AppendLine("}");

                        FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), txtDatabase.Text.Trim(), finalName.Substring(0, finalName.Length - 1) + ".ts"),
                            sb.ToString(), false);
                    }
                }
            }

            if (rbHtml.IsChecked == true)
            {
                if (chbHtmlControl.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        string Template = new System.Windows.Documents.TextRange(txtHTMLCodeSnippet.Document.ContentStart, txtHTMLCodeSnippet.Document.ContentEnd).Text;
                        StringBuilder sb = new StringBuilder();
                        string finalName = tableName.ToCamelCase();

                        foreach (var item in ClassProperties)
                        {
                            sb.AppendLine(Template.Replace("[CONTROL_NAME]", item.PropName));
                        }

                        FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), txtDatabase.Text.Trim(), finalName.Substring(0, finalName.Length - 1) + ".html"),
                            sb.ToString(), true);
                    }

                    if (IsExecuteAll)
                    {
                        ObservableCollection<SQLTable> SQLTableList = (ObservableCollection<SQLTable>)lstTables.ItemsSource;


                        string Template = new System.Windows.Documents.TextRange(txtHTMLCodeSnippet.Document.ContentStart, txtHTMLCodeSnippet.Document.ContentEnd).Text;
                        StringBuilder sb = new StringBuilder();
                        string finalName = "ExecuteAll_output.txt";

                        foreach (var table_Name in SQLTableList)
                        {
                            if (!string.IsNullOrEmpty(table_Name.Name))
                            {
                                sb.AppendLine(Template.Replace("[TABLE_NAME]", table_Name.Name.ToCamelCase()));
                            }

                            if (Template.Contains("[CONTROL_NAME]"))
                            {
                                ClassProperties = new List<ClassProperty>();

                                SQLConnector sqlconn = GetSQLConnector();
                                DataSet ds = sqlconn.ColumnList(tableName, txtDatabase.Text.Trim());

                                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                                {
                                    DataTable dt = ds.Tables[0];
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        ClassProperty prop = new ClassProperty();
                                        prop.PropName = dr["COLUMN_NAME"] == DBNull.Value ? default(string) : Convert.ToString(dr["COLUMN_NAME"]).RemoveSpecialCharacters();
                                        string dataType = dr["DATA_TYPE"] == DBNull.Value ? default(string) : Convert.ToString(dr["DATA_TYPE"]);
                                        prop.PropType = prop.GetCsharpTypeFromDBType(dataType);
                                        ClassProperties.Add(prop);
                                    }
                                }
                                foreach (var item in ClassProperties)
                                {
                                    sb.AppendLine(Template.Replace("[CONTROL_NAME]", item.PropName));
                                }
                            }
                        }

                        FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), txtDatabase.Text.Trim(), finalName.Substring(0, finalName.Length - 1) + ".html"),
                            sb.ToString(), true);
                    }
                }
            }
        }

        private void CreateJsonTemplates(IEnumerable<SQLTable> SQLTableList)
        {
            string jsonTemplate = "";

            List<ContentType> contentTypes = new List<ContentType>();

            List<string> blockColumnList = new List<string>() { "id", "created_at", "updated_at", "user_id", "archived_at", "deleted_at" };

            List<string> buildingsCategories = new List<string>() { "amenities", "design", "financial", "gallery", "history", "location", "neighborhood", "notes", "occupants", "overview", "purpose" };
            List<string> charactersCategories = new List<string>() { "family", "gallery", "history", "inventory", "looks", "nature", "notes", "overview", "social" };
            List<string> conditionsCategories = new List<string>() { "analysis", "causes", "effects", "gallery", "history", "notes", "overview", "treatment" };
            List<string> continentsCategories = new List<string>() { "climate", "culture", "gallery", "geography", "history", "nature", "notes", "overview" };
            List<string> countriesCategories = new List<string>() { "culture", "gallery", "geography", "history", "notes", "overview", "points_of_interest" };
            List<string> creaturesCategories = new List<string>() { "classification", "comparisons", "evolution", "gallery", "habitat", "looks", "notes", "overview", "reproduction", "traits" };
            List<string> deitiesCategories = new List<string>() { "appearance", "family", "gallery", "history", "notes", "overview", "powers", "rituals", "symbolism" };
            List<string> florasCategories = new List<string>() { "appearance", "classification", "ecosystem", "gallery", "notes", "overview", "produce" };
            List<string> foodsCategories = new List<string>() { "changelog", "eating", "effects", "gallery", "history", "notes", "overview", "recipe", "sales" };
            List<string> governmentsCategories = new List<string>() { "assets", "gallery", "history", "ideologies", "members", "notes", "overview", "populace", "process", "structure" };
            List<string> groupsCategories = new List<string>() { "gallery", "hierarchy", "inventory", "locations", "members", "notes", "overview", "politics", "purpose" };
            List<string> itemsCategories = new List<string>() { "abilities", "gallery", "history", "looks", "notes", "overview" };
            List<string> jobsCategories = new List<string>() { "gallery", "history", "notes", "overview", "requirements", "rewards", "risks", "specialization" };
            List<string> landmarksCategories = new List<string>() { "appearance", "ecosystem", "gallery", "history", "location", "notes", "overview" };
            List<string> languagesCategories = new List<string>() { "entities", "gallery", "grammar", "info", "notes", "overview", "phonology", "vocabulary" };
            List<string> locationsCategories = new List<string>() { "cities", "culture", "gallery", "geography", "history", "notes", "overview" };
            List<string> loresCategories = new List<string>() { "about", "content", "culture", "gallery", "history", "notes", "origin", "overview", "setting", "truthiness", "variations" };
            List<string> magicsCategories = new List<string>() { "alignment", "appearance", "effects", "gallery", "notes", "overview", "requirements" };
            List<string> planetsCategories = new List<string>() { "astral", "climate", "gallery", "geography", "history", "inhabitants", "notes", "overview", "time" };
            List<string> racesCategories = new List<string>() { "culture", "gallery", "history", "looks", "notes", "overview", "traits" };
            List<string> religionsCategories = new List<string>() { "beliefs", "gallery", "history", "notes", "overview", "spread", "traditions" };
            List<string> scenesCategories = new List<string>() { "action", "gallery", "looks", "notes", "overview" };
            List<string> sportsCategories = new List<string>() { "culture", "gallery", "history", "notes", "overview", "playing", "setup" };
            List<string> technologiesCategories = new List<string>() { "appearance", "gallery", "notes", "overview", "presence", "production", "related_technologies", "use" };
            List<string> townsCategories = new List<string>() { "culture", "gallery", "history", "layout", "notes", "overview", "populace", "sustainability" };
            List<string> traditionsCategories = new List<string>() { "celebrations", "gallery", "history", "notes", "observance", "overview" };
            List<string> universesCategories = new List<string>() { "contributors", "gallery", "history", "notes", "overview", "rules" };
            List<string> vehiclesCategories = new List<string>() { "gallery", "looks", "manufacturing", "notes", "operators", "overview", "travel" };

            contentTypes.Add(new ContentType("buildings", buildingsCategories));
            contentTypes.Add(new ContentType("characters", charactersCategories));
            contentTypes.Add(new ContentType("conditions", conditionsCategories));
            contentTypes.Add(new ContentType("continents", continentsCategories));
            contentTypes.Add(new ContentType("countries", countriesCategories));
            contentTypes.Add(new ContentType("creatures", creaturesCategories));
            contentTypes.Add(new ContentType("deities", deitiesCategories));
            contentTypes.Add(new ContentType("floras", florasCategories));
            contentTypes.Add(new ContentType("foods", foodsCategories));
            contentTypes.Add(new ContentType("governments", governmentsCategories));
            contentTypes.Add(new ContentType("groups", groupsCategories));
            contentTypes.Add(new ContentType("items", itemsCategories));
            contentTypes.Add(new ContentType("jobs", jobsCategories));
            contentTypes.Add(new ContentType("landmarks", landmarksCategories));
            contentTypes.Add(new ContentType("languages", languagesCategories));
            contentTypes.Add(new ContentType("locations", locationsCategories));
            contentTypes.Add(new ContentType("lores", loresCategories));
            contentTypes.Add(new ContentType("magics", magicsCategories));
            contentTypes.Add(new ContentType("planets", planetsCategories));
            contentTypes.Add(new ContentType("races", racesCategories));
            contentTypes.Add(new ContentType("religions", religionsCategories));
            contentTypes.Add(new ContentType("scenes", scenesCategories));
            contentTypes.Add(new ContentType("sports", sportsCategories));
            contentTypes.Add(new ContentType("technologies", technologiesCategories));
            contentTypes.Add(new ContentType("towns", townsCategories));
            contentTypes.Add(new ContentType("traditions", traditionsCategories));
            contentTypes.Add(new ContentType("universes", universesCategories));
            contentTypes.Add(new ContentType("vehicles", vehiclesCategories));

            ContentTemplate contentTemplate = new ContentTemplate();
            contentTemplate.TemplateName = "Content Template";
            contentTemplate.Contents = new List<Content>();

            foreach (var tableName in SQLTableList)
            {
                if (!string.IsNullOrEmpty(tableName.Name))
                {
                    Content content = new Content("", true);
                    content.content_type = tableName.Name;

                    List<string> categories = contentTypes.Find(x => x.Name.ToLower() == tableName.Name.ToLower()).Categories;

                    List<string> ColumnList = new List<string>();

                    SQLConnector sqlconn = GetSQLConnector();
                    DataSet ds = sqlconn.ColumnList(tableName.Name, txtDatabase.Text.Trim());

                    foreach (var cat in categories)
                    {
                        Category category = new Category();
                        category.Label = cat;
                        category.Icon = cat;
                        category.Order = 1;

                        if (cat.ToLower() == "overview")
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            {
                                DataTable dt = ds.Tables[0];
                                foreach (DataRow dr in dt.Rows)
                                {
                                    Attribute attr = new Attribute();
                                    attr.field_name = dr["COLUMN_NAME"] == DBNull.Value ? default(string) : Convert.ToString(dr["COLUMN_NAME"]).RemoveSpecialCharacters();

                                    if (attr.field_name.NotIn(blockColumnList))
                                    {
                                        attr.allow_null = dr["IS_NULLABLE"] == DBNull.Value ? default(bool) : Convert.ToString(dr["IS_NULLABLE"]).RemoveSpecialCharacters() == "YES" ? true : false;
                                        attr.auto_increament = dr["EXTRA"] == DBNull.Value ? default(bool) : Convert.ToString(dr["COLUMN_NAME"]).RemoveSpecialCharacters() == "auto_increment" ? true : false;

                                        attr.field_label = dr["COLUMN_NAME"] == DBNull.Value ? default(string) : Convert.ToString(dr["COLUMN_NAME"]).RemoveSpecialCharacters().ToCamelCaseWithNewSeparator();
                                        attr.field_type = dr["DATA_TYPE"] == DBNull.Value ? default(string) : Convert.ToString(dr["DATA_TYPE"]).RemoveSpecialCharacters();
                                        //attr.FieldValue = dr["COLUMN_NAME"] == DBNull.Value ? default(string) : Convert.ToString(dr["COLUMN_NAME"]).RemoveSpecialCharacters();
                                        category.Attributes.Add(attr);
                                    }
                                }
                            }
                        content.categories.Add(category);

                    }


                    var contentJSON = JsonConvert.SerializeObject(content, Formatting.Indented);
                    FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    txtDatabase.Text.Trim(), "Templates", tableName.Name + ".json"), contentJSON, true);

                    contentTemplate.Contents.Add(content);
                }

            }
            jsonTemplate = JsonConvert.SerializeObject(contentTemplate, Formatting.Indented);
            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            txtDatabase.Text.Trim(), "Templates", "MainTemplate.json"), jsonTemplate, true);
        }

        private List<PropertyAttribute> GetUserdefinedAttributes()
        {
            List<PropertyAttribute> returnValue = new List<PropertyAttribute>();
            PropertyAttribute propertyattribute = new PropertyAttribute("DisplayName", null, "");
            returnValue.Add(propertyattribute);
            return returnValue;
        }

        private void CreatePostmanApiJson(IEnumerable<SQLTable> SQLTableList)
        {
            PostmanCollectionModel postmanCollectionModel = new PostmanCollectionModel();
            //string filePath = @"C:\Users\sande\Desktop\my_book\My_Book_API.postman_collection.json";

            //postmanCollectionModel = JsonConvert.DeserializeObject<PostmanCollectionModel>(File.ReadAllText(filePath));

            postmanCollectionModel = new ToDoStuff.Model.PostmanCollectionModel();

            postmanCollectionModel.info = new ToDoStuff.Model.Info();
            postmanCollectionModel.info._postman_id = Guid.NewGuid().ToString();
            postmanCollectionModel.info.name = "My_Book_API_Gen V2";
            postmanCollectionModel.info.schema = "https://schema.getpostman.com/json/collection/v2.1.0/collection.json";

            foreach (var tableName in SQLTableList)
            {
                List<string> ColumnList = new List<string>();

                if (!string.IsNullOrEmpty(tableName.Name))
                {
                    SQLConnector sqlconn = GetSQLConnector();
                    DataSet ds = sqlconn.ColumnList(tableName.Name, txtDatabase.Text.Trim());

                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            ClassProperty prop = new ClassProperty();
                            prop.PropName = dr["COLUMN_NAME"] == DBNull.Value ? default(string) : Convert.ToString(dr["COLUMN_NAME"]).RemoveSpecialCharacters();
                            ColumnList.Add(prop.PropName);
                        }
                    }
                }

                JObject jsonObj = new JObject();
                foreach (var item in ColumnList)
                {
                    jsonObj.Add(item, item);
                }

                ToDoStuff.Model.Item tableItem = new ToDoStuff.Model.Item();
                tableItem.description = "Apis related to " + tableName.Name.ToCamelCase() + ".";
                tableItem.name = tableName.Name.ToCamelCase();
                tableItem.request = null;
                tableItem.response = null;

                List<string> methodTypes = new List<string>(new[] { "Add", "Update", "Get", "Delete", "GetAll" });
                foreach (var mtype in methodTypes)
                {
                    string ResponseModelType = "string";
                    string methodType = "POST";

                    if (mtype == "Get")
                        ResponseModelType = tableName + "Model";

                    if (mtype == "GetAll")
                        ResponseModelType = "List<" + tableName + "Model>";

                    if (mtype == "GetAll")
                        methodType = "GET";
                    else
                        methodType = "POST";


                    ToDoStuff.Model.Item apiItem = new ToDoStuff.Model.Item();
                    apiItem.description = null;
                    apiItem.item = null;
                    apiItem.name = mtype + tableName.Name.ToCamelCase();
                    apiItem.request = new ToDoStuff.Model.Request();
                    apiItem.request.body = new ToDoStuff.Model.Body();
                    apiItem.request.body.mode = "raw";
                    apiItem.request.body.options = new ToDoStuff.Model.Options();
                    apiItem.request.body.options.raw = new ToDoStuff.Model.Raw();
                    apiItem.request.body.options.raw.language = "json";
                    apiItem.request.body.raw = jsonObj.ToString();
                    apiItem.request.header = null;
                    apiItem.request.@method = methodType;
                    apiItem.request.url = new Url();
                    apiItem.request.url.raw = "http://www.my-world.com/api/myworld/" + mtype + tableName.Name.ToCamelCase(); ;
                    apiItem.request.url.protocol = "http";
                    apiItem.request.url.host = new List<string>();
                    apiItem.request.url.host.AddRange(new[] { "www", "my-world", "com" });

                    apiItem.request.url.path = new List<string>();
                    apiItem.request.url.path.AddRange(new[] { "api", "myworld", mtype + tableName.Name.ToCamelCase() });

                    apiItem.response = null;

                    tableItem.item.Add(apiItem);
                }

                postmanCollectionModel.item.Add(tableItem);
            }

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), txtDatabase.Text.Trim(), "postman_collection" + ".json"),
               JsonConvert.SerializeObject(postmanCollectionModel), true);
        }

        private void CreateApiServiceClass(string tableName)
        {
            try
            {
                CSharpClass cSharpInterface = new CSharpClass(tableName.Trim() + "_Service");

                cSharpInterface.CSharpClassFileSettings.IsClassNameCamelCasing = true;
                cSharpInterface.CSharpClassFileSettings.IsIncludeNameSpace = true;
                cSharpInterface.CSharpClassFileSettings.IsIncludeUsings = true;
                cSharpInterface.CSharpClassFileSettings.NameSpace = string.IsNullOrEmpty(txtNamespace.Text) == true ? "My.Services" : txtNamespace.Text;
                string userUsings = new TextRange(rtbUserUsings.Document.ContentStart, rtbUserUsings.Document.ContentEnd).Text;
                foreach (var item in userUsings.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    cSharpInterface.CSharpClassFileSettings.UserDefinedUsings.Add(item);
                }

                cSharpInterface.CSharpClassFileSettings.IsInterface = true;

                cSharpInterface.ClassMethods = CreateApiServiceMethods(tableName.ToCamelCase(), false);

                string interfaceData = cSharpInterface.GenerateCSharpClassData(false);

                FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    txtDatabase.Text.Trim(), "Services", "I" + cSharpInterface.ClassName + ".cs"),
                    interfaceData, true);

                CSharpClass cSharpClass = new CSharpClass(tableName.Trim() + "_Service");
                cSharpClass.InheritedClass = "I" + cSharpInterface.ClassName;
                cSharpClass.CSharpClassFileSettings.IsIncludeDefaultConstructor = true;
                cSharpClass.CSharpClassFileSettings.IsClassNameCamelCasing = true;
                cSharpClass.CSharpClassFileSettings.IsIncludeNameSpace = true;
                cSharpClass.CSharpClassFileSettings.IsIncludeUsings = true;
                cSharpClass.CSharpClassFileSettings.NameSpace = string.IsNullOrEmpty(txtNamespace.Text) == true ? "My.Services" : txtNamespace.Text;
                userUsings = new TextRange(rtbUserUsings.Document.ContentStart, rtbUserUsings.Document.ContentEnd).Text;
                foreach (var item in userUsings.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add(item);
                }

                cSharpClass.ClassMethods = CreateApiServiceMethods(tableName.ToCamelCase());
                cSharpClass.Constructors = CreateApiServiceConstructors(cSharpInterface.ClassName);
                cSharpClass.ClassProperties = CreateApiServiceClassProperties();

                string classData = cSharpClass.GenerateCSharpClassData(false);

                FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    txtDatabase.Text.Trim(), "Services", cSharpClass.ClassName + ".cs"),
                    classData, true);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private ObservableCollection<ClassProperty> CreateApiServiceClassProperties()
        {
            ObservableCollection<ClassProperty> classProperties = new ObservableCollection<ClassProperty>();
            ClassProperty classProperty = new ClassProperty("dbContext", "DBContext");

            classProperties.Add(classProperty);

            return classProperties;
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
                sb.AppendLine("			this.dbContext = dbContext;");
                method.MethodBody = sb.ToString();
            }
            Constructors.Add(method);

            return Constructors;
        }

        private List<ClassMethodModel> CreateApiServiceMethods(string tableName, bool hasBody = true)
        {
            List<ClassMethodModel> Methods = new List<ClassMethodModel>();
            List<string> methodTypes = new List<string>(new[] { "Add", "Update", "Get", "Delete", "GetAll" });
            foreach (var mtype in methodTypes)
            {
                string ResponseModelType = "string";

                if (mtype == "Get")
                    ResponseModelType = tableName + "Model";

                if (mtype == "GetAll")
                    ResponseModelType = "List<" + tableName + "Model>";

                ClassMethodModel method = new ClassMethodModel(hasBody ? "public" : "", "ResponseModel<" + ResponseModelType + ">", "", mtype + tableName + "Data");
                method.Parameters = new List<ClassProperty>();
                ClassProperty propData = new ClassProperty("Data", tableName + "Model");

                if (!mtype.Contains("GetAll"))
                    method.Parameters.Add(propData);

                if (hasBody)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("			ResponseModel<" + ResponseModelType + "> return_value = null;");
                    sb.AppendLine("            try");
                    sb.AppendLine("            {");
                    sb.AppendLine("                return_value = new ResponseModel<" + ResponseModelType + ">();");
                    sb.AppendLine("                " + tableName + "DAL " + tableName + "Dalobj = new " + tableName + "DAL(dbContext);");

                    if (mtype == "GetAll")
                        sb.AppendLine("                " + ResponseModelType + " value = " + tableName + "Dalobj." + "SelectAll" + tableName + "Data" + "();");
                    else
                        sb.AppendLine("                " + ResponseModelType + " value = " + tableName + "Dalobj." + mtype + tableName + "Data" + "(Data);");

                    sb.AppendLine("                return_value.Value = value;");
                    sb.AppendLine("                return_value.Message = \"Success\";");
                    sb.AppendLine("                return_value.HttpStatusCode = \"200\";");
                    sb.AppendLine("                return_value.IsSuccess = true;");
                    sb.AppendLine("            }");
                    sb.AppendLine("            catch (Exception)");
                    sb.AppendLine("            {");
                    sb.AppendLine("");
                    sb.AppendLine("                throw;");
                    sb.AppendLine("            }");
                    sb.AppendLine("            return return_value;");
                    method.MethodBody = sb.ToString();
                }
                Methods.Add(method);
            }
            return Methods;
        }

        private void cbClassTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CSharpMethodSelected = ((ComboBoxItem)cbClassTemplate.SelectedItem).Name;// "ADODALCLASS";

            rtbUserUsings.Document.Blocks.Clear();
            switch (CSharpMethodSelected)
            {
                case "ADODALCLASS":
                    rtbUserUsings.AppendText(Environment.NewLine + "using My.Models;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using MySql.Data.MySqlClient;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using System.Configuration;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using System.Data;");
                    break;
                case "APICONTROLLERCLASS":
                    rtbUserUsings.AppendText(Environment.NewLine + "using My.Models;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using My.Services;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using Newtonsoft.Json;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using System.Net;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using System.Net.Http;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using System.Threading.Tasks;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using System.Web;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using System.Web.Http;");
                    break;
                case "APISERVICECLASS":
                    rtbUserUsings.AppendText(Environment.NewLine + "using My.Models;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using My.DataAccess;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using System.Web;");
                    break;
                case "APISERVICECLASSCLIENT":
                    rtbUserUsings.AppendText(Environment.NewLine + "using My.Models;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using System.Web;");
                    rtbUserUsings.AppendText(Environment.NewLine + "using System.Threading.Tasks;");
                    break;
                default:
                    break;
            }
        }

        void CreateApiControllerClass()
        {
            try
            {
                ObservableCollection<SQLTable> SQLTableList = (ObservableCollection<SQLTable>)lstTables.ItemsSource;

                CSharpClass cSharpClass = new CSharpClass(txtDatabase.Text.Trim() + "_Controller");

                cSharpClass.CSharpClassFileSettings.Attributes.Add("[ApiController]");
                cSharpClass.CSharpClassFileSettings.Attributes.Add("[Route(\"api/" + txtDatabase.Text.Trim() + "\")]");

                cSharpClass.CSharpClassFileSettings.IsClassNameCamelCasing = true;
                cSharpClass.CSharpClassFileSettings.IsIncludeNameSpace = true;
                cSharpClass.CSharpClassFileSettings.IsIncludeUsings = true;
                cSharpClass.CSharpClassFileSettings.NameSpace = string.IsNullOrEmpty(txtNamespace.Text) == true ? "My.Controller" : txtNamespace.Text;
                string userUsings = new TextRange(rtbUserUsings.Document.ContentStart, rtbUserUsings.Document.ContentEnd).Text;
                foreach (var item in userUsings.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add(item);
                }

                cSharpClass.ClassMethods = CreateApiControllerMethods();


                cSharpClass.ClassProperties = CreateApiServiceClassProperties();
                cSharpClass.InheritedClass = "ControllerBase";

                string classData = cSharpClass.GenerateCSharpClassData(false);

                FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    txtDatabase.Text.Trim(), "Controllers", cSharpClass.ClassName + ".cs"),
                    classData, true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<ClassMethodModel> CreateApiControllerMethods()
        {
            List<ClassMethodModel> Methods = new List<ClassMethodModel>();
            ObservableCollection<SQLTable> SQLTableList = (ObservableCollection<SQLTable>)lstTables.ItemsSource;
            List<string> methodTypes = new List<string>(new[] { "Add", "Update", "Get", "Delete", "GetAll" });

            foreach (var tableName in SQLTableList)
            {
                StringBuilder sb = new StringBuilder();

                Methods.Add(new AddControllerMethodModel() { TableName = (tableName.Name).ToCamelCase(), LeadingLine = "#region " + (tableName.Name).ToCamelCase() }.Initialize());
                Methods.Add(new GetControllerMethodModel() { TableName = (tableName.Name).ToCamelCase() }.Initialize());
                Methods.Add(new DeleteControllerMethodModel() { TableName = (tableName.Name).ToCamelCase() }.Initialize());
                Methods.Add(new GetAllControllerMethodModel() { TableName = (tableName.Name).ToCamelCase() }.Initialize());
                Methods.Add(new SaveControllerMethodModel() { TableName = (tableName.Name).ToCamelCase(), TrailingLine = "#endregion " + (tableName.Name).ToCamelCase() }.Initialize());

                //foreach (var mtype in methodTypes)
                //{
                //ClassMethodModel method = new ClassMethodModel("public", "Task<IActionResult>", "async", mtype + tableName.Name.ToCamelCase());

                //string ResponseModelType = "string";

                //if (mtype == "Get")
                //    ResponseModelType = tableName.Name.ToCamelCase() + "Model";

                //if (mtype == "GetAll")
                //    ResponseModelType = "List<" + tableName.Name.ToCamelCase() + "Model>";

                //if (mtype == "GetAll")
                //    method.Attributes.Add("[HttpGet]");
                //else
                //    method.Attributes.Add("[HttpPost]");


                //method.Attributes.Add("[Route(\"" + mtype + tableName.Name.ToCamelCase() + "\")]");

                //sb.AppendLine("            string _rawContent = null;");
                //sb.AppendLine("");
                //sb.AppendLine("            var responseModel = new ResponseModel<" + ResponseModelType + ">()");
                //sb.AppendLine("            {");
                //sb.AppendLine("                HttpStatusCode = \"200\"");
                //sb.AppendLine("            };");
                //sb.AppendLine("");
                //sb.AppendLine("            try");
                //sb.AppendLine("            {");

                //if (mtype == "GetAll")
                //{
                //    sb.AppendLine("                var " + tableName.Name + "Service = new " + tableName.Name.ToCamelCase() + "Service(_dbContext);");
                //    sb.AppendLine("                responseModel = " + tableName.Name + "Service." + mtype + tableName.Name.ToCamelCase() + "Data();");
                //}
                //else
                //{
                //    sb.AppendLine("                _rawContent = await GetRawContent(_rawContent);");
                //    sb.AppendLine("                var objPayLoad = JsonConvert.DeserializeObject<" + tableName.Name.ToCamelCase() + "Model>(_rawContent);");
                //    sb.AppendLine("                var " + tableName.Name + "Service = new " + tableName.Name.ToCamelCase() + "Service(_dbContext);");
                //    sb.AppendLine("                responseModel = " + tableName.Name + "Service." + mtype + tableName.Name.ToCamelCase() + "Data(objPayLoad);");
                //}

                //sb.AppendLine("            }");
                //sb.AppendLine("            catch (Exception ex)");
                //sb.AppendLine("            {");
                //sb.AppendLine("                string message = \"Error while processing \";");
                //sb.AppendLine("");
                //sb.AppendLine("                if (!ex.Message.ToLower().Contains(\"object reference\"))");
                //sb.AppendLine("                    message += ex.Message;");
                //sb.AppendLine("");
                //sb.AppendLine("                responseModel.HttpStatusCode = ((int)HttpStatusCode.BadRequest).ToString();");
                //sb.AppendLine("                responseModel.Message = message;");
                //sb.AppendLine("                responseModel.Reason.Add(\"ERROR\");");
                //sb.AppendLine("                responseModel.IsSuccess = false;");
                //sb.AppendLine("            }");
                //sb.AppendLine("");
                //sb.AppendLine("            return new JsonResult(responseModel);");

                //method.MethodBody = sb.ToString();

                //Methods.Add(method);

                //}
            }

            return Methods;
        }

        private void CreateAPIDoc()
        {
            StringBuilder APILinks = new StringBuilder();

            ObservableCollection<SQLTable> SQLTableList = (ObservableCollection<SQLTable>)lstTables.ItemsSource;
            List<string> methodTypes = new List<string>(new[] { "Add", "Update", "Get", "Delete", "GetAll" });

            APIDocInfoModel apiDocInfoModel = new APIDocInfoModel();
            apiDocInfoModel.DocInfoList = new List<DocInfoModel>();

            Paragraph paraHead = new Paragraph();

            paraHead.AddFormattedText("API Description", "Heading1");
            paraHead.AddBookmark("API Description");

            DocInfoModel docInfoModelHead = new DocInfoModel();
            docInfoModelHead.SectionObjects.Add(paraHead);

            apiDocInfoModel.DocInfoList.Add(docInfoModelHead);

            foreach (var tableName in SQLTableList)
            {
                DocInfoModel docInfoModel = new DocInfoModel();

                Paragraph paragraph = new Paragraph();

                paragraph.AddFormattedText(tableName.Name.ToCamelCase(), "Heading2");

                docInfoModel.SectionObjects.Add(paragraph);

                JObject jsonObj = new JObject();

                if (!string.IsNullOrEmpty(tableName.Name))
                {
                    SQLConnector sqlconn = GetSQLConnector();
                    DataSet ds = sqlconn.ColumnList(tableName.Name, txtDatabase.Text.Trim());

                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            ClassProperty prop = new ClassProperty();
                            prop.PropName = dr["COLUMN_NAME"] == DBNull.Value ? default(string) : Convert.ToString(dr["COLUMN_NAME"]).RemoveSpecialCharacters();

                            jsonObj.Add(prop.PropName, prop.PropName);
                        }
                    }
                }

                foreach (var mtype in methodTypes)
                {
                    string methodType = "POST";

                    if (mtype == "GetAll")
                        methodType = "GET";

                    string APILink = "http://192.168.0.2/mybook.api/api/myworld/" + mtype + tableName.Name.ToCamelCase();
                    APILinks.AppendLine(APILink);

                    Paragraph paraAPIType = new Paragraph();
                    paraAPIType.AddFormattedText(mtype.ToCamelCase(), "Heading3");
                    docInfoModel.SectionObjects.Add(paraAPIType);
                    Table TableData = new Table();
                    TableData.Borders.Width = 0.75;

                    Column column = TableData.AddColumn(Unit.FromCentimeter(5));
                    column.Format.Alignment = ParagraphAlignment.Center;
                    TableData.AddColumn(Unit.FromCentimeter(10));
                    Row row = TableData.AddRow();
                    //row.Shading.Color = MigraDoc.DocumentObjectModel.Colors.PaleGoldenrod;
                    Cell cell = row.Cells[0];
                    cell.AddParagraph("Function");
                    cell = row.Cells[1];
                    cell.AddParagraph(mtype + " " + tableName.Name.ToCamelCase());

                    row = TableData.AddRow();
                    cell = row.Cells[0];
                    cell.AddParagraph("API Link");
                    cell = row.Cells[1];
                    cell.AddParagraph(APILink);

                    row = TableData.AddRow();
                    cell = row.Cells[0];
                    cell.AddParagraph("Type");
                    cell = row.Cells[1];
                    cell.AddParagraph(methodType);

                    row = TableData.AddRow();
                    cell = row.Cells[0];
                    cell.AddParagraph("Data");
                    cell = row.Cells[1];
                    cell.AddParagraph(jsonObj.ToString());

                    docInfoModel.SectionObjects.Add(TableData);
                    Paragraph paraSpacegap = new Paragraph();
                    paraSpacegap.Format.SpaceBefore = "1cm";
                    docInfoModel.SectionObjects.Add(paraSpacegap);
                }


                apiDocInfoModel.DocInfoList.Add(docInfoModel);
            }

            //FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            //    txtDatabase.Text.Trim(), "myworldAPILinks.txt"),
            //    APILinks.ToString(), true);

            string Version = "V.1.6";
            apiDocInfoModel.Title = "My World API Documentation.";
            apiDocInfoModel.Subject = "API Document";
            apiDocInfoModel.Author = "Sandeep Ray";
            apiDocInfoModel.Version = Version;
            apiDocInfoModel.FilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                txtDatabase.Text.Trim(), "My_World_API_Document_" + Version + ".pdf");

            var doc = APIDocument.CreateDocument(apiDocInfoModel);
            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(doc, "MigraDoc.mdddl");

            MigraDoc.Rendering.PdfDocumentRenderer renderer = new MigraDoc.Rendering.PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = doc;

            renderer.RenderDocument();
            string filename = apiDocInfoModel.FilePath;
            renderer.PdfDocument.Save(filename);

            System.Diagnostics.Process.Start(filename);
        }

        private void btnCreateAPIDoc_Click(object sender, RoutedEventArgs e)
        {
            CreateAPIDoc();
        }

        private void CreateApiServiceClassForClient(string tableName)
        {
            try
            {
                CSharpClass cSharpInterface = new CSharpClass(tableName.Trim().ToCamelCase() + "_API_Service");

                cSharpInterface.CSharpClassFileSettings.IsClassNameCamelCasing = true;
                cSharpInterface.CSharpClassFileSettings.IsIncludeNameSpace = true;
                cSharpInterface.CSharpClassFileSettings.IsIncludeUsings = true;
                cSharpInterface.CSharpClassFileSettings.NameSpace = string.IsNullOrEmpty(txtNamespace.Text) == true ? "My.Services" : txtNamespace.Text;
                string userUsings = new TextRange(rtbUserUsings.Document.ContentStart, rtbUserUsings.Document.ContentEnd).Text;
                foreach (var item in userUsings.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    cSharpInterface.CSharpClassFileSettings.UserDefinedUsings.Add(item);
                }
                cSharpInterface.CSharpClassFileSettings.IsInterface = true;
                cSharpInterface.ClassMethods = CreateApiServiceMethodsForClient(tableName.ToCamelCase(), false);
                string interfaceData = cSharpInterface.GenerateCSharpClassData(false);

                FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    txtDatabase.Text.Trim(), "APIServices", "I" + cSharpInterface.ClassName + ".cs"),
                    interfaceData, true);

                CSharpClass cSharpClass = new CSharpClass(tableName.Trim().ToCamelCase() + "_API_Service");
                cSharpClass.InheritedClass = "BaseAPIService, I" + cSharpInterface.ClassName;
                cSharpClass.CSharpClassFileSettings.IsClassNameCamelCasing = true;
                cSharpClass.CSharpClassFileSettings.IsIncludeNameSpace = true;
                cSharpClass.CSharpClassFileSettings.IsIncludeUsings = true;
                cSharpClass.CSharpClassFileSettings.NameSpace = string.IsNullOrEmpty(txtNamespace.Text) == true ? "My.Services" : txtNamespace.Text;
                userUsings = new TextRange(rtbUserUsings.Document.ContentStart, rtbUserUsings.Document.ContentEnd).Text;
                foreach (var item in userUsings.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    cSharpClass.CSharpClassFileSettings.UserDefinedUsings.Add(item);
                }

                cSharpClass.ClassMethods = CreateApiServiceMethodsForClient(tableName.ToCamelCase());
                //cSharpClass.ClassProperties = CreateApiServiceClassProperties();

                string classData = cSharpClass.GenerateCSharpClassData(false);

                FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    txtDatabase.Text.Trim(), "APIServices", cSharpClass.ClassName + ".cs"),
                    classData, true);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<ClassMethodModel> CreateApiServiceMethodsForClient(string tableName, bool hasBody = true)
        {
            List<ClassMethodModel> Methods = new List<ClassMethodModel>();
            List<string> methodTypes = new List<string>(new[] { "Add", "Update", "Get", "Delete", "GetAll" });
            foreach (var mtype in methodTypes)
            {
                string ResponseModelType = "string";
                string methodType = "POST";

                if (mtype == "Get")
                    ResponseModelType = tableName + "Model";

                if (mtype == "GetAll")
                    ResponseModelType = "List<" + tableName + "Model>";

                if (mtype == "GetAll")
                    methodType = "GET";
                else
                    methodType = "POST";

                ClassMethodModel method = new ClassMethodModel(hasBody ? "public async" : "", "Task<" + ResponseModelType + ">", "", mtype + tableName);
                method.Parameters = new List<ClassProperty>();
                ClassProperty propData = new ClassProperty("model", tableName + "Model");

                if (!mtype.Contains("GetAll"))
                    method.Parameters.Add(propData);

                if (hasBody)
                {
                    StringBuilder sb = new StringBuilder();

                    if (mtype == "GetAll")
                        sb.AppendLine("			" + ResponseModelType + " " + tableName.ToLower() + "Model = new " + ResponseModelType + "();");
                    else
                        sb.AppendLine("			" + ResponseModelType + " " + tableName.ToLower() + "Model = null;");

                    sb.AppendLine("			RestHttpClient client = new RestHttpClient();");
                    sb.AppendLine("			client.Host = MyWorldApiUrl;");
                    sb.AppendLine("			client.ApiUrl = \"" + mtype + tableName + "\";");
                    sb.AppendLine("			client.ServiceMethod = Method." + methodType + ";");

                    if (mtype != "GetAll")
                        sb.AppendLine("			client.RequestBody = model;");

                    sb.AppendLine("			string jsonResult = await client.GetResponseAsync();");

                    sb.AppendLine("			ResponseModel<" + ResponseModelType + "> response = JsonConvert.DeserializeObject<ResponseModel<" + ResponseModelType + ">>(jsonResult);");

                    sb.AppendLine("			" + tableName.ToLower() + "Model = response.Value;");
                    sb.AppendLine("			return " + tableName.ToLower() + "Model;");

                    method.MethodBody = sb.ToString();
                }
                Methods.Add(method);
            }
            return Methods;
        }
    }

    class SQLTable
    {
        public string Name { get; set; }
    }

    class ContentType
    {
        public ContentType()
        {

        }
        public ContentType(string name, List<string> categories)
        {
            Name = name;
            Categories = categories;
        }

        public string Name;
        public List<string> Categories;
    }
}
