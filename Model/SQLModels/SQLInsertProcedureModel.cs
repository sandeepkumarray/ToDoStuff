using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToDoStuff.Model.SQL
{
    public class SQLInsertProcedureModel : BaseSQLModel
    {
        public SQLInsertProcedureModel()
        {
        }

        public SQLInsertProcedureModel(string procedure_name)
        {
            this.procedure_name = procedure_name;
        }

        public SQLInsertProcedureModel(string procedure_name, string procedure_body, string input_params, string output_params)
        {
            this.procedure_name = procedure_name;
            this.procedure_body = procedure_body;
            this.input_params = input_params;
            this.output_params = output_params;
        }

        public override BaseSQLModel Initialize()
        {
            base.Initialize();

            //if (ClassProperties.Any(i => i.PropName == "id"))
            //{
            //    ClassProperties.Remove(ClassProperties.First(i => i.PropName == "id"));
            //    this.output_params = "OUT `id` BIGINT";
            //}

            var primary_id = ClassProperties.Find(c => c.DB_ColumnKey == "PRI");
            if (primary_id != null)
                this.output_params = " OUT `" + primary_id.PropName + "` " + primary_id.DB_ColumnType;

            if (string.IsNullOrEmpty(this.input_params))
            {
                this.input_params = string.Join(",", from n in ClassProperties where n.DB_ColumnKey != "PRI" select "IN `p_" + n.PropName + "` " + n.DB_ColumnType + "");

            }

            if (string.IsNullOrEmpty(this.procedure_name))
                this.procedure_name = "insert_" + this.TableName;


            string columns = string.Join("`,`", from n in ClassProperties where n.DB_ColumnKey != "PRI" select n.PropName);
            string values = string.Join(",", from n in ClassProperties where n.DB_ColumnKey != "PRI" select "p_" + n.PropName.RemoveSpecialCharacters());
            string CommandText = "INSERT INTO `" + TableName.ToCamelCaseWithSeparator() + "`(`" + columns + "`) VALUES(" + values + ");";
            
            StringBuilder body = new StringBuilder();
            body.AppendLine("\t" + CommandText);
            if (primary_id != null)
                body.AppendLine("\tSET " + primary_id.PropName + "= LAST_INSERT_ID();");


            this.procedure_body = body.ToString();
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
