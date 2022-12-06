using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Helpers;

namespace ToDoStuff.Model.SQL
{
    public abstract class BaseSQLModel
    {
        public string TableName { get; set; }
        public string TemplateBody { get; set; }

        public List<ClassProperty> ClassProperties { get; set; }
        public string procedure_name { get; set; }
        public string procedure_body { get; set; }
        public string input_params { get; set; }
        public string output_params { get; set; }

        public SQLEngineSettings sqlEngineSettings { get; set; }

        public virtual string GenerateSQLProcedure()
        {
            return TemplateBody.Replace(SQLEngineSettings.PROCEDURE_NAME, procedure_name)
                .Replace(SQLEngineSettings.PROCEDURE_BODY, procedure_body)
                .Replace(SQLEngineSettings.INPUT_PARAMS, input_params)
                .Replace(SQLEngineSettings.OUTPUT_PARAMS, string.IsNullOrEmpty(output_params) == true ? "" : "," + output_params);
        }

        public virtual void SampleMethod<U>()
        {

        }
        public virtual BaseSQLModel Initialize()
        {
            return this;
        }
        public abstract string SetProcedureBody();
        public abstract string SetProcedureInputParams();
        public abstract string SetProcedureOutputParams();

        public string CreateInputParametersForProcedure()
        {
            string input_params = string.Empty;

            foreach (var prop in ClassProperties)
            {

            }

            return input_params;
        }
    }
}
