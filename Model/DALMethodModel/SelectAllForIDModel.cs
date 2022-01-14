﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    class SelectAllForIDModel : ClassMethodModel
    {
        public SelectAllForIDModel() : base()
        {

        }

        public SelectAllForIDModel(string accessType, string returnType, string methodType, string methodName) : base(accessType, returnType, methodType, methodName)
        {

        }

        public override ClassMethodModel Initialize()
        {
            base.Initialize();
            this.AccessType = "public";
            this.ReturnType = "List<" + TableName + "Model>";
            this.MethodType = null;
            this.MethodName = "GetAll" + TableName + "ForUserID";
            this.Parameters = new List<ClassProperty>();
            ClassProperty propData = new ClassProperty("userId", "long");
            this.Parameters.Add(propData);
            StringBuilder sbSelectAll = new StringBuilder();
            sbSelectAll.AppendLine("\t\t\tList<" + TableName + "Model> _return_value = null;");
            sbSelectAll.AppendLine("\t\t\ttry");
            sbSelectAll.AppendLine("\t\t\t{");

            sbSelectAll.AppendLine("\t\t\t\tdbContext.cmd = new MySqlCommand();");
            sbSelectAll.AppendLine("\t\t\t\tdbContext.cmd.Connection = dbContext.GetConnection();");

            string CommandText = "SELECT * FROM `" + TableName + "` where user_id = @user_id;";

            sbSelectAll.AppendLine("\t\t\t\tdbContext.cmd.CommandText = \"" + CommandText + "\";");

            sbSelectAll.AppendLine("\t\t\t\tdbContext.AddInParameter(dbContext.cmd, \"@user_id\",userId);");

            sbSelectAll.AppendLine("\t\t\t\tDataSet ds = dbContext.ExecuteDataSet(dbContext.cmd);");
            sbSelectAll.AppendLine("\t\t\t\tif (ds != null && ds.Tables != null && ds.Tables.Count > 0)");
            sbSelectAll.AppendLine("\t\t\t\t{");
            sbSelectAll.AppendLine("\t\t\t\t\t_return_value = new List<" + TableName + "Model>();");
            sbSelectAll.AppendLine("\t\t\t\t\tDataTable dt = ds.Tables[0];");
            sbSelectAll.AppendLine("\t\t\t\t");
            sbSelectAll.AppendLine("\t\t\t\t\tforeach (DataRow dr in dt.Rows)");
            sbSelectAll.AppendLine("\t\t\t\t\t{");
            sbSelectAll.AppendLine("\t\t\t\t\t\t" + TableName + "Model " + TableName.ToLower() + " = new " + TableName + "Model();");
            sbSelectAll.AppendLine(CreateDataSetToModel());
            sbSelectAll.AppendLine("\t\t\t\t\t\t_return_value.Add(" + TableName.ToLower() + ");");
            sbSelectAll.AppendLine("\t\t\t\t\t}");
            sbSelectAll.AppendLine("\t\t\t\t}");

            sbSelectAll.AppendLine("\t\t\t}");
            sbSelectAll.AppendLine("\t\t\tcatch (Exception ex)");
            sbSelectAll.AppendLine("\t\t\t{");
            sbSelectAll.AppendLine("\t\t\t    _return_value = null;");
            sbSelectAll.AppendLine("\t\t\t    throw;");
            sbSelectAll.AppendLine("\t\t\t}");
            sbSelectAll.AppendLine("\t\t\t");
            sbSelectAll.AppendLine("\t\t\treturn _return_value;");

            this.MethodBody = sbSelectAll.ToString();
            return this;
        }
    }
}
