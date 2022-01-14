using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    public class InsertMethodModel : ClassMethodModel
    {
        public InsertMethodModel() : base()
        {
        }

        public InsertMethodModel(string accessType, string returnType, string methodType, string methodName) : base(accessType, returnType, methodType, methodName)
        {

        }

        public override ClassMethodModel Initialize()
        {
            base.Initialize();
            this.AccessType = "public";
            this.ReturnType = "string";
            this.MethodType = null;
            this.MethodName = "Add" + TableName + "Data";
            this.Parameters = new List<ClassProperty>();
            ClassProperty propData = new ClassProperty("Data", TableName + "Model");
            this.Parameters.Add(propData);

            StringBuilder sbInsert = new StringBuilder();
            sbInsert.AppendLine("\t\t\tstring _return_value = string.Empty;");
            sbInsert.AppendLine("\t\t\ttry");
            sbInsert.AppendLine("\t\t\t{");

            sbInsert.AppendLine("\t\t\t\tdbContext.cmd = new MySqlCommand();");
            sbInsert.AppendLine("\t\t\t\tdbContext.cmd.Connection = dbContext.GetConnection();");
            if (ClassProperties.Any(i => i.PropName == "id"))
                ClassProperties.Remove(ClassProperties.First(i => i.PropName == "id"));

            string columns = string.Join("`,`", from n in ClassProperties select n.PropName);
            string values = string.Join(",", from n in ClassProperties select "@" + n.PropName.RemoveSpecialCharacters());
            string CommandText = "INSERT INTO `" + TableName + "`(`" + columns + "`) VALUES(" + values + ")";
            sbInsert.AppendLine("\t\t\t\tdbContext.cmd.CommandText = \"" + CommandText + "\";");
            foreach (var prop in ClassProperties)
            {
                string proptype = prop.PropType;
                sbInsert.AppendLine("\t\t\t\tdbContext.AddInParameter(dbContext.cmd, \"@" + prop.PropName.RemoveSpecialCharacters() + "\", Data." + prop.PropName.RemoveSpecialCharacters() + ");");
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

            this.MethodBody = sbInsert.ToString();
            return this;
        }
    }
}
