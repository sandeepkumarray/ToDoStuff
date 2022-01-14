using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Model;

namespace ToDoStuff
{
    public class CSharpClassFileSetting
    {
        #region Properties
        public bool IsIncludeUsings { get; set; }
        public bool IsIncludeNameSpace { get; set; }
        public string NameSpace { get; set; }
        public bool IsSerializable { get; set; }
        public bool IsInterface { get; set; }
        public bool IsXmlRoot { get; set; }
        public bool IsIncludeDefaultConstructor { get; set; }
        public bool IsIncludeParametrizedConstructor { get; set; }
        public ObservableCollection<ClassProperty> Parameters { get; set; }
        public string CSharpFilePath { get; set; }
        public string CSharpFileExtension { get; set; }
        public bool IsAdditionalCodeSnippet { get; set; }
        public string AdditionalCodeSnippet { get; set; }
        public string ParameterizedConstructorContent { get; set; }
        public bool IsClassNameCamelCasing { get; set; }
        public char Separator { get; set; }
        public ObservableCollection<string> UserDefinedUsings { get; set; }
        #endregion

        public List<string> Attributes { get; set; }
        public CSharpClassFileSetting()
        {
            Parameters = new ObservableCollection<ClassProperty>();
            UserDefinedUsings = new ObservableCollection<string>();
            Attributes = new List<string>();
            CSharpFilePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            CSharpFileExtension = "cs";
            Separator = '_';
        }
    }
}
