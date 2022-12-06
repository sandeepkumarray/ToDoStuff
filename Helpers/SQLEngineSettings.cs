using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Model;
using ToDoStuff.Model.SQL;

namespace ToDoStuff.Helpers
{
    public class SQLEngineSettings
    {
        public string DatabaseName { get; set; }

        public static string PROCEDURE_NAME = "[PROCEDURE_NAME]";
        public static string PROCEDURE_BODY = "[PROCEDURE_BODY]";
        public static string INPUT_PARAMS = "[INPUT_PARAMS]";
        public static string OUTPUT_PARAMS = "[OUTPUT_PARAMS]";

        public SQLObjectType sqlObjectType { get; set; }

        public SQLEngineSettings()
        {

        }

        public string GetTemplateContents(TemplateType templateType)
        {
            string content = string.Empty;
            string templatePath = string.Empty;

            switch (templateType)
            {
                case TemplateType.Login:
                    templatePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "SQL", "Procedures", "SQLProcedureTemplate.txt");
                    break;
                case TemplateType.SignUp:
                    templatePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "SQL", "Procedures", "SQLProcedureTemplate.txt");
                    break;
                case TemplateType.Procedure:
                    templatePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "SQL", "Procedures", "SQLProcedureTemplate.txt");
                    
                    break;
                default:
                    break;
            }
            content = File.ReadAllText(templatePath);
            return content;
        }

        public static SQLEngineSettings FromJson(string json) { return JsonConvert.DeserializeObject<SQLEngineSettings>(json, Converter.Settings); }
    }

    public enum SQLObjectType
    {
        Table,
        Procedure,
        Cursor,
        InsertScript,
        DeleteScript,
        SelectScript,
        UpdateScript,
        DBScripts, // Includes Insert,Delete,Select and Update
    }

    public enum TemplateType
    {
        Login,
        SignUp,
        Procedure
    }
}
