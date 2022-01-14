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

namespace ToDoStuff.UserControls
{
    /// <summary>
    /// Interaction logic for ImageStoreJsonView.xaml
    /// </summary>

    [ControlNameAttribute("Image Store Json")]
    public partial class ImageStoreJsonView : UserControl, IControlsInterface
    {
        public ImageStoreJsonView()
        {
            InitializeComponent();
        }

        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtImageFolderParentPath.Text.Trim()))
                {
                    string rootPath = txtImageFolderParentPath.Text.Trim();

                    string[] directories = Directory.GetDirectories(rootPath);
                    foreach (var dir in directories)
                    {
                        string directoryName = System.IO.Path.GetFileName(dir);
                        string[] subDirectories = Directory.GetDirectories(dir);
                        if (subDirectories != null)
                        {
                            foreach (var subdir in subDirectories)
                            {
                                string[] subDirectories1 = Directory.GetDirectories(subdir);
                                ImageStoreJsonModel imageStoreJsonModel = new ImageStoreJsonModel();
                                imageStoreJsonModel.FolderName = System.IO.Path.GetFileName(subdir);
                                imageStoreJsonModel.Url = "/" + directoryName + "/" + System.IO.Path.GetFileName(subdir);
                                imageStoreJsonModel.SerialFolderName = "chapter-";
                                imageStoreJsonModel.SerialStart = 1;
                                imageStoreJsonModel.SerialEnd = subDirectories1.Count();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }


    class ImageStoreJsonModel
    {
        public string FolderName { get; set; }
        public string SerialType { get; set; }
        public long IndexStart { get; set; }
        public long IndexEnd { get; set; }
        public string SerialFolderName { get; set; }
        public long SerialStart { get; set; }
        public long SerialEnd { get; set; }
        public string Url { get; set; }

    }
}
