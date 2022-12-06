using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    class SelectMethodModel : ClassMethodModel
    {
        public SelectMethodModel() : base()
        {

        }

        public SelectMethodModel(string accessType, string returnType, string methodType, string methodName) : base(accessType, returnType, methodType, methodName)
        {

        }

        public override ClassMethodModel Initialize()
        {
            base.Initialize();

            this.AccessType = "public";
            this.ReturnType = TableName.ToCamelCase() + "Model";
            this.MethodType = null;
            this.MethodName = "Get" + TableName.ToCamelCase() + "Data";
            this.Parameters = new List<ClassProperty>();
            ClassProperty propData = new ClassProperty("Data", TableName.ToCamelCase() + "Model");
            string CommandText = "";

            this.Parameters.Add(propData);

            StringBuilder sbSelect = new StringBuilder();
            sbSelect.AppendLine("\t\t\t" + TableName.ToCamelCase() + "Model _return_value = null;");
            sbSelect.AppendLine("\t\t\ttry");
            sbSelect.AppendLine("\t\t\t{");

            sbSelect.AppendLine("\t\t\t\tdbContext.cmd = new MySqlCommand();");
            sbSelect.AppendLine("\t\t\t\tdbContext.cmd.Connection = dbContext.GetConnection();");

            CommandText = "SELECT * FROM `" + TableName.ToCamelCaseWithSeparator() + "` WHERE id = @id";

            sbSelect.AppendLine("\t\t\t\tdbContext.cmd.CommandText = \"" + CommandText + "\";");
            sbSelect.AppendLine("\t\t\t\tdbContext.AddInParameter(dbContext.cmd, \"@id\", Data.id);");

            sbSelect.AppendLine("\t\t\t\tDataSet ds = dbContext.ExecuteDataSet(dbContext.cmd);");
            sbSelect.AppendLine("\t\t\t\tif (ds != null && ds.Tables != null && ds.Tables.Count > 0)");
            sbSelect.AppendLine("\t\t\t\t{");
            sbSelect.AppendLine("\t\t\t\t    _return_value = new " + TableName.ToCamelCase() + "Model();");
            sbSelect.AppendLine("\t\t\t\t    DataTable dt = ds.Tables[0];");
            sbSelect.AppendLine("\t\t\t\t");
            sbSelect.AppendLine("\t\t\t\t    DataRow dr = dt.Rows[0];");
            sbSelect.AppendLine("\t\t\t\t");
            sbSelect.AppendLine("\t\t\t\t    " + TableName.ToCamelCase() + "Model " + TableName.ToCamelCase().ToLower() + " = new " + TableName.ToCamelCase() + "Model();");
            sbSelect.AppendLine(CreateDataSetToModel());
            sbSelect.AppendLine("\t\t\t\t\t_return_value = " + TableName.ToCamelCase().ToLower() + ";");
            sbSelect.AppendLine("\t\t\t\t}");

            sbSelect.AppendLine("\t\t\t}");
            sbSelect.AppendLine("\t\t\tcatch (Exception ex)");
            sbSelect.AppendLine("\t\t\t{");
            sbSelect.AppendLine("\t\t\t    _return_value = null;");
            sbSelect.AppendLine("\t\t\t    throw;");
            sbSelect.AppendLine("\t\t\t}");
            sbSelect.AppendLine("\t\t\t");
            sbSelect.AppendLine("\t\t\treturn _return_value;");

            this.MethodBody = sbSelect.ToString();

            return this;
        }
    }
}
