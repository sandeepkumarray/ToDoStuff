using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model.Angular
{
    public class AngularClassModel : CSharpClass
    {
        public AngularClassFileSetting AngularClassFileSetting { get; set; }

        public AngularClassModel()
        {
            AngularClassFileSetting = new AngularClassFileSetting();
        }

        public AngularClassModel(AngularClassFileSetting angularClassFileSetting)
        {
            AngularClassFileSetting = angularClassFileSetting;
        }

        public override string GenerateCSharpClassData(bool IsUseDefaultSettings)
        {
            StringBuilder codeFileData = new StringBuilder();

            if (AngularClassFileSetting.IsIncludeUsings)
            {
                codeFileData.AppendLine("import { isDevMode } from '@angular/core';");
                codeFileData.AppendLine("import { Observable } from 'rxjs';");
                codeFileData.AppendLine("import { delay, map } from 'rxjs/operators';");
                codeFileData.AppendLine("import { environment } from '../../environments/environment';");
                codeFileData.AppendLine("import { Router } from '@angular/router';");
                codeFileData.AppendLine("import { JsonPipe } from '@angular/common';");
                codeFileData.AppendLine("import { ResponseModel } from '../model/ResponseModel'");
            }

            if (this.AngularClassFileSetting.AngularClassType == AngularClassType.Module)
            {
                codeFileData.AppendLine("import { NgModule } from '@angular/core';");
                codeFileData.AppendLine("import { CommonModule } from '@angular/common';");
            }

            if (this.AngularClassFileSetting.AngularClassType == AngularClassType.RoutingModule)
            {
                codeFileData.AppendLine("import { NgModule } from '@angular/core';");
                codeFileData.AppendLine("import { RouterModule, Routes } from '@angular/router';");
                codeFileData.AppendLine("import { AuthGuard } from 'src/app/utility/AuthGuard';");
            }

            if (this.AngularClassFileSetting.AngularClassType == AngularClassType.Component)
            {
                codeFileData.AppendLine("import { Component, OnInit } from '@angular/core';");
            }

            if (this.AngularClassFileSetting.AngularClassType == AngularClassType.Service)
            {
                codeFileData.AppendLine("import { Injectable } from '@angular/core';");
                codeFileData.AppendLine("import { HttpClient, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';");
            }

            if (AngularClassFileSetting.UserDefinedUsings != null && AngularClassFileSetting.UserDefinedUsings.Count > 0)
            {
                foreach (string udUsing in AngularClassFileSetting.UserDefinedUsings)
                {
                    codeFileData.AppendLine(udUsing);
                }
            }

            if (this.AngularClassFileSetting.AngularClassType == AngularClassType.Component)
            {
                var component = this.AngularClassFileSetting.Component;
                codeFileData.AppendLine("");
                codeFileData.AppendLine("@Component({");
                codeFileData.AppendLine("  selector: '" + component.selector + "',");
                codeFileData.AppendLine("  templateUrl: '" + component.templateUrl + "',");
                codeFileData.AppendLine("  styleUrls: ['" + component.styleUrls + "']");
                codeFileData.AppendLine("})");
            }

            if (this.AngularClassFileSetting.AngularClassType == AngularClassType.Service)
            {
                codeFileData.AppendLine("");
                codeFileData.AppendLine("@Injectable({");
                codeFileData.AppendLine("providedIn: 'root'");
                codeFileData.AppendLine("})");
            }

            if (this.AngularClassFileSetting.AngularClassType == AngularClassType.Module)
            {
                codeFileData.AppendLine("");
                codeFileData.AppendLine("");
                codeFileData.AppendLine("");
                codeFileData.AppendLine("@NgModule({");
                codeFileData.AppendLine("  declarations: [");
                //declarations
                if (this.AngularClassFileSetting.Declarations != null && this.AngularClassFileSetting.Declarations.Count > 0)
                {
                    foreach (var dec in this.AngularClassFileSetting.Declarations)
                    {
                        codeFileData.AppendLine("    " + dec + ",");
                    }
                }
                //codeFileData.AppendLine("    UniversesComponent,");
                //codeFileData.AppendLine("    ViewUniversesComponent,");
                //codeFileData.AppendLine("    EditUniversesComponent");
                codeFileData.AppendLine("  ],");
                codeFileData.AppendLine("  imports: [");
                codeFileData.AppendLine("    CommonModule,");
                //imports
                if (this.AngularClassFileSetting.Imports != null && this.AngularClassFileSetting.Imports.Count > 0)
                {
                    foreach (var imp in this.AngularClassFileSetting.Imports)
                    {
                        codeFileData.AppendLine("    " + imp + ",");
                    }
                }
                //codeFileData.AppendLine("    UniversesRoutingModule");
                codeFileData.AppendLine("  ]");
                codeFileData.AppendLine("})");
            }

            if (this.AngularClassFileSetting.AngularClassType == AngularClassType.RoutingModule)
            {
                if (this.AngularClassFileSetting.AngularRoute != null)
                {
                    var routes = this.AngularClassFileSetting.AngularRoute;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("");
                    sb.AppendLine("const routes: Routes = [");
                    sb.AppendLine("  {");

                    foreach (var route in routes.Routes)
                    {
                        sb.AppendLine("    path: '" + route.Path + "',");

                        if (!string.IsNullOrEmpty(route.Component))
                            sb.AppendLine("    component: " + route.Component + ",");

                        if (!string.IsNullOrEmpty(route.RedirectTo))
                            sb.AppendLine("    redirectTo: " + route.RedirectTo + ",");

                        if (!string.IsNullOrEmpty(route.PathMatch))
                            sb.AppendLine("    pathMatch: " + route.PathMatch + ",");

                        if (route.Data != null)
                        {
                            sb.AppendLine("    data: {");
                            sb.AppendLine("      title: '" + route.Data.Title + "',");
                            sb.AppendLine("    },");
                        }

                        if (route.Children != null)
                        {
                            sb.AppendLine("    children: [");
                            foreach (var child in route.Children)
                            {
                                sb.AppendLine("      {");
                                sb.AppendLine("        path: '" + child.Path + "',");

                                if (!string.IsNullOrEmpty(child.Component))
                                    sb.AppendLine("        component: " + child.Component + ",");

                                if (!string.IsNullOrEmpty(child.LoadChildren))
                                    sb.AppendLine("        loadChildren: () => " + child.LoadChildren + ",");

                                sb.AppendLine("        data: {");
                                sb.AppendLine("          title: '" + child.Data.Title + "',");
                                sb.AppendLine("        },");
                                sb.AppendLine("        canActivate: " + child.CanActivate);
                                sb.AppendLine("      },");
                            }
                        }
                        sb.AppendLine("    ]");
                    }

                    sb.AppendLine("  },");
                    sb.AppendLine("];");
                    codeFileData.AppendLine(sb.ToString());
                }

                codeFileData.AppendLine("");
                codeFileData.AppendLine("@NgModule({");
                codeFileData.AppendLine("  imports: [RouterModule.forChild(routes)],");
                codeFileData.AppendLine("  exports: [RouterModule],");
                codeFileData.AppendLine("})");
            }

            if (!string.IsNullOrEmpty(InheritedClass))
                codeFileData.Append("export " + (AngularClassFileSetting.IsInterface ? "interface" : "class") + " " + (AngularClassFileSetting.IsInterface ? "I" : "") + ClassName + (AngularClassFileSetting.IsInterface ? " implements " : " extends ") + InheritedClass);
            else
                codeFileData.Append("export " + (AngularClassFileSetting.IsInterface ? "interface" : "class") + " " + (AngularClassFileSetting.IsInterface ? "I" : "") + ClassName + "");

            codeFileData.AppendLine(" {");
            codeFileData.AppendLine("");

            //Properties
            if (ClassProperties != null && ClassProperties.Count > 0)
            {
                foreach (ClassProperty prop in ClassProperties)
                {
                    codeFileData.AppendLine("\t\t" + prop.AccessType + " " + prop.PropName + "!: " + (prop.PropType == null ? "string" : prop.PropType) + ";");
                }
            }

            //Constructor
            if (AngularClassFileSetting.IsIncludeDefaultConstructor)
            {
                codeFileData.AppendLine("\tconstructor" + "() { }");
            }

            if (AngularClassFileSetting.IsIncludeParametrizedConstructor)
            {
                if (AngularClassFileSetting.Parameters.Count > 0)
                {
                    string parameters = string.Empty;
                    IEnumerable<string> paramList = from p in AngularClassFileSetting.Parameters
                                                    select "private " + p.PropName + ": " + p.PropType;

                    parameters = String.Join(",", paramList);

                    codeFileData.AppendLine("\tconstructor" + "(" + parameters + ") {");
                    codeFileData.AppendLine(AngularClassFileSetting.ParameterizedConstructorContent);
                    codeFileData.AppendLine("\t}");
                    codeFileData.AppendLine("");
                }
            }

            //Functions
            if (ClassMethods != null && ClassMethods.Count > 0)
            {
                foreach (var meth in ClassMethods)
                {
                    if (!string.IsNullOrEmpty(meth.LeadingLine))
                        codeFileData.AppendLine("\t" + meth.LeadingLine);

                    codeFileData.AppendLine("\t" + meth.MethodName + "(" + GetParametersString(meth.Parameters) + "): " + meth.ReturnType + " {");

                    codeFileData.AppendLine("" + meth.MethodBody);
                    codeFileData.AppendLine("\t}");

                    if (!string.IsNullOrEmpty(meth.TrailingLine))
                        codeFileData.AppendLine("\t" + meth.TrailingLine);
                    codeFileData.AppendLine("");
                }
            }

            codeFileData.AppendLine("}");
            return codeFileData.ToString();
        }

        public override string GetParametersString(List<ClassProperty> parameters)
        {
            string return_value = string.Empty;
            List<string> propList = new List<string>();

            if (parameters != null)
            {
                foreach (var par in parameters)
                {
                    propList.Add(par.PropName + ": " + par.PropType);
                }
                return_value = string.Join(",", propList);
            }
            return return_value;
        }
    }

    public class AngularClassFileSetting : CSharpClassFileSetting
    {
        public AngularClassType AngularClassType { get; set; }
        public List<string> Declarations { get; set; }
        public List<string> Imports { get; set; }
        public AngularRoute AngularRoute { get; set; }
        public AngularComponent Component { get; set; }


        public AngularClassFileSetting()
        {
            Parameters = new ObservableCollectionFast<ClassProperty>();
            UserDefinedUsings = new ObservableCollectionFast<string>();
            Attributes = new List<string>();
            CSharpFilePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            CSharpFileExtension = "ts";
            Separator = '_';
        }
    }

    public class AngularClassDataSetting
    {
        public ObservableCollectionFast<ClassProperty> ClassProperties { get; set; }
        public string APIName { get; set; }
    }

    public enum AngularClassType
    {
        Component,
        Service,
        RoutingModule,
        Module,
        Model,
        CSS,
        SCSS,
        Html,
        ts
    }

    public enum AngularComponentType
    {
        ModuleComponent,
        Component,
        ViewComponent,
        EditComponent
    }

    public class AngularComponent
    {
        public string selector { get; set; }
        public string templateUrl { get; set; }
        public string styleUrls { get; set; }

    }

    public class Route
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("component")]
        public string Component { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("canActivate")]
        public string CanActivate { get; set; }

        [JsonProperty("children")]
        public List<Route> Children { get; set; }

        [JsonProperty("loadChildren")]
        public string LoadChildren { get; set; }

        [JsonProperty("redirectTo")]
        public string RedirectTo { get; set; }

        [JsonProperty("pathMatch")]
        public string PathMatch { get; set; }
    }

    public class Data
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class AngularRoute
    {
        [JsonProperty("Routes")]
        public List<Route> Routes { get; set; }

        public AngularRoute()
        {
            Routes = new List<Route>();
        }
    }


}
