using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Helpers;
using ToDoStuff.Model.SQL;

namespace ToDoStuff.Engine
{
    public class SQLEngine
    {
        public DataStore DataStore { get; set; }
        public string RootFilePath { get; set; }

        public SQLEngineSettings sqlEngineSettings = null;
        public List<BaseSQLModel> BaseSQLModels { get; set; }

        private SQLObjectType SelectedSQLObjectType { get; set; }

        const string ProcedureTemplateName = "SQLProcedureTemplate.txt";

        public SQLEngine()
        {
            this.BaseSQLModels = new List<BaseSQLModel>();
            this.sqlEngineSettings = GetDefaultSettings();
            DataStore = new DataStore()
            {
                Database = this.sqlEngineSettings.DatabaseName,
            };
        }

        public SQLEngine(SQLEngineSettings _sqlEngineSettings) : this()
        {
            this.sqlEngineSettings = _sqlEngineSettings;
        }

        public SQLEngine(string sqlEngineSettingsJson) : this()
        {
            this.sqlEngineSettings = SQLEngineSettings.FromJson(sqlEngineSettingsJson);
        }

        public SQLEngine SetDataStore(DataStore dataStore)
        {
            this.DataStore = dataStore;
            RootFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                     DataStore.Database.Trim(), "SQL-Output");
            return this;
        }

        public SQLEngine Initialize(SQLObjectType sqlObjectType)
        {
            this.SelectedSQLObjectType = sqlObjectType;
            foreach (DataStoreTable table in DataStore.Tables)
            {
                switch (sqlObjectType)
                {
                    case SQLObjectType.Table:
                        break;
                    case SQLObjectType.Procedure:
                        this.BaseSQLModels.Add(new SQLInsertProcedureModel("add_" + table.TableName) { TableName = table.TableName, ClassProperties = table.ColumnList, TemplateBody = this.sqlEngineSettings.GetTemplateContents(TemplateType.Procedure) }.Initialize());
                        this.BaseSQLModels.Add(new SQLSelectProcedureModel("get_" + table.TableName) { TableName = table.TableName, ClassProperties = table.ColumnList, TemplateBody = this.sqlEngineSettings.GetTemplateContents(TemplateType.Procedure) }.Initialize());
                        this.BaseSQLModels.Add(new SQLSelectAllProcedureModel("getAll_" + table.TableName) { TableName = table.TableName, ClassProperties = table.ColumnList, TemplateBody = this.sqlEngineSettings.GetTemplateContents(TemplateType.Procedure) }.Initialize());
                        this.BaseSQLModels.Add(new SQLUpdateProcedureModel("update_" + table.TableName) { TableName = table.TableName, ClassProperties = table.ColumnList, TemplateBody = this.sqlEngineSettings.GetTemplateContents(TemplateType.Procedure) }.Initialize());
                        this.BaseSQLModels.Add(new SQLDeleteProcedureModel("delete_" + table.TableName) { TableName = table.TableName, ClassProperties = table.ColumnList, TemplateBody = this.sqlEngineSettings.GetTemplateContents(TemplateType.Procedure) }.Initialize());
                        break;
                    case SQLObjectType.Cursor:
                        break;
                    case SQLObjectType.InsertScript:
                        break;
                    case SQLObjectType.DeleteScript:
                        break;
                    case SQLObjectType.SelectScript:
                        break;
                    case SQLObjectType.UpdateScript:
                        break;
                    case SQLObjectType.DBScripts:
                        break;
                    default:
                        break;
                }
            }

            return this;
        }
        public SQLEngine Process()
        {
            switch (this.SelectedSQLObjectType)
            {
                case SQLObjectType.Table:
                    break;
                case SQLObjectType.Procedure:
                    var procedureScripts = GetProcedureDetails();

                    FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                        RootFilePath, "Procedure", "procedures" + ".sql"), procedureScripts, true);

                    var dropProcedures = string.Join("\r\n", from n in this.BaseSQLModels select "DROP PROCEDURE  IF EXISTS `" + n.procedure_name + "`;");
                    FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                        RootFilePath, "Procedure", "drop_procedures" + ".sql"), dropProcedures, true);
                    break;
                case SQLObjectType.Cursor:
                    break;
                case SQLObjectType.InsertScript:
                    break;
                case SQLObjectType.DeleteScript:
                    break;
                case SQLObjectType.SelectScript:
                    break;
                case SQLObjectType.UpdateScript:
                    break;
                case SQLObjectType.DBScripts:
                    break;
                default:
                    break;
            }
            return this;
        }

        private string GetProcedureDetails()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var sqlObject in this.BaseSQLModels)
            {
                sqlObject.Initialize();
                sb.AppendLine(sqlObject.GenerateSQLProcedure());
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private SQLEngineSettings GetDefaultSettings()
        {
            SQLEngineSettings sQLEngineSettings = new SQLEngineSettings();
            sQLEngineSettings.DatabaseName = "my-database";
            return sQLEngineSettings;
        }

    }

}
