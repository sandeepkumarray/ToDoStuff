using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Model.SQL;

namespace ToDoStuff.Model.SQL
{
    public class SQLSelectAllProcedureModel : BaseSQLModel
    {
        public SQLSelectAllProcedureModel()
        {
        }

        public SQLSelectAllProcedureModel(string procedure_name)
        {
            this.procedure_name = procedure_name;
        }

        public SQLSelectAllProcedureModel(string procedure_name, string procedure_body, string input_params, string output_params)
        {
            this.procedure_name = procedure_name;
            this.procedure_body = procedure_body;
            this.input_params = input_params;
            this.output_params = output_params;
        }

        public override BaseSQLModel Initialize()
        {
            base.Initialize();

            if (string.IsNullOrEmpty(this.procedure_name))
                this.procedure_name = "select_all_" + this.TableName;

            string updateColumns = string.Join(",", from n in ClassProperties select n.PropName + " = p_" + n.PropName);

            var CommandText = "SELECT * FROM `" + TableName.ToCamelCaseWithSeparator() + "`;";
            this.procedure_body = CommandText;
            return this;
        }

        public override string SetProcedureBody()
        {
            throw new NotImplementedException();
        }

        public override string SetProcedureInputParams()
        {
            throw new NotImplementedException();
        }

        public override string SetProcedureOutputParams()
        {
            throw new NotImplementedException();
        }
    }
}
