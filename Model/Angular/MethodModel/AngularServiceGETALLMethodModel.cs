using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model.Angular.MethodModel
{
    public class AngularServiceGETALLMethodModel : ClassMethodModel
    {
        public AngularClassDataSetting methodSetting { get; set; }
        public AngularServiceGETALLMethodModel() : base()
        {

        }
        public AngularServiceGETALLMethodModel(string returnType, string methodName)
        {
            this.TableName = returnType;
            this.ReturnType = returnType;
            this.MethodName = methodName;
        }

        public override ClassMethodModel Initialize()
        {
            base.Initialize();
            this.Parameters = new List<ClassProperty>();
            ClassProperty propData = new ClassProperty("user_id", "any");
            if (ClassProperties.Select(p => p.PropName.ToLower() == "user_id").Any())
                this.Parameters.Add(propData);

            StringBuilder sbBody = new StringBuilder();
            sbBody.AppendLine("\t\tlet apiURL = `${environment.serviceUrl}" + methodSetting.APIName + "?procedureName=" + MethodName + "&user_id=` + user_id;");
            sbBody.AppendLine("\t\treturn this.http.get<" + TableName + "[]>(apiURL);");
            this.MethodBody = sbBody.ToString();

            return this;
        }
    }
}
