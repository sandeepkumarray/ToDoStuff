using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoStuff.Helpers;
using ToDoStuff.Model;
using ToDoStuff.Model.Angular;

namespace ToDoStuff
{
    public class AngularRoutine
    {
        public DataStore DataStore { get; set; }
        public string RootFilePath { get; set; }

        public AngularPackageStructure packageStructure = null;

        public AngularRoutine()
        {

        }

        public AngularRoutine(string packageJson)
        {
            this.packageStructure = AngularPackageStructure.FromJson(packageJson);
        }

        public AngularRoutine SetDataStore(DataStore dataStore)
        {
            this.DataStore = dataStore;
            RootFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                     DataStore.Database.Trim(), "Angular");
            return this;
        }

        public AngularRoutine Initialize()
        {
            if (this.packageStructure == null)
            {
                CreatePackageStructure();
                string packageJson = JsonConvert.SerializeObject(this.packageStructure);
            }
            return this;
        }

        private void CreatePackageStructure()
        {
            this.packageStructure = new AngularPackageStructure();
            this.packageStructure.PackageName = DataStore.Database;

            foreach (var table in DataStore.Tables)
            {
                AngularModule module = new AngularModule(table.TableName.ToLower() + ".module", AngularClassType.Module);
                module.ModuleFolderName = table.TableName.ToLower();
                module.ModuleFolderPath = "src\\app\\views\\content";
                module.FileContents = new AngularFileService(table).GenerateFromTemplateFile("{name}.module.ts");
                module.Components = GetModuleComponents(table, module);
                this.packageStructure.Modules.Add(module);

                AngularModule routingModule = new AngularModule(table.TableName.ToLower() + "-routing.module", AngularClassType.RoutingModule);
                routingModule.ModuleFolderName = table.TableName.ToLower();
                routingModule.ModuleFolderPath = "src\\app\\views\\content";
                routingModule.FileContents = new AngularFileService(table).GenerateFromTemplateFile("{name}-routing.module.ts");
                this.packageStructure.Modules.Add(routingModule);
            }
        }

        private List<AngularModuleComponent> GetModuleComponents(DataStoreTable table, AngularModule module)
        {
            List<AngularModuleComponent> angularModuleComponents = new List<AngularModuleComponent>();
            //main component
            {
                AngularModuleComponent mainComponent = new AngularModuleComponent(table.TableName.ToLower(), AngularComponentType.ModuleComponent);
                mainComponent.ComponentFolderName = table.TableName.ToLower();
                mainComponent.ComponentFolderPath = Path.Combine(module.ModuleFolderPath, module.ModuleFolderName);

                AngularFile styleFile = new AngularFile(table.TableName.ToLower() + ".component", AngularClassType.CSS);
                AngularFile htmlFile = new AngularFile(table.TableName.ToLower() + ".component", AngularClassType.Html);
                htmlFile.FileContents = new AngularFileService(table).GenerateFromTemplateFile("{name}.component.html");
                AngularFile tsFile = new AngularFile(table.TableName.ToLower() + ".component", AngularClassType.ts);
                tsFile.FileContents = new AngularFileService(table).GenerateFromTemplateFile("{name}.component.ts");
                mainComponent.Files = new List<AngularFile>() { styleFile, htmlFile, tsFile };
                angularModuleComponents.Add(mainComponent);
            }

            //edit component
            {
                AngularModuleComponent editComponent = new AngularModuleComponent("edit-" + table.TableName.ToLower(), AngularComponentType.EditComponent);
                editComponent.ComponentFolderName = "edit-" + table.TableName.ToLower();
                editComponent.ComponentFolderPath = Path.Combine(module.ModuleFolderPath, module.ModuleFolderName, editComponent.ComponentFolderName);

                AngularFile styleFile = new AngularFile("edit-" + table.TableName.ToLower() + ".component", AngularClassType.SCSS);
                AngularFile htmlFile = new AngularFile("edit-" + table.TableName.ToLower() + ".component", AngularClassType.Html);
                htmlFile.FileContents = new AngularFileService(table).GenerateFromTemplateFile("edit-{name}.component.html");
                AngularFile tsFile = new AngularFile("edit-" + table.TableName.ToLower() + ".component", AngularClassType.ts);
                tsFile.FileContents = new AngularFileService(table).GenerateFromTemplateFile("edit-{name}.component.ts");
                editComponent.Files = new List<AngularFile>() { styleFile, htmlFile, tsFile };
                angularModuleComponents.Add(editComponent);
            }

            //view component
            {
                AngularModuleComponent viewComponent = new AngularModuleComponent("view-" + table.TableName.ToLower(), AngularComponentType.ViewComponent);
                viewComponent.ComponentFolderName = "view-" + table.TableName.ToLower();
                viewComponent.ComponentFolderPath = Path.Combine(module.ModuleFolderPath, module.ModuleFolderName, viewComponent.ComponentFolderName);

                AngularFile styleFile = new AngularFile("view-" + table.TableName.ToLower() + ".component", AngularClassType.SCSS);
                AngularFile htmlFile = new AngularFile("view-" + table.TableName.ToLower() + ".component", AngularClassType.Html);
                htmlFile.FileContents = new AngularFileService(table).GenerateFromTemplateFile("view-{name}.component.html");
                AngularFile tsFile = new AngularFile("view-" + table.TableName.ToLower() + ".component", AngularClassType.ts);
                tsFile.FileContents = new AngularFileService(table).GenerateFromTemplateFile("view-{name}.component.ts");
                viewComponent.Files = new List<AngularFile>() { styleFile, htmlFile, tsFile };
                angularModuleComponents.Add(viewComponent);
            }

            return angularModuleComponents;
        }

        public AngularRoutine Process()
        {
            GeneratePackage();
            return this;
        }

        private AngularRoutine GeneratePackage()
        {
            AngularPackageFileList modulesFilesList = new AngularPackageFileList();

            foreach (AngularModule module in packageStructure.Modules)
            {
                string moduleFile = (Path.Combine(RootFilePath, module.ModuleFolderPath, module.ModuleFolderName, module.ModuleName + GetFileExtension(module.ModuleType)));
                modulesFilesList.Add(new AngularPackageFile()
                {
                    FilePath = moduleFile,
                    Contents = module.FileContents
                });

                foreach (var component in module.Components)
                {
                    foreach (var cfile in component.Files)
                    {
                        string componentFile = Path.Combine(RootFilePath, component.ComponentFolderPath, cfile.FileName + GetFileExtension(cfile.FileType));
                        modulesFilesList.Add(new AngularPackageFile()
                        {
                            FilePath = componentFile,
                            Contents = cfile.FileContents
                        });
                    }
                }
            }

            modulesFilesList.Create();
            return this;
        }

        private string GetFileExtension(AngularClassType moduleType)
        {
            string extension = ".ts";
            switch (moduleType)
            {
                case AngularClassType.Component:
                    extension = ".ts";
                    break;
                case AngularClassType.Service:
                    extension = ".ts";
                    break;
                case AngularClassType.RoutingModule:
                    extension = ".ts";
                    break;
                case AngularClassType.Module:
                    extension = ".ts";
                    break;
                case AngularClassType.Model:
                    extension = ".ts";
                    break;
                case AngularClassType.CSS:
                    extension = ".css";
                    break;
                case AngularClassType.SCSS:
                    extension = ".scss";
                    break;
                case AngularClassType.Html:
                    extension = ".html";
                    break;
                case AngularClassType.ts:
                    extension = ".ts";
                    break;
                default:
                    break;
            }
            return extension;
        }
    }

    public class AngularPackageFileList : List<AngularPackageFile>
    {
        public AngularPackageFileList()
        {

        }

        public void Create()
        {
            foreach (AngularPackageFile packageFile in this)

                new AngularFileService().SaveToFile(packageFile.FilePath, packageFile.Contents);
        }
    }

    public class AngularPackageFile
    {
        public string FilePath { get; set; }
        public string Contents { get; set; }
    }
}