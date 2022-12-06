using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Engine
{
    public class CreateHTMLControlsEngine
    {
        List<string> ItemsList;
        string HTMLTemplate;
        string Placeholder;

        public CreateHTMLControlsEngine()
        {

        }

        public CreateHTMLControlsEngine(List<string> itemsList, string hTMLTemplate, string placeholder)
        {
            this.ItemsList = itemsList;
            this.HTMLTemplate = hTMLTemplate;
            this.Placeholder = placeholder;
        }

        public string Process()
        {
            StringBuilder sbResult = new StringBuilder();
            foreach (var item in ItemsList)
            {
                string propertyName = Convert.ToString(item);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    string result = HTMLTemplate.Replace(Placeholder.Trim() + "[U]", propertyName.ToUpper())
                        .Replace(Placeholder.Trim() + "[l]", propertyName.ToLower())
                        .Replace(Placeholder.Trim() + "[Uc]", propertyName.ToCamelCase())
                        .Replace(Placeholder.Trim() + "[Cc]", propertyName.ToCamelCase())
                        .Replace(Placeholder.Trim() + "[CcS]", propertyName.ToCamelCaseWithNewSeparator('_', ' '))
                        .Replace(Placeholder.Trim(), propertyName);
                    sbResult.AppendLine(result);
                }
            }
            return sbResult.ToString();
        }
    }
}
