using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    public class CSharpClass
    {
        public ClassProperty ClassPropertyObj { get; set; }
        public bool IsXmlElement { get; set; }
        public bool IsXmlAttribute { get; set; }
        public bool IsXmlText { get; set; }
        public bool IsJsonProperty { get; set; }

        public bool AddGetterSetter { get; set; }
        public bool IsIncludetOnPropertyChanged { get; set; }
        public bool IsIncludePrivateField { get; set; }
        public string ClassName { get; set; }
        public string InheritedClass { get; set; }
        public ObservableCollection<ClassProperty> ClassProperties { get; set; }
        public CSharpClassFileSetting CSharpClassFileSettings { get; set; }

        public List<ClassMethodModel> ClassMethods { get; set; }

        public List<ClassMethodModel> Constructors { get; set; }

        public List<PropertyAttribute> UserDefinedPropertyAttributeList { get; set; }

        public CSharpClass()
        {
            ResetAllProperties();
            GetDefaultCSharpClassFileSettings();
        }

        public CSharpClass(string className) : this()
        {
            this.ClassName = className;
        }

        public CSharpClass(CSharpClassFileSetting ParamCSharpClassFileSettings)
        {
            CSharpClassFileSettings = ParamCSharpClassFileSettings;
        }

        public void UpdateClassProperties()
        {
            if (ClassProperties != null && ClassProperties.Count > 0)
            {
                ClassProperties.ForEach(prop => prop.IsXmlAttribute = IsXmlAttribute);
                ClassProperties.ForEach(prop => prop.IsXmlElement = IsXmlElement);
                ClassProperties.ForEach(prop => prop.IsXmlText = IsXmlText);
                ClassProperties.ForEach(prop => prop.IsJsonProperty = IsJsonProperty);

                ClassProperties.ForEach(prop => prop.AddGetterSetter = AddGetterSetter);
            }
        }

        public string GenerateCSharpClassData(bool IsUseDefaultSettings)
        {
            if (IsUseDefaultSettings == true)
                GetDefaultCSharpClassFileSettings();

            ClassName = CSharpClassFileSettings.IsClassNameCamelCasing ? ClassName.ToCamelCase(CSharpClassFileSettings.Separator) : ClassName;

            StringBuilder codeFileData = new StringBuilder();

            if (CSharpClassFileSettings.IsIncludeUsings)
            {
                codeFileData.AppendLine("using System;");
                codeFileData.AppendLine("using System.Collections.Generic;");
                codeFileData.AppendLine("using System.Linq;");
                codeFileData.AppendLine("using System.Text;");

                if (IsJsonProperty)
                {
                    codeFileData.AppendLine("using Newtonsoft.Json;");
                }

                if (CSharpClassFileSettings.IsSerializable || CSharpClassFileSettings.IsXmlRoot)
                    codeFileData.AppendLine("using System.Xml.Serialization;");

                if (CSharpClassFileSettings.UserDefinedUsings != null && CSharpClassFileSettings.UserDefinedUsings.Count > 0)
                {
                    foreach (string udUsing in CSharpClassFileSettings.UserDefinedUsings)
                    {
                        codeFileData.AppendLine(udUsing);
                    }
                }
                codeFileData.AppendLine("");
            }

            if (CSharpClassFileSettings.IsIncludeNameSpace)
            {
                codeFileData.AppendLine("namespace " + CSharpClassFileSettings.NameSpace);
                codeFileData.AppendLine("{");
            }

            if (CSharpClassFileSettings.IsSerializable)
            {
                codeFileData.AppendLine("\t[Serializable]");
            }

            if (CSharpClassFileSettings.IsXmlRoot)
            {
                codeFileData.AppendLine("\t[XmlRoot(ElementName = \"" + ClassName.ToLower() + "\")]");
            }

            if (CSharpClassFileSettings.Attributes != null)
            {
                foreach (var item in CSharpClassFileSettings.Attributes)
                    codeFileData.AppendLine("\t" + item);
            }

            if (IsIncludetOnPropertyChanged)
                codeFileData.AppendLine("\tpublic " + (CSharpClassFileSettings.IsInterface ? "interface" : "class") + " " + (CSharpClassFileSettings.IsInterface ? "I" : "") + ClassName + " : ViewModelBase");
            else if (!string.IsNullOrEmpty(InheritedClass))
                codeFileData.AppendLine("\tpublic " + (CSharpClassFileSettings.IsInterface ? "interface" : "class") + " " + (CSharpClassFileSettings.IsInterface ? "I" : "") + ClassName + " : " + InheritedClass);
            else
                codeFileData.AppendLine("\tpublic " + (CSharpClassFileSettings.IsInterface ? "interface" : "class") + " " + (CSharpClassFileSettings.IsInterface ? "I" : "") + ClassName + "");

            codeFileData.AppendLine("\t{");

            if (ClassProperties != null && ClassProperties.Count > 0)
            {
                if (IsIncludePrivateField)
                {
                    foreach (var prop in ClassProperties)
                    {
                        string proptype = prop.PropType;

                        if (prop.IsGenericListType)
                        {
                            if (!string.IsNullOrEmpty(prop.GenericListType))
                            {
                                proptype = prop.GenericListType + "<" + prop.PropType + ">";
                            }
                        }
                        codeFileData.AppendLine("\t\tprivate " + proptype + " _" + prop.PropName.FirstCharToLower() + ";");
                    }
                }

                foreach (ClassProperty prop in ClassProperties)
                {
                    string proptype = prop.PropType;

                    if (prop.IsGenericListType)
                    {
                        if (!string.IsNullOrEmpty(prop.GenericListType))
                        {
                            proptype = prop.GenericListType + "<" + prop.PropType + ">";
                        }
                    }
                    if (IsIncludePrivateField)
                    {

                        if (prop.IsXmlAttribute)
                        {
                            codeFileData.AppendLine("\t\t[XmlAttribute(AttributeName = \"" + prop.PropName.ToLower() + "\")]");
                        }
                        if (prop.IsXmlElement)
                        {
                            codeFileData.AppendLine("\t\t[XmlElement(ElementName = \"" + prop.PropName.ToLower() + "\")]");
                        }
                        if (prop.IsXmlText)
                        {
                            codeFileData.AppendLine("\t\t[XmlText]");
                        }
                        if (UserDefinedPropertyAttributeList != null && UserDefinedPropertyAttributeList.Count > 0)
                        {
                            foreach (var att in UserDefinedPropertyAttributeList)
                            {
                                att.AttributeValue = prop.PropName.ToCamelCaseWithNewSeparator();
                                codeFileData.AppendLine(att.ToString());
                            }
                        }

                        codeFileData.AppendLine("\t\tpublic " + proptype + " " + prop.PropName);
                        codeFileData.AppendLine("\t\t{");
                        codeFileData.AppendLine("\t\t\tget { return _" + prop.PropName.FirstCharToLower() + "; }");
                        codeFileData.AppendLine("\t\t\tset");
                        codeFileData.AppendLine("\t\t\t{");
                        codeFileData.AppendLine("\t\t\t\t_" + prop.PropName.FirstCharToLower() + " = value;");
                        if (IsIncludetOnPropertyChanged)
                            codeFileData.AppendLine("\t\t\t\tOnPropertyChanged(\"" + prop.PropName + "\");");

                        codeFileData.AppendLine("\t\t\t}");
                        codeFileData.AppendLine("\t\t}");
                        codeFileData.AppendLine("");
                    }
                    else
                    {
                        if (prop.IsXmlAttribute)
                        {
                            codeFileData.AppendLine("\t\t[XmlAttribute(AttributeName = \"" + prop.PropName.ToLower() + "\")]");
                        }
                        if (prop.IsXmlElement)
                        {
                            codeFileData.AppendLine("\t\t[XmlElement(ElementName = \"" + prop.PropName.ToLower() + "\")]");
                        }
                        if (prop.IsXmlText)
                        {
                            codeFileData.AppendLine("\t\t[XmlText]");
                        }
                        if (prop.IsJsonProperty)
                        {
                            codeFileData.AppendLine("\t\t[JsonProperty(\"" + prop.PropName.ToLower() + "\")]");
                        }

                        if (UserDefinedPropertyAttributeList != null && UserDefinedPropertyAttributeList.Count > 0)
                        {
                            foreach (var att in UserDefinedPropertyAttributeList)
                            {
                                att.AttributeValue = prop.PropName.ToCamelCaseWithNewSeparator();
                                codeFileData.AppendLine(att.ToString());
                            }
                        }
                        codeFileData.AppendLine("\t\tpublic " + proptype + " " + prop.PropName + (prop.AddGetterSetter ? " { get; set; }" : ";"));
                    }
                    codeFileData.AppendLine("");
                }
            }

            codeFileData.AppendLine("");

            if (CSharpClassFileSettings.IsIncludeDefaultConstructor)
            {
                codeFileData.AppendLine("\t\tpublic " + ClassName + "()");
                codeFileData.AppendLine("\t\t{");
                codeFileData.AppendLine("\t\t}");
                codeFileData.AppendLine("");
            }

            if (CSharpClassFileSettings.IsIncludeParametrizedConstructor)
            {
                if (CSharpClassFileSettings.Parameters.Count > 0)
                {
                    string parameters = string.Empty;
                    IEnumerable<string> paramList = from p in CSharpClassFileSettings.Parameters
                                                    select p.PropType + " " + p.PropName;

                    parameters = String.Join(",", paramList);

                    codeFileData.AppendLine("\t\tpublic " + ClassName + "(" + parameters + ")");
                    codeFileData.AppendLine("\t\t{");
                    codeFileData.AppendLine("\t\t\t" + CSharpClassFileSettings.ParameterizedConstructorContent);
                    codeFileData.AppendLine("\t\t}");
                    codeFileData.AppendLine("");
                }
            }


            if (Constructors != null && Constructors.Count > 0)
            {
                foreach (var cons in Constructors)
                {
                    codeFileData.AppendLine("\t\t" + cons.AccessType + (string.IsNullOrEmpty(cons.AccessType) ? "" : " ") +
                        cons.MethodType + (string.IsNullOrEmpty(cons.MethodType) ? "" : " ") +
                        cons.ReturnType + " " + cons.MethodName + "(" + GetParametersString(cons.Parameters) + ")");

                    if (!CSharpClassFileSettings.IsInterface)
                    {
                        codeFileData.AppendLine("\t\t{");
                        codeFileData.AppendLine("" + cons.MethodBody);
                        codeFileData.AppendLine("\t\t}");
                    }
                    codeFileData.AppendLine("");
                }
            }



            if (ClassMethods != null && ClassMethods.Count > 0)
            {
                foreach (var meth in ClassMethods)
                {
                    if (!string.IsNullOrEmpty(meth.LeadingLine))
                        codeFileData.AppendLine("\t\t" + meth.LeadingLine);

                    if (meth.Attributes != null)
                    {
                        foreach (var item in meth.Attributes)
                            codeFileData.AppendLine("\t\t" + item);
                    }

                    codeFileData.AppendLine("\t\t" + meth.AccessType + (string.IsNullOrEmpty(meth.AccessType) ? "" : " ") +
                        meth.MethodType + (string.IsNullOrEmpty(meth.MethodType) ? "" : " ") +
                        meth.ReturnType + " " + meth.MethodName + "(" + GetParametersString(meth.Parameters) + ")" + (CSharpClassFileSettings.IsInterface ? ";" : ""));

                    if (!CSharpClassFileSettings.IsInterface)
                    {
                        codeFileData.AppendLine("\t\t{");
                        codeFileData.AppendLine("" + meth.MethodBody);
                        codeFileData.AppendLine("\t\t}");
                    }
                    if (!string.IsNullOrEmpty(meth.TrailingLine))
                        codeFileData.AppendLine("\t\t" + meth.TrailingLine);
                    codeFileData.AppendLine("");
                }
            }

            if (CSharpClassFileSettings.IsAdditionalCodeSnippet)
            {
                codeFileData.AppendLine("\t\t" + CSharpClassFileSettings.AdditionalCodeSnippet);
                codeFileData.AppendLine("");
            }

            codeFileData.AppendLine("\t}");

            if (CSharpClassFileSettings.IsIncludeNameSpace)
            {
                codeFileData.AppendLine("}");
            }

            return codeFileData.ToString();
        }

        private string GetParametersString(List<ClassProperty> parameters)
        {
            string return_value = string.Empty;
            List<string> propList = new List<string>();

            if (parameters != null)
            {
                foreach (var par in parameters)
                {
                    propList.Add(par.PropType + " " + par.PropName);
                }
                return_value = string.Join(",", propList);
            }
            return return_value;
        }

        public void GenerateCSharpClassFile(bool IsUseDefaultSettings)
        {
            string codeFileData = GenerateCSharpClassData(IsUseDefaultSettings);

            string filePath = Path.Combine(CSharpClassFileSettings.CSharpFilePath, ClassName);
            filePath = filePath + "." + CSharpClassFileSettings.CSharpFileExtension;
            File.WriteAllText(filePath, codeFileData);
        }

        public void ResetAllProperties()
        {
            IsIncludetOnPropertyChanged = false;
            IsIncludePrivateField = false;
            ClassName = default(string);
            ClassProperties = new ObservableCollection<ClassProperty>();
        }

        private void GetDefaultCSharpClassFileSettings()
        {
            this.CSharpClassFileSettings = new CSharpClassFileSetting();
        }

        private void AddClassPropertyCommandMethod()
        {
            ClassProperties.Add(ClassPropertyObj);
        }
    }

    public class ClassMethodModel
    {
        public string TableName { get; set; }
        public string AccessType { get; set; }
        public string ReturnType { get; set; }
        public string MethodType { get; set; }
        public string MethodName { get; set; }
        public List<ClassProperty> Parameters { get; set; }
        public ObservableCollection<ClassProperty> ClassProperties { get; set; }
        public List<string> Attributes { get; set; }
        public string MethodBody { get; set; }
        public bool IsForInterface { get; set; }
        public string LeadingLine { get; set; }
        public string TrailingLine { get; set; }

        public ClassMethodModel()
        {
            Attributes = new List<string>();
            Parameters = new List<ClassProperty>();
        }
        public ClassMethodModel(string accessType, string returnType, string methodType, string methodName) : this()
        {
            this.AccessType = accessType;
            this.ReturnType = returnType;
            this.MethodType = methodType;
            this.MethodName = methodName;
        }
        public virtual ClassMethodModel Initialize()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new Exception("TableName is null");

            return this;
        }
        public string CreateDataSetToModel()
        {
            string Template = "\t\t\t\t\t[Table_Name].[COLUMN_NAME] = dr[\"[DB_COLUMN_NAME]\"] == DBNull.Value ? default([DATA_TYPE]) : Convert.To[DATA_TYPE](dr[\"[DB_COLUMN_NAME]\"]);";

            string TemplateBytes = "\t\t\t\t\t[Table_Name].[COLUMN_NAME] = dr[\"[DB_COLUMN_NAME]\"] == DBNull.Value ? default([DATA_TYPE]) : GetStringDataFromByteArray((byte[])(dr[\"[DB_COLUMN_NAME]\"]));";

            StringBuilder sb = new StringBuilder();
            foreach (var item in ClassProperties)
            {
                if (item.DBDataType.In(new List<string>() { "binary", "varbinary", "tinyblob", "blob", "mediumblob", "longblob", "char byte" }))
                    sb.AppendLine(TemplateBytes.Replace("[Table_Name]", TableName.ToLower()).Replace("[COLUMN_NAME]", item.PropName.RemoveSpecialCharacters()).Replace("[DATA_TYPE]", item.PropType).Replace("[DB_COLUMN_NAME]", item.PropName));
                else
                    sb.AppendLine(Template.Replace("[Table_Name]", TableName.ToLower()).Replace("[COLUMN_NAME]", item.PropName.RemoveSpecialCharacters()).Replace("[DATA_TYPE]", item.PropType).Replace("[DB_COLUMN_NAME]", item.PropName));
            }

            return sb.ToString();
        }
    }

    public class PropertyAttribute
    {
        public string AttributeName { get; set; }
        public string AttributeProperty { get; set; }
        public string AttributeValue { get; set; }

        public PropertyAttribute(string attributename, string attributeproperty, string attributevalue)
        {
            AttributeName = attributename;
            AttributeProperty = attributeproperty;
            AttributeValue = attributevalue;

        }

        public override string ToString()
        {
            string returnValue = string.Empty;
            returnValue = "\t\t[" + AttributeName + "(" + (String.IsNullOrEmpty(AttributeProperty) ? "" : AttributeProperty + " = ") + "\"" + AttributeValue + "\")]";
            return returnValue;
        }
    }
}
