using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    class SaveServiceMethodModel : ClassMethodModel
    {
        public SaveServiceMethodModel(bool IsForInterface) :
            base()
        {
            this.IsForInterface = IsForInterface;
        }

        public SaveServiceMethodModel(string accessType, string returnType, string methodType, string methodName) :
            base(accessType, returnType, methodType, methodName)
        {

        }

        public override ClassMethodModel Initialize()
        {
            base.Initialize();

            this.AccessType = this.IsForInterface ? "" : "public";
            this.ReturnType = "ResponseModel<string>";
            this.MethodType = null;
            this.MethodName = "Save" + TableName.Trim().Substring(0, TableName.Trim().Length - 1).ToCamelCase();
            this.Parameters = new List<ClassProperty>();
            ClassProperty propData = new ClassProperty("Data", TableName + "Model");
            this.Parameters.Add(propData);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("			" + this.ReturnType + " return_value = null;");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                return_value = new " + this.ReturnType + "();");
            sb.AppendLine("                " + TableName + "DAL " + TableName + "Dalobj = new " + TableName + "DAL(dbContext);");
            sb.AppendLine("                " + "string" + " value = " + TableName + "Dalobj." + "SaveData(Data);");
            sb.AppendLine("                return_value.Value = value;");
            sb.AppendLine("                return_value.Message = \"Success\";");
            sb.AppendLine("                return_value.HttpStatusCode = \"200\";");
            sb.AppendLine("                return_value.IsSuccess = true;");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception)");
            sb.AppendLine("            {");
            sb.AppendLine("                throw;");
            sb.AppendLine("            }");
            sb.AppendLine("            return return_value;");
            this.MethodBody = this.IsForInterface ? null : sb.ToString();

            return this;
        }
    }
}
