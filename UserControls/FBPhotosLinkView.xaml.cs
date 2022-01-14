using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
    /// Interaction logic for FBPhotosLinkView.xaml
    /// </summary>
    [ControlNameAttribute("FB Photos Link")]
    public partial class FBPhotosLinkView : UserControl, IControlsInterface
    {
        public FBPhotosLinkView()
        {
            InitializeComponent();
        }

        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            var URLList = txtPasteUrls.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            StringBuilder sb = new StringBuilder();
            string format = txtFormat.Text;
            string profileName = txtProfileName.Text;

            List<string> queryNames = new List<string>();
            queryNames.Add("fbid");
            queryNames.Add("set");

            foreach (var url in URLList)
            {
                Uri uri = new Uri(url);
                var queryCol = HttpUtility.ParseQueryString(uri.Query);
                sb.AppendLine(format.Replace("{PAGENAME}", profileName).Replace("{SET}", queryCol.Get("set")).Replace("{FBID}", queryCol.Get("fbid")));
            }

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                "ScrapOut", "fb_img_urls" + ".txt"),
              sb.ToString(), false);
            MessageBox.Show("Process Completed!!!");
        }
    }
}
