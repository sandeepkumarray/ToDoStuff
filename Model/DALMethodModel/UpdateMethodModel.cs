using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    class UpdateMethodModel : ClassMethodModel
    {
        public UpdateMethodModel() : base()
        {
        }

        public UpdateMethodModel(string accessType, string returnType, string methodType, string methodName) : base(accessType, returnType, methodType, methodName)
        {

        }

        public override ClassMethodModel Initialize()
        {
            base.Initialize();
            this.AccessType = "public";
            this.ReturnType = "string";
            this.MethodType = null;
            this.MethodName = "Update" + TableName.ToCamelCase() + "Data";
            this.Parameters = new List<ClassProperty>();
            ClassProperty propData = new ClassProperty("Data", TableName.ToCamelCase() + "Model");
            this.Parameters.Add(propData);

            StringBuilder sbUpdate = new StringBuilder();
            sbUpdate.AppendLine("\t\t\tstring _return_value = string.Empty;");
            sbUpdate.AppendLine("\t\t\ttry");
            sbUpdate.AppendLine("\t\t\t{");

            sbUpdate.AppendLine("\t\t\t\tdbContext.cmd = new MySqlCommand();");
            sbUpdate.AppendLine("\t\t\t\tdbContext.cmd.Connection = dbContext.GetConnection();");

            string updateColumns = string.Join(",", from n in ClassProperties select n.PropName + " = @" + n.PropName);

            var CommandText = "UPDATE " + TableName + " SET " + updateColumns;

            sbUpdate.AppendLine("\t\t\t\tdbContext.cmd.CommandText = \"" + CommandText + " WHERE id = @id\";");
            foreach (var prop in ClassProperties)
            {
                string proptype = prop.PropType;
                sbUpdate.AppendLine("\t\t\t\tdbContext.AddInParameter(dbContext.cmd, \"@" + prop.PropName.RemoveSpecialCharacters() + "\", Data." + prop.PropName.RemoveSpecialCharacters() + ");");
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

            this.MethodBody = sbUpdate.ToString();
            return this;
        }
    }
}
