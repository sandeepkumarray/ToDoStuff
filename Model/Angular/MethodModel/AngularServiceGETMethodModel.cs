using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model.Angular.MethodModel
{
    public class AngularServiceGETMethodModel : ClassMethodModel
    {
        public AngularClassDataSetting methodSetting { get; set; }
        public AngularServiceGETMethodModel() : base()
        {

        }
        public AngularServiceGETMethodModel(string returnType, string methodName)
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
            ClassProperty propDataID = new ClassProperty("id", "any");

            if (ClassProperties.Select(p => p.PropName.ToLower() == "user_id").Any())
                this.Parameters.Add(propData);

            this.Parameters.Add(propDataID);

            StringBuilder sbBody = new StringBuilder();
            sbBody.AppendLine("\t\tlet apiURL = `${environment.serviceUrl}" + methodSetting.APIName + "?procedureName=" + MethodName + "&user_id=` + user_id + `&id=` + id;");
            sbBody.AppendLine("\t\treturn this.http.get<"+ TableName + ">(apiURL);");
            this.MethodBody = sbBody.ToString();

            return this;
        }
    }
}
