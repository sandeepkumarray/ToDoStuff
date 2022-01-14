using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace ToDoStuff
{
    public class ParseRailsSchemaToSQL
    {
        string SchameDbString = null;
        public ParseRailsSchemaToSQL(Uri FilePath)
        {
            SchameDbString = File.ReadAllText(FilePath.AbsolutePath);
        }

        public ParseRailsSchemaToSQL(string SchemaString)
        {
            SchameDbString = SchemaString;
        }

        public string ConvertToSQL()
        {
            string result = null;
            StringBuilder sb = new StringBuilder();
            try
            {
                if (SchameDbString != null)
                {
                    string regEx = "[^\r\n]+((\r|\n|\r\n)[^\r\n]+)*";
                    MatchCollection mc = Regex.Matches(SchameDbString, regEx);

                    foreach (Match m in mc)
                    {
                        Console.WriteLine(m);
                        sb.AppendLine(ProcessParagraph(m.Value));
                    }
                }
                else
                {
                    MessageBox.Show("No Schema provided.");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return sb.ToString();
        }

        private string ProcessParagraph(string value)
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (value != null)
            {
                string[] allLines = value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                List<string> innerList = new List<string>();
                for (int i = 0; i < allLines.Length; i++)
                {
                    string Line = allLines[i];
                    Line = Line.Replace('"', '`');
                    if (i == 0)
                    {
                        var chLine = Line.Replace("create_table", "CREATE TABLE")
                            .Replace(", force: :cascade do |t|", " ( ");

                        Line = chLine;
                        sb.AppendLine(Line);
                    }
                    else if (i == allLines.Length - 1)
                    {
                        Line = Line.Replace("end", ");");

                        innerList.RemoveAll(p => string.IsNullOrEmpty(p));

                        sb.AppendLine(string.Join(",\r\n", innerList));
                        sb.AppendLine(Line);
                    }
                    else
                    {
                        string column = "";
                        string columntype = "";
                        List<string> allParts = Line.Split(' ').ToList();

                        allParts = allParts.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                        column = allParts[1].Replace(',', ' ');
                        columntype = GetProperSQLType(column, allParts, Line);
                        if (!string.IsNullOrEmpty(columntype))
                        {
                            if (i != allLines.Length - 2)
                                Line = columntype;
                            else
                                Line = columntype;

                        }
                        else
                        {
                            Line = string.Empty;
                        }
                        innerList.Add(Line);
                    }

                }


                result = sb.ToString();

            }

            return result;
        }

        private string GetProperSQLType(string column, List<string> allParts, string line)
        {
            string type = allParts[0].Replace("t.", "");
            string returnValue = "";
            switch (type)
            {
                case "binary":
                    returnValue = column + " " + type + "(1)";
                    break;
                case "boolean":
                    returnValue = column + " " + type;
                    break;
                case "date":
                    returnValue = column + " " + type;
                    break;
                case "datetime":
                    returnValue = column + " " + type;
                    break;
                case "decimal":
                    returnValue = column + " " + type;
                    break;
                case "float":
                    returnValue = column + " " + type;
                    break;
                case "integer":
                    returnValue = column + " " + "bigint";
                    break;
                case "bigint":
                    returnValue = column + " " + type;
                    break;
                case "primary_key":
                    returnValue = column + " " + type;
                    break;
                case "references":
                    returnValue = column + " " + type;
                    break;
                case "string":
                    returnValue = column + " " + "text";
                    break;
                case "text":
                    returnValue = column + " " + type;
                    break;
                case "time":
                    returnValue = column + " " + type;
                    break;
                case "timestamp":
                    returnValue = column + " " + type;
                    break;
                case "index":
                    //var indexType = type;
                    //string regEx = @"\[(.*?)\]";
                    //MatchCollection mc = Regex.Matches(line, regEx);
                    //var indexColumns = mc[0].Value;
                    //indexColumns = indexColumns.Replace("[","(").Replace("]",")");

                    //int uniqueIndex = allParts.IndexOf("unique:");
                    //if (uniqueIndex != -1)
                    //    indexType = "UNIQUE";

                    //int nameIndex = allParts.IndexOf("name:");
                    //string indexName = allParts[nameIndex + 1];
                    //indexName = indexName.Replace(",", "");

                    //returnValue = indexType + " " + indexName + " " + indexColumns;
                    returnValue = string.Empty;
                    break;
                default:
                    returnValue = column + " " + type;
                    break;
            }
            return returnValue;
        }
    }
}
