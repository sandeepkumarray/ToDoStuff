using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model.PHP
{
    public class UpdatePhpMethodModel : PhpMethodModel
    {
        public UpdatePhpMethodModel(string MethodName, string TableName, List<ClassProperty> Parameters, TypeOfSQLObject typeOfSQLObject) :
            base(MethodName, TableName, Parameters, typeOfSQLObject)
        {
            this.MethodName = MethodName;
            this.TableName = TableName;
            this.Parameters = Parameters;
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("function " + MethodName + "($data){");
            content.AppendLine("    global $response;");
            content.AppendLine("    global $log;");
            content.AppendLine("    global $link;");
            content.AppendLine("    $id = $_GET['id']; ");
            content.AppendLine("");
            content.AppendLine("    $log->info(\"Started update function.\");");
            content.AppendLine("");
            content.AppendLine(this.GenerateColumnsForQuery(Parameters, TypeOfQuery.Set));
            content.AppendLine("");
            content.Append("    $sql = \"UPDATE " + TableName + " SET ");
            content.Append(this.GenerateColumnsForQuery(Parameters, TypeOfQuery.Update));
            content.AppendLine("    WHERE id = $id\"; ");
            content.AppendLine("");
            content.AppendLine("    $log->info(\"sql\".$sql.\"\");");
            content.AppendLine("                ");
            content.AppendLine("    if($stmt = mysqli_prepare($link, $sql)){");
            content.AppendLine("          ");
            content.AppendLine("        // Attempt to execute the prepared statement");
            content.AppendLine("        if(mysqli_stmt_execute($stmt)){");
            content.AppendLine("            $response->success = true;");
            content.AppendLine("            $priority_id = $stmt->insert_id;");
            content.AppendLine("            $response->data = $stmt->insert_id;");
            content.AppendLine("            $response->message = \"Updated successfully!!!\";");
            content.AppendLine("        } ");
            content.AppendLine("        else{");
            content.AppendLine("            $dberror= \"DB Error: \".mysqli_stmt_error($stmt);");
            content.AppendLine("            $log->info(\"\".$dberror.\"\");");
            content.AppendLine("            $response->success = false;");
            content.AppendLine("            $response->message = \"Something went wrong.Please try again later.\";");
            content.AppendLine("        }");
            content.AppendLine("    }");
            content.AppendLine("            ");
            content.AppendLine("    // Close statement");
            content.AppendLine("    mysqli_stmt_close($stmt);");
            content.AppendLine("            ");
            content.AppendLine("    $log->info(\"Completed update function.\");");
            content.Append("}");
            return content.ToString();
        }
    }
}
