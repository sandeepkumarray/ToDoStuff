using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for UrlsToBrowserView.xaml
    /// </summary>
    [ControlNameAttribute("Urls To Browser")]
    public partial class UrlsToBrowserView : UserControl, IControlsInterface
    {
        public UrlsToBrowserView()
        {
            InitializeComponent();
        }

        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }

        private void btnOpenBrowser_Click(object sender, RoutedEventArgs e)
        {
            List<string> URLList = new List<string>();
            URLList = txtPasteUrls.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();

            Process.Start("chrome", string.Join(" ", URLList));

        }
    }
}
