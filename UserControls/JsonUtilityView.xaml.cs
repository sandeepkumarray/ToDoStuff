using Newtonsoft.Json;
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
using ToDoStuff.Model;
using ToDoStuff.Model.Book;

namespace ToDoStuff.UserControls
{
    /// <summary>
    /// Interaction logic for JsonUtilityView.xaml
    /// </summary>
    [ControlNameAttribute("Json Utility")]
    public partial class JsonUtilityView : UserControl, IControlsInterface
    {
        public JsonUtilityView()
        {
            InitializeComponent();
        }
        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }

        private void btnProcessFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtFolderPath.Text))
                {
                    string folderPath = txtFolderPath.Text.Trim();
                    string[] files = Directory.GetFiles(folderPath);
                    foreach (var item in files)
                    {
                        string sqlstring = ProcessJsonFile(item);
                        FileUtility.SaveDataToFile(System.IO.Path.Combine(folderPath, "result" + DateTime.Now.ToString("_dd_MM_yyyy_hh_mm_ss") + ".txt"), sqlstring);
                    }
                    MessageBox.Show("All files processed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Folder doesn't exist!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string ProcessJsonFile(string item)
        {
            string jsonString = "";
            try
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(item);
                string fileExtention = System.IO.Path.GetExtension(item);
                if (fileExtention.ToLower().Contains("json"))
                {
                    string sr = File.ReadAllText(item);
                    BookCategoryModel bmodel = new BookCategoryModel();
                    bmodel = JsonConvert.DeserializeObject<BookCategoryModel>(sr);

                    ProcessCategory(fileName, bmodel.overview);
                    ProcessCategory(fileName, bmodel.overview);
                    ProcessCategory(fileName, bmodel.occupants);
                    ProcessCategory(fileName, bmodel.design);
                    ProcessCategory(fileName, bmodel.purpose);
                    ProcessCategory(fileName, bmodel.location);
                    ProcessCategory(fileName, bmodel.neighborhood);
                    ProcessCategory(fileName, bmodel.financial);
                    ProcessCategory(fileName, bmodel.amenities);
                    ProcessCategory(fileName, bmodel.history);
                    ProcessCategory(fileName, bmodel.gallery);
                    ProcessCategory(fileName, bmodel.notes);

                    string newSR = JsonConvert.SerializeObject(bmodel, Formatting.Indented);
                    File.WriteAllText(item, newSR);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return jsonString;
        }

        void ProcessCategory(string fileName, Model.Book.Category bookCategory)
        {
            if (bookCategory != null && bookCategory.attributes != null)
            {
                if (bookCategory.attributes.Count > 0)
                {
                    foreach (var attr in bookCategory.attributes)
                    {
                        if (string.IsNullOrEmpty(attr.name))
                        {
                            if (!string.IsNullOrEmpty(attr.label))
                            {
                                attr.name = attr.label;
                            }
                        }
                        else
                        {
                            if (attr.name.ToLower().Contains("type_of"))
                            {
                                attr.help_text = "What kind of " + fileName + " is this " + fileName + "?";
                            }

                            switch (attr.name.ToLower())
                            {
                                case "name":
                                    attr.help_text = "What is this " + fileName + "'s name?";
                                    break;
                                case "size":
                                    attr.help_text = "How big (or small) is this " + fileName + "?";
                                    break;
                                case "weight":
                                    attr.help_text = "How much does this " + fileName + " weigh?";
                                    break;
                                case "alternate_names":
                                    attr.help_text = "What other names is this " + fileName + " known by?";
                                    break;
                                case "other_names":
                                    attr.help_text = "What other names is this " + fileName + " known by?";
                                    break;
                                case "description":
                                    attr.help_text = "How would you describe this " + fileName + "?";
                                    break;
                                case "universe_id":
                                    attr.help_text = "This field allows you to link your other pages to this " + fileName + ".";
                                    break;
                                case "notes":
                                    attr.help_text = "Write as little or as much as you want!";
                                    break;
                                case "private_notes":
                                    attr.help_text = "Write as little or as much as you want!";
                                    break;
                                case "tags":
                                    attr.help_text = "This field lets you add clickable tags to your " + fileName + "'s.";
                                    break;
                                default:
                                    attr.help_text = "This field allows you to link your other pages to this " + fileName + ".";
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void btnReadJson_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
