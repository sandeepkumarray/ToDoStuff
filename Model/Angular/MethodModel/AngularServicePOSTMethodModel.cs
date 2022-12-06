using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model.Angular.MethodModel
{
    public class AngularServicePOSTMethodModel : ClassMethodModel
    {
        public AngularClassDataSetting methodSetting { get; set; }
        public AngularServicePOSTMethodModel() : base()
        {

        }
        public AngularServicePOSTMethodModel(string returnType, string methodName)
        {
            this.TableName = returnType;
            this.ReturnType = returnType;
            this.MethodName = methodName;
        }

        public override ClassMethodModel Initialize()
        {
            base.Initialize();
            this.Parameters = new List<ClassProperty>();
            ClassProperty propData = new ClassProperty(TableName.ToLower(), TableName);
            this.Parameters.Add(propData);

            StringBuilder sbBody = new StringBuilder();
            sbBody.AppendLine("\t\tlet apiURL = `${environment.serviceUrl}" + methodSetting.APIName + "`;");
            sbBody.AppendLine("\t\t");
            sbBody.AppendLine("\t\t");
            sbBody.AppendLine("\t\t" + TableName.ToLower() + ".procedureName = \"" + MethodName + "\";");
            sbBody.AppendLine("\t\t");
            sbBody.AppendLine("\t\tvar jsonData = " + TableName.ToLower() + ";");
            sbBody.AppendLine("\t\t");
            sbBody.AppendLine("\t\tconst httpOptions = {");
            sbBody.AppendLine("\t\t  headers: new HttpHeaders({");
            sbBody.AppendLine("\t\t	'Content-Type': 'application/json'");
            sbBody.AppendLine("\t\t  })");
            sbBody.AppendLine("\t\t};");
            sbBody.AppendLine("\t\t");
            sbBody.AppendLine("\t\treturn this.http.post<ResponseModel>(apiURL, { data: jsonData }, httpOptions);");
            this.MethodBody = sbBody.ToString();

            return this;
        }

    }
}
