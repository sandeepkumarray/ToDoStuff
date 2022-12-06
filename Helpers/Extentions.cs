using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToDoStuff
{
    public static class Extentions
    {
        public static string FirstCharToLower(this string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("input Is Null Or Empty");
            return input.First().ToString().ToLower() + input.Substring(1);
        }

        public static string CleanFileName(this string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[^\w\.@-]", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }
        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public static string RemoveSpecialCharactersExcludingSpace(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string ToCamelCase(this string input, char separator = '_')
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("input Is Null Or Empty");

            string[] values = input.Split(new char[] { separator });

            if(input.Contains(separator))
            {
                StringBuilder sb = new StringBuilder();
                foreach (string str in values)
                {
                    sb.Append(str.First().ToString().ToUpper() + str.Substring(1).ToLower());
                }

                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder(); 
                sb.Append(input.First().ToString().ToUpper() + input.Substring(1).ToLower());
                return sb.ToString();
            }

        }

        public static string ToCamelCaseWithSeparator(this string input, char separator = '_')
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("input Is Null Or Empty");

            string[] values = input.Split(new char[] { separator });

            String sb = "";

            string[] finalValues = new string[values.Length];
            int count = 0;
            foreach (string str in values)
            {
                finalValues[count] = (str.First().ToString().ToUpper() + str.Substring(1).ToLower());
                count++;
            }
            sb = string.Join(separator.ToString(), finalValues);
            return sb;

        }

        public static string ToCamelCaseWithNewSeparator(this string input, char separator = '_', char newseparator = ' ')
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("input Is Null Or Empty");

            string[] values = input.Split(new char[] { separator });

            String sb = "";

            string[] finalValues = new string[values.Length];
            int count = 0;
            foreach (string str in values)
            {
                finalValues[count] = (str.First().ToString().ToUpper() + str.Substring(1).ToLower());
                count++;
            }
            sb = string.Join(newseparator.ToString(), finalValues);
            return sb;

        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

        public static void ForEach<T>(this ObservableCollectionFast<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

        public static DataSet ConvertToDataSet<T>(this IEnumerable<T> source, string name)
        {
            if (source == null)
                throw new ArgumentNullException("source ");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            var converted = new DataSet(name);
            converted.Tables.Add(NewTable(name, source));
            return converted;
        }

        private static DataTable NewTable<T>(string name, IEnumerable<T> list)
        {
            PropertyInfo[] propInfo = typeof(T).GetProperties();
            DataTable table = Table<T>(name, list, propInfo);
            IEnumerator<T> enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
                table.Rows.Add(CreateRow<T>(table.NewRow(), enumerator.Current, propInfo));
            return table;
        }

        private static DataRow CreateRow<T>(DataRow row, T listItem, PropertyInfo[] pi)
        {
            foreach (PropertyInfo p in pi)
                row[p.Name.ToString()] = p.GetValue(listItem, null);
            return row;
        }

        private static DataTable Table<T>(string name, IEnumerable<T> list, PropertyInfo[] pi)
        {
            DataTable table = new DataTable(name);
            foreach (PropertyInfo p in pi)
                table.Columns.Add(p.Name, p.PropertyType);
            return table;
        }

        public static bool NotIn<T>(this T check, IEnumerable<T> list)
        {
            if (!list.Contains(check))
                return true;
            else
                return false;
        }
        public static bool In<T>(this T check, IEnumerable<T> list)
        {
            if (list.Contains(check))
                return true;
            else
                return false;
        }
        public static void AddAutoIncrmentId<T>(this IEnumerable<T> list, string propertyName)
        {
            int id = 1;
            
            foreach(var item in list)
            {
                SetPropertyValue(item, propertyName, id++);
            }

        }

        public static void SetPropertyValue<T>(T obj, string propertyName, object value)
        {
            // these should be cached if possible
            Type type = typeof(T);
            PropertyInfo pi = type.GetProperty(propertyName);

            pi.SetValue(obj, Convert.ChangeType(value, pi.PropertyType), null);
        }


        public static string GetDateString(this DateTime dateTime)
        {
            string value = "Today";

            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (dateTime.Date == DateTime.Today)
            {
                value = "Today";
            }
            else if (dateTime.Date == DateTime.Now.AddDays(1).Date)
            {
                value = "Tomorrow";
            }
            else if (dateTime.Date == DateTime.Now.AddDays(-1).Date)
            {
                value = "Yesterday";
            }
            else if (dateTime.Month == DateTime.Now.Month && dateTime.Year == DateTime.Now.Year)
            {
                value = "This Month";
                var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
                var d1 = dateTime.Date.AddDays(-1 * (int)cal.GetDayOfWeek(dateTime));
                var d2 = DateTime.Now.Date.AddDays(-1 * (int)cal.GetDayOfWeek(DateTime.Now));

                if (d1 == d2)
                    value = "This Week";
            }
            else if(dateTime.Year == DateTime.Now.Year)
            {
                value = "This Year";
            }

            return value;
        }
    }
}
