using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoStuff.Helpers;

namespace ToDoStuff.UserControls
{
    /// <summary>
    /// Interaction logic for YamlTOJsonView.xaml
    /// </summary>

    [ControlNameAttribute("Yaml 2 Json")]
    public partial class YamlTOJsonView : UserControl, IControlsInterface
    {
        public YamlTOJsonView()
        {
            InitializeComponent();
        }

        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }
        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (rbYamlText.IsChecked == true)
                {

                }

                if (rbYamlTextFile.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(txtYAMLFilePath.Text))
                    {
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(txtYAMLFilePath.Text.Trim());
                        string jsonFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ScrapOut", "YamlToJson", fileName + ".json");

                        string jsonString = YamlToJsonConverter.YamlToJson(File.ReadAllText(txtYAMLFilePath.Text.Trim()));
                        jsonString = JToken.Parse(jsonString).ToString();

                        FileUtility.SaveDataToFile(jsonFilePath, jsonString, true);
                    }                        
                }

                if (rbYamlFolder.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(txtYAMLFolderPath.Text))
                    {
                        string[] files = Directory.GetFiles(txtYAMLFolderPath.Text.Trim());

                        foreach (var file in files)
                        {
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                            string jsonFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ScrapOut", "YamlToJson", fileName + ".json");

                            string jsonString = YamlToJsonConverter.YamlToJson(File.ReadAllText(file));
                            jsonString = JToken.Parse(jsonString).ToString();

                            FileUtility.SaveDataToFile(jsonFilePath, jsonString, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("Process Completed.");

        }
    }
}
