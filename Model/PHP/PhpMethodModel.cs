using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model.PHP
{
    public class PhpMethodModel
    {
        public string TableName { get; set; }
        public string MethodName { get; set; }
        public List<ClassProperty> Parameters { get; set; }

        public List<string> blockColumnList = new List<string>() { "id", "created_at", "updated_at", "user_id", "archived_at", "deleted_at" };
        public List<string> globalColumnList = new List<string>() { "user_id" };
        public string[] intList = new string[] { "boolean", "tinyint", "number", "int", "integer", "smallint unsigned", "mediumint", "bigint", "int unsigned", "integer unsigned", "bit" };
        public TypeOfSQLObject TypeOfSQLObject { get; set; }

        public PhpMethodModel(string MethodName, string TableName, List<ClassProperty> Parameters, TypeOfSQLObject typeOfSQLObject = TypeOfSQLObject.Insert)
        {
            this.MethodName = MethodName;
            this.TableName = TableName;
            this.Parameters = Parameters;
            this.TypeOfSQLObject = typeOfSQLObject;
        }

        public override string ToString()
        {
            return "Not Implemented";
        }
        public string GenerateColumnsForQuery(List<ClassProperty> Parameters, TypeOfQuery typeOfQuery)
        {
            StringBuilder content = new StringBuilder();

            switch (typeOfQuery)
            {
                case TypeOfQuery.Set:
                    {
                        var intCols = from n in Parameters
                                      where n.PropType.In(intList) && n.PropName.NotIn(blockColumnList)
                                      select n.PropName;
                        var stringCols = from n in Parameters
                                         where n.PropType.NotIn(intList) && n.PropName.NotIn(blockColumnList)
                                         select n.PropName;

                        var globalCols = from n in Parameters
                                         where n.PropName.In(globalColumnList)
                                         select n.PropName;

                        foreach (var col in intCols)
                        {
                            content.AppendLine("\t$" + col + " = $data->" + col + " == null ? \"NULL\" : $data->" + col + "; ");
                        }

                        foreach (var col in stringCols)
                        {
                            content.AppendLine("\t$" + col + " = $data->" + col + " == null ? NULL : $data->" + col + "; ");
                        }

                        foreach (var col in globalCols)
                        {
                            content.AppendLine("\t$" + col + " = $data->" + col + " == null ? \"NULL\" : $data->" + col + "; ");
                        }
                    }
                    //foreach (var col in Parameters)
                    //{
                    //    if (col.PropName.NotIn(this.blockColumnList))
                    //        content.AppendLine("\t$" + col.PropName + " = $data->" + col.PropName + " == null ? NULL : $data->"+ col.PropName + "; ");

                    //    if (col.PropName.In(this.globalColumnList))
                    //        content.AppendLine("\t$" + col.PropName + " = $data->" + col.PropName + ";");
                    //}
                    break;
                case TypeOfQuery.Insert:
                    {
                        var intCols = from n in Parameters
                                      where n.PropType.In(intList) && n.PropName.NotIn(blockColumnList)
                                      select "$" + n.PropName;
                        var stringCols = from n in Parameters
                                         where n.PropType.NotIn(intList) && n.PropName.NotIn(blockColumnList)
                                         select "'$" + n.PropName + "'";

                        var globalCols = from n in Parameters
                                      where n.PropName.In(globalColumnList)
                                      select "$" + n.PropName;

                        content.Append(string.Join(",", intCols.ToArray().Concat(stringCols).Concat(globalCols)));
                        break;
                    }
                case TypeOfQuery.InsertColumns:
                    {
                        var intCols = from n in Parameters
                                      where n.PropType.In(intList) && n.PropName.NotIn(blockColumnList)
                                      select n.PropName;
                        var stringCols = from n in Parameters
                                         where n.PropType.NotIn(intList) && n.PropName.NotIn(blockColumnList)
                                         select n.PropName;

                        var globalCols = from n in Parameters
                                         where n.PropName.In(globalColumnList)
                                         select n.PropName;

                        content.Append(string.Join(",", intCols.Concat(stringCols).Concat(globalCols)));
                        break;
                    }
                case TypeOfQuery.Update:

                    string updateIntColumns = string.Join(",", from n in Parameters
                                                               where n.PropType.In(intList) && n.PropName.NotIn(blockColumnList)
                                                               select n.PropName + " = $" + n.PropName
                                                            );
                    string updateStringColumns = string.Join(",", from n in Parameters
                                                                  where n.PropType.NotIn(intList) && n.PropName.NotIn(blockColumnList)
                                                                  select n.PropName + " = '$" + n.PropName + "'"
                                                            );
                    content.AppendLine(updateIntColumns);
                    content.Append("," + updateStringColumns);
                    break;
                default:
                    break;
            }
            return content.ToString();
        }

        public string EvaluateWhereCondition(List<string> columnsList)
        {
            StringBuilder content = new StringBuilder();
            List<string> SelectedColumnsList = new List<string>();

            foreach (var c in columnsList)
            {
                if (Parameters.Find(p => p.PropName.ToLower() == c) != null)
                {
                    SelectedColumnsList.Add(c);
                }
            }

            if (SelectedColumnsList.Any())
            {
                content.Append("Where ");
                content.Append(string.Join(" and ", from n in SelectedColumnsList
                                                    select n + " = '$" + n + "'"));
            }

            return content.ToString();
        }


        public string EvaluateGETParamas(List<string> columnsList)
        {
            StringBuilder content = new StringBuilder();
            List<string> SelectedColumnsList = new List<string>();

            foreach (var c in columnsList)
            {
                if (Parameters.Find(p => p.PropName.ToLower() == c) != null)
                {
                    content.AppendLine("    $" + c + " = $_GET['" + c + "'];");
                    SelectedColumnsList.Add(c);
                }
            }

            if (SelectedColumnsList.Any())
            {
            }

            return content.ToString();
        }
    }

    public enum TypeOfQuery
    {
        Update,
        Insert,
        InsertColumns,
        Set,
        Select,
        SelectAll
    }
    public enum TypeOfSQLObject
    {
        Insert,
        Delete,
        Update,
        Select,
        SelectAll,
        InsertProcedure,
        DeleteProcedure,
        UpdateProcedure,
        SelectProcedure,
        SelectAllProcedure
    }
}
