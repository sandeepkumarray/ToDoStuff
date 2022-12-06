using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToDoStuff.Model.SQL
{
    public class SQLUpdateProcedureModel : BaseSQLModel
    {
        public SQLUpdateProcedureModel()
        {
        }

        public SQLUpdateProcedureModel(string procedure_name)
        {
            this.procedure_name = procedure_name;
        }

        public SQLUpdateProcedureModel(string procedure_name, string procedure_body, string input_params, string output_params)
        {
            this.procedure_name = procedure_name;
            this.procedure_body = procedure_body;
            this.input_params = input_params;
            this.output_params = output_params;
        }

        public string V { get; }

        public override BaseSQLModel Initialize()
        {
            base.Initialize();

            if (string.IsNullOrEmpty(this.input_params))
            {
                this.input_params = string.Join(",", from n in ClassProperties select "IN `p_" + n.PropName + "` " + n.DB_ColumnType + "");
            }

            string where_condition = string.Empty;
            var primary_id = ClassProperties.Find(c => c.DB_ColumnKey == "PRI");
            if (primary_id != null)
                where_condition = " WHERE " + primary_id.PropName + " = p_" + primary_id.PropName;

            if (string.IsNullOrEmpty(this.procedure_name))
                this.procedure_name = "update_" + this.TableName;

            string updateColumns = string.Join(",", from n in ClassProperties select n.PropName + " = p_" + n.PropName);

            var CommandText = "UPDATE " + TableName + " SET " + updateColumns + where_condition + ";";
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