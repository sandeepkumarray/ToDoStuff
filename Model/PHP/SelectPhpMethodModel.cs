using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model.PHP
{
    public class SelectPhpMethodModel : PhpMethodModel
    {
        public SelectPhpMethodModel(string MethodName, string TableName, List<ClassProperty> Parameters, TypeOfSQLObject typeOfSQLObject) : base(MethodName, TableName, Parameters, typeOfSQLObject)
        {
            this.MethodName = MethodName;
            this.TableName = TableName;
            this.Parameters = Parameters;
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder();

            content.AppendLine("function " + MethodName + "(){");
            content.AppendLine(EvaluateGETParamas(new List<string>() { "user_id", "id" }));
            content.AppendLine();
            content.AppendLine("    global $response;");
            content.AppendLine("    global $log;");
            content.AppendLine("    global $link;");
            content.AppendLine("");
            content.AppendLine("    $sql = \"SELECT * FROM " + TableName + " " + EvaluateWhereCondition(new List<string>() { "user_id", "id" }) + "\";");
            content.AppendLine("");
            content.AppendLine("    $log->info(\"sql = \".$sql);");
            content.AppendLine("    $result = mysqli_query($link, $sql);");
            content.AppendLine("    $row_cnt = $result->num_rows;");
            content.AppendLine("");
            content.AppendLine("    if ($result) {");
            content.AppendLine("        if ($row_cnt > 0) {");
            content.AppendLine("            while ($row = $result->fetch_object()) {");
            content.AppendLine("                $myArray = $row;");
            content.AppendLine("            }");
            content.AppendLine("");
            content.AppendLine("            $response->success = true;");
            content.AppendLine("            $response->data = $myArray;");
            content.AppendLine("            $result->close();");
            content.AppendLine("        } else {");
            content.AppendLine("            $response->success = false;");
            content.AppendLine("            $response->message = \"No data available in table\";");
            content.AppendLine("        }");
            content.AppendLine("    } else {");
            content.AppendLine("        $response->success = false;");
            content.AppendLine("        $response->message = \"Error: \" . $sql . \" < br > \" . mysqli_error($link);");
            content.AppendLine("    }");
            content.AppendLine("}");
            return content.ToString();
        }
    }
}
