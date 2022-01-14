using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    class DALClassTemplate : CSharpClass
    {
        public string TableName { get; set; }
        public DALClassTemplate()
        {
        }

        public DALClassTemplate(string className) : base(className)
        {

        }

        public virtual void CreateMethods()
        {
            List<ClassMethodModel> Methods = new List<ClassMethodModel>();

            //Insert
            #region "INSERT"
            ClassMethodModel InsertMethod = new ClassMethodModel("public", "string", null, "Add" + TableName + "Data");
            InsertMethod.Parameters = new List<ClassProperty>();
            ClassProperty propData = new ClassProperty("Data", TableName + "Model");
            InsertMethod.Parameters.Add(propData);

            StringBuilder sbInsert = new StringBuilder();
            sbInsert.AppendLine("\t\t\tstring _return_value = string.Empty;");
            sbInsert.AppendLine("\t\t\ttry");
            sbInsert.AppendLine("\t\t\t{");

            sbInsert.AppendLine("\t\t\t\tdbContext.cmd = new MySqlCommand();");
            sbInsert.AppendLine("\t\t\t\tdbContext.cmd.Connection = dbContext.GetConnection();");
            string columns = string.Join(",", from n in ClassProperties select n.PropName);
            string values = string.Join(",", from n in ClassProperties select "@" + n.PropName);
            string CommandText = "INSERT INTO " + TableName + "(" + columns + ") VALUES(" + values + ")";
            sbInsert.AppendLine("\t\t\t\tdbContext.cmd.CommandText = \"" + CommandText + "\";");
            foreach (var prop in ClassProperties)
            {
                string proptype = prop.PropType;
                sbInsert.AppendLine("\t\t\t\tdbContext.AddInParameter(dbContext.cmd, \"@" + prop.PropName + "\", Data." + prop.PropName + ");");
            }
            sbInsert.AppendLine("\t\t\t\tdbContext.cmd.ExecuteNonQuery();");
            sbInsert.AppendLine("\t\t\t\t_return_value = Convert.ToString(dbContext.cmd.LastInsertedId);");

            sbInsert.AppendLine("\t\t\t}");
            sbInsert.AppendLine("\t\t\tcatch (Exception ex)");
            sbInsert.AppendLine("\t\t\t{");
            sbInsert.AppendLine("\t\t\t    _return_value = null;");
            sbInsert.AppendLine("\t\t\t    throw;");
            sbInsert.AppendLine("\t\t\t}");
            sbInsert.AppendLine("\t\t\t");
            sbInsert.AppendLine("\t\t\treturn _return_value;");

            InsertMethod.MethodBody = sbInsert.ToString();

            Methods.Add(InsertMethod);
            #endregion

            //Delete
            #region DELETE
            ClassMethodModel DeleteMethod = new ClassMethodModel("public", "string", null, "Delete" + TableName + "Data");
            DeleteMethod.Parameters = new List<ClassProperty>();
            propData = new ClassProperty("Data", TableName + "Model");
            DeleteMethod.Parameters.Add(propData);

            StringBuilder sbDelete = new StringBuilder();
            sbDelete.AppendLine("\t\t\tstring _return_value = string.Empty;");
            sbDelete.AppendLine("\t\t\ttry");
            sbDelete.AppendLine("\t\t\t{");

            sbDelete.AppendLine("\t\t\t\tdbContext.cmd = new MySqlCommand();");
            sbDelete.AppendLine("\t\t\t\tdbContext.cmd.Connection = dbContext.GetConnection();");

            CommandText = "DELETE FROM " + TableName + " WHERE id = @id";

            sbDelete.AppendLine("\t\t\t\tdbContext.cmd.CommandText = \"" + CommandText + "\";");
            sbDelete.AppendLine("\t\t\t\tdbContext.AddInParameter(dbContext.cmd, \"@id\", Data.id);");

            sbDelete.AppendLine("\t\t\t\t_return_value = Convert.ToString(dbContext.cmd.ExecuteNonQuery());");

            sbDelete.AppendLine("\t\t\t}");
            sbDelete.AppendLine("\t\t\tcatch (Exception ex)");
            sbDelete.AppendLine("\t\t\t{");
            sbDelete.AppendLine("\t\t\t    _return_value = null;");
            sbDelete.AppendLine("\t\t\t    throw;");
            sbDelete.AppendLine("\t\t\t}");
            sbDelete.AppendLine("\t\t\t");
            sbDelete.AppendLine("\t\t\treturn _return_value;");
            DeleteMethod.MethodBody = sbDelete.ToString();

            Methods.Add(DeleteMethod);
            #endregion

            //select 1
            #region "SELECT"
            ClassMethodModel SelectMethod = new ClassMethodModel("public", TableName + "Model", null, "Get" + TableName + "Data");
            SelectMethod.Parameters = new List<ClassProperty>();
            propData = new ClassProperty("Data", TableName + "Model");
            SelectMethod.Parameters.Add(propData);

            StringBuilder sbSelect = new StringBuilder();
            sbSelect.AppendLine("\t\t\t"+TableName + "Model _return_value = null;");
            sbSelect.AppendLine("\t\t\ttry");
            sbSelect.AppendLine("\t\t\t{");

            sbSelect.AppendLine("\t\t\t\tdbContext.cmd = new MySqlCommand();");
            sbSelect.AppendLine("\t\t\t\tdbContext.cmd.Connection = dbContext.GetConnection();");

            CommandText = "SELECT * FROM " + TableName + " WHERE id = @id";

            sbSelect.AppendLine("\t\t\t\tdbContext.cmd.CommandText = \"" + CommandText + "\";");
            sbSelect.AppendLine("\t\t\t\tdbContext.AddInParameter(dbContext.cmd, \"@id\", Data.id);");

            sbSelect.AppendLine("\t\t\t\tDataSet ds = dbContext.ExecuteDataSet(dbContext.cmd);");
            sbSelect.AppendLine("\t\t\t\tif (ds != null && ds.Tables != null && ds.Tables.Count > 0)");
            sbSelect.AppendLine("\t\t\t\t{");
            sbSelect.AppendLine("\t\t\t\t    _return_value = new " + TableName + "Model();");
            sbSelect.AppendLine("\t\t\t\t    DataTable dt = ds.Tables[0];");
            sbSelect.AppendLine("\t\t\t\t");
            sbSelect.AppendLine("\t\t\t\t    DataRow dr = dt.Rows[0];");
            sbSelect.AppendLine("\t\t\t\t");
            sbSelect.AppendLine("\t\t\t\t    " + TableName + "Model " + TableName.ToLower() + " = new " + TableName + "Model();");
            sbSelect.AppendLine(CreateDataSetToModel());
            sbSelect.AppendLine("\t\t\t\t\t_return_value = " + TableName.ToLower() + ";");
            sbSelect.AppendLine("\t\t\t\t}");

            sbSelect.AppendLine("\t\t\t}");
            sbSelect.AppendLine("\t\t\tcatch (Exception ex)");
            sbSelect.AppendLine("\t\t\t{");
            sbSelect.AppendLine("\t\t\t    _return_value = null;");
            sbSelect.AppendLine("\t\t\t    throw;");
            sbSelect.AppendLine("\t\t\t}");
            sbSelect.AppendLine("\t\t\t");
            sbSelect.AppendLine("\t\t\treturn _return_value;");

            SelectMethod.MethodBody = sbSelect.ToString();

            Methods.Add(SelectMethod);
            #endregion

            //select ALL
            #region "SELECT ALL"
            ClassMethodModel SelectAllMethod = new ClassMethodModel("public", "List<" + TableName + "Model>", null, "SelectAll" + TableName + "Data");
            SelectAllMethod.Parameters = new List<ClassProperty>();

            StringBuilder sbSelectAll = new StringBuilder();
            sbSelectAll.AppendLine("\t\t\tList < " + TableName + "Model> _return_value = null;");
            sbSelectAll.AppendLine("\t\t\ttry");
            sbSelectAll.AppendLine("\t\t\t{");

            sbSelectAll.AppendLine("\t\t\t\tdbContext.cmd = new MySqlCommand();");
            sbSelectAll.AppendLine("\t\t\t\tdbContext.cmd.Connection = dbContext.GetConnection();");

            CommandText = "SELECT * FROM " + TableName + ";";

            sbSelectAll.AppendLine("\t\t\t\tdbContext.cmd.CommandText = \"" + CommandText + "\";");

            sbSelectAll.AppendLine("\t\t\t\tDataSet ds = dbContext.ExecuteDataSet(dbContext.cmd);");
            sbSelectAll.AppendLine("\t\t\t\tif (ds != null && ds.Tables != null && ds.Tables.Count > 0)");
            sbSelectAll.AppendLine("\t\t\t\t{");
            sbSelectAll.AppendLine("\t\t\t\t\t_return_value = new List<" + TableName + "Model>();");
            sbSelectAll.AppendLine("\t\t\t\t\tDataTable dt = ds.Tables[0];");
            sbSelectAll.AppendLine("\t\t\t\t");
            sbSelectAll.AppendLine("\t\t\t\t\tforeach (DataRow dr in dt.Rows)");
            sbSelectAll.AppendLine("\t\t\t\t\t{");
            sbSelectAll.AppendLine("\t\t\t\t\t\t" + TableName + "Model " + TableName.ToLower() + " = new " + TableName + "Model();");
            sbSelectAll.AppendLine(CreateDataSetToModel());
            sbSelectAll.AppendLine("\t\t\t\t\t\t_return_value.Add(" + TableName.ToLower() + ");");
            sbSelectAll.AppendLine("\t\t\t\t\t}");
            sbSelectAll.AppendLine("\t\t\t\t}");

            sbSelectAll.AppendLine("\t\t\t}");
            sbSelectAll.AppendLine("\t\t\tcatch (Exception ex)");
            sbSelectAll.AppendLine("\t\t\t{");
            sbSelectAll.AppendLine("\t\t\t    _return_value = null;");
            sbSelectAll.AppendLine("\t\t\t    throw;");
            sbSelectAll.AppendLine("\t\t\t}");
            sbSelectAll.AppendLine("\t\t\t");
            sbSelectAll.AppendLine("\t\t\treturn _return_value;");

            SelectAllMethod.MethodBody = sbSelectAll.ToString();

            Methods.Add(SelectAllMethod);
            #endregion

            //update
            #region "UPDATE"
            ClassMethodModel UpdateMethod = new ClassMethodModel("public", "string", null, "Update" + TableName + "Data");
            UpdateMethod.Parameters = new List<ClassProperty>();
            propData = new ClassProperty("Data", TableName + "Model");
            UpdateMethod.Parameters.Add(propData);

            StringBuilder sbUpdate = new StringBuilder();
            sbUpdate.AppendLine("\t\t\tstring _return_value = string.Empty;");
            sbUpdate.AppendLine("\t\t\ttry");
            sbUpdate.AppendLine("\t\t\t{");

            sbUpdate.AppendLine("\t\t\t\tdbContext.cmd = new MySqlCommand();");
            sbUpdate.AppendLine("\t\t\t\tdbContext.cmd.Connection = dbContext.GetConnection();");

            string updateColumns = string.Join(",", from n in ClassProperties select n.PropName + " = @" + n.PropName);

            CommandText = "UPDATE " + TableName + " SET " + updateColumns;

            sbUpdate.AppendLine("\t\t\t\tdbContext.cmd.CommandText = \"" + CommandText + " WHERE id = @id\";");
            foreach (var prop in ClassProperties)
            {
                string proptype = prop.PropType;

                sbUpdate.AppendLine("\t\t\t\tdbContext.AddInParameter(dbContext.cmd, \"@" + prop.PropName + "\", Data." + prop.PropName + ");");
            }

            sbUpdate.AppendLine("\t\t\t\t_return_value = Convert.ToString(dbContext.cmd.ExecuteNonQuery());");

            sbUpdate.AppendLine("\t\t\t}");
            sbUpdate.AppendLine("\t\t\tcatch (Exception ex)");
            sbUpdate.AppendLine("\t\t\t{");
            sbUpdate.AppendLine("\t\t\t    _return_value = null;");
            sbUpdate.AppendLine("\t\t\t    throw;");
            sbUpdate.AppendLine("\t\t\t}");
            sbUpdate.AppendLine("\t\t\t");
            sbUpdate.AppendLine("\t\t\treturn _return_value;");
            UpdateMethod.MethodBody = sbUpdate.ToString();

            Methods.Add(UpdateMethod);
            #endregion

            this.ClassMethods = Methods;
        }

        string CreateDataSetToModel()
        {
            string Template = "\t\t\t\t\t[Table_Name].[COLUMN_NAME] = dr[\"[COLUMN_NAME]\"] == DBNull.Value ? default([DATA_TYPE]) : Convert.To[DATA_TYPE](dr[\"[COLUMN_NAME]\"]);";
            StringBuilder sb = new StringBuilder();
            foreach (var item in ClassProperties)
            {
                sb.AppendLine(Template.Replace("[Table_Name]", TableName.ToLower()).Replace("[COLUMN_NAME]", item.PropName).Replace("[DATA_TYPE]", item.PropType));
            }

            return sb.ToString();
        }
    }
}
