using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model.PHP
{
    public class PhpFileModel
    {
        public List<PhpIncludeModel> IncludeList { get; set; }
        public List<string> Headers { get; set; }
        public List<PhpVariableModel> VariableList { get; set; }
        public List<PhpSection> SectionList { get; set; }

        public PhpFileModel()
        {
            Headers = new List<string>();
            IncludeList = new List<PhpIncludeModel>();
            VariableList = new List<PhpVariableModel>();
            SectionList = new List<PhpSection>();
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder();
            if (Headers != null && Headers.Any())
            {
                foreach (var header in Headers)
                {
                    content.AppendLine(header);
                }
                content.AppendLine("");
            }

            if (IncludeList != null && IncludeList.Any())
            {
                foreach (var include in IncludeList)
                {
                    content.AppendLine(include.IncludeString + " \"" + include.IncludeValue + "\";");
                }
                content.AppendLine("");
            }

            if (VariableList != null && VariableList.Any())
            {
                foreach (var variable in VariableList)
                {
                    content.AppendLine("$" + variable.VariableName + " = new " + variable.VariableType + ";");
                }
                content.AppendLine("");
            }

            if (SectionList != null && SectionList.Any())
            {
                foreach (var section in SectionList)
                {
                    content.AppendLine(section.SectionBody);
                }
            }

            return content.ToString();
        }
    }

    public class PhpIncludeModel
    {
        public string IncludeString { get; set; }
        public string IncludeValue { get; set; }

        public PhpIncludeModel(string IncludeString, string IncludeValue)
        {
            this.IncludeString = IncludeString;
            this.IncludeValue = IncludeValue;
        }
    }

    public class PhpVariableModel
    {
        public string VariableName { get; set; }
        public string VariableType { get; set; }
        public PhpVariableModel(string VariableName, string VariableType)
        {
            this.VariableName = VariableName;
            this.VariableType = VariableType;
        }
    }

    public class PhpSection
    {
        public string SectionBody { get; set; }
    }

    public class PhpFunctionCondition
    {
        public string FunctionName { get; set; }
        public bool HasParameters { get; set; }
        public PhpFunctionCondition(string FunctionName, bool HasParameters = true)
        {
            this.FunctionName = FunctionName;
            this.HasParameters = HasParameters;
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("\tif ($procedureName == \"" + FunctionName + "\") {");
            content.AppendLine("\t\t" + FunctionName + (HasParameters == true ? "($data);" : "();"));
            content.AppendLine("\t}");
            return content.ToString();
        }
    }
}
