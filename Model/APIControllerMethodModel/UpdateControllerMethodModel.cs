﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    class UpdateControllerMethodModel : ClassMethodModel
    {
        public UpdateControllerMethodModel() :
            base()
        {
        }

        public UpdateControllerMethodModel(string accessType, string returnType, string methodType, string methodName) :
            base(accessType, returnType, methodType, methodName)
        {

        }

        public override ClassMethodModel Initialize()
        {
            base.Initialize();

            this.AccessType = "public";
            this.ReturnType = "IActionResult";
            this.MethodType = null;
            this.MethodName = "Update" + TableName;

            this.Parameters = new List<ClassProperty>();
            ClassProperty propData = new ClassProperty("model", TableName.ToCamelCase() + "Model");
            this.Parameters.Add(propData);

            this.Attributes.Add("[HttpPost]");
            this.Attributes.Add("[Route(\"" + TableName + "/" + this.MethodName + "\")]");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("            string _rawContent = null;");
            sb.AppendLine("");
            sb.AppendLine("            var responseModel = new ResponseModel<string>()");
            sb.AppendLine("            {");
            sb.AppendLine("                HttpStatusCode = \"200\"");
            sb.AppendLine("            };");
            sb.AppendLine("");
            sb.AppendLine("            try");
            sb.AppendLine("            {");

            //sb.AppendLine("                _rawContent = GetRawContent(_rawContent);");
            //sb.AppendLine("                var objPayLoad = JsonConvert.DeserializeObject<" + TableName.ToCamelCase() + "Model>(_rawContent);");
            sb.AppendLine("                var " + TableName + "Service = new " + TableName.ToCamelCase() + "Service(_dbContext);");
            sb.AppendLine("                responseModel = " + TableName + "Service." + this.MethodName + "Data(model);");

            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                string message = \"Error while processing \";");
            sb.AppendLine("");
            sb.AppendLine("                if (!ex.Message.ToLower().Contains(\"object reference\"))");
            sb.AppendLine("                    message += ex.Message;");
            sb.AppendLine("");
            sb.AppendLine("                responseModel.HttpStatusCode = ((int)HttpStatusCode.BadRequest).ToString();");
            sb.AppendLine("                responseModel.Message = message;");
            sb.AppendLine("                responseModel.Reason.Add(\"ERROR\");");
            sb.AppendLine("                responseModel.IsSuccess = false;");
            sb.AppendLine("            }");
            sb.AppendLine("");
            sb.AppendLine("            return new JsonResult(responseModel);");

            this.MethodBody = sb.ToString();

            return this;
        }
    }
}
