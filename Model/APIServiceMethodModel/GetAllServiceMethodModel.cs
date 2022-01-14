using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    class GetAllServiceMethodModel : ClassMethodModel
    {
        public GetAllServiceMethodModel(bool IsForInterface) :
            base()
        {
            this.IsForInterface = IsForInterface;
        }

        public GetAllServiceMethodModel(string accessType, string returnType, string methodType, string methodName) :
            base(accessType, returnType, methodType, methodName)
        {

        }

        public override ClassMethodModel Initialize()
        {
            base.Initialize();

            this.AccessType = this.IsForInterface ? "" : "public";
            this.ReturnType = "ResponseModel<List<" + TableName + "Model >>";
            this.MethodType = null;
            this.MethodName = "GetAll" + TableName + "ForUserID";
            this.Parameters = new List<ClassProperty>();
            ClassProperty propData = new ClassProperty("userId", "long");
            this.Parameters.Add(propData);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("			" + this.ReturnType + " return_value = null;");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                return_value = new " + this.ReturnType + "();");
            sb.AppendLine("                " + TableName + "DAL " + TableName + "Dalobj = new " + TableName + "DAL(dbContext);");
            sb.AppendLine("                " + "List<" + TableName + "Model>" + " value = " + TableName + "Dalobj." + "GetAll" + TableName + "ForUserID" + "(userId);");
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
