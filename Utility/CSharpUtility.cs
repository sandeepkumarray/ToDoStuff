using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Model;

namespace ToDoStuff
{
    public class CSharpUtility
    {
        CSharpClass cSharpClass { get; set; }
        public void CreateCSharpClass(string ClassName, List<ClassProperty> ClassProperties)
        {
            cSharpClass = new CSharpClass();
            cSharpClass.ClassName = ClassName;
            cSharpClass.ClassProperties = new ObservableCollection<ClassProperty>(ClassProperties);
            cSharpClass.GenerateCSharpClassFile(true);
        }
    }
}
