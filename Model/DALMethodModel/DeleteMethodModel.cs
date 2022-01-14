using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    class DeleteMethodModel : ClassMethodModel
    {
        public DeleteMethodModel() : base()
        {

        }

        public DeleteMethodModel(string accessType, string returnType, string methodType, string methodName) : base(accessType, returnType, methodType, methodName)
        {

        }

        public override ClassMethodModel Initialize()
        {
            base.Initialize();
            this.AccessType = "public";
            this.ReturnType = "string";
            this.MethodType = null;
            this.MethodName = "Delete" + TableName + "Data";
            this.Parameters = new List<ClassProperty>();
            ClassProperty propData = new ClassProperty("Data", TableName + "Model");
            this.Parameters.Add(propData);

            StringBuilder sbDelete = new StringBuilder();
            sbDelete.AppendLine("\t\t\tstring _return_value = string.Empty;");
            sbDelete.AppendLine("\t\t\ttry");
            sbDelete.AppendLine("\t\t\t{");

            sbDelete.AppendLine("\t\t\t\tdbContext.cmd = new MySqlCommand();");
            sbDelete.AppendLine("\t\t\t\tdbContext.cmd.Connection = dbContext.GetConnection();");

            string CommandText = "DELETE FROM `" + TableName + "` WHERE id = @id";

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
            this.MethodBody = sbDelete.ToString();

            return this;
        }
    }
}
