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
        public string ClassName { get; set; }
        public string ClassFileName { get; set; }
        public string InheritedClass { get; set; }
        public bool IsIncludeUsings { get; set; }
        public bool IsIncludeNameSpace { get; set; }
        public string NameSpace { get; set; }
        public bool IsSerializable { get; set; }
        public bool IsInterface { get; set; }
        public bool IsXmlRoot { get; set; }
        public bool IsIncludeDefaultConstructor { get; set; }
        public bool IsIncludeParametrizedConstructor { get; set; }
        public ObservableCollectionFast<ClassProperty> Parameters { get; set; }
        public string CSharpFilePath { get; set; }
        public string CSharpFileExtension { get; set; }
        public bool IsAdditionalCodeSnippet { get; set; }
        public string AdditionalCodeSnippet { get; set; }
        public string ParameterizedConstructorContent { get; set; }
        public bool IsClassNameCamelCasing { get; set; }
        public char Separator { get; set; }
        public ObservableCollectionFast<string> UserDefinedUsings { get; set; }
        public List<ClassMethodModel> ClassMethods { get; set; }
        #endregion

        public List<string> Attributes { get; set; }
        public CSharpClassFileSetting()
        {
            Parameters = new ObservableCollectionFast<ClassProperty>();
            UserDefinedUsings = new ObservableCollectionFast<string>();
            Attributes = new List<string>();
            CSharpFilePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            CSharpFileExtension = "cs";
            Separator = '_';
        }
    }
}
