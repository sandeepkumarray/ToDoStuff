using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ChromeBookmarkParserView.xaml
    /// </summary>
    [ControlNameAttribute("Chrome Bookmark Parser")]
    public partial class ChromeBookmarkParserView : UserControl, IControlsInterface
    {
        public ChromeBookmarkParserView()
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
                string regPattern = @"<A.*?>(.*?)<\\/A>";
                var rx = new Regex(".*?<a href=\"(.*?)\">(.*?)</a>");

                if (!string.IsNullOrEmpty(txtBMFilePath.Text.Trim()))
                {
                    string htmlContent = File.ReadAllText(txtBMFilePath.Text);
                    htmlContent = htmlContent.ToLower().Replace("<dt>", "").Replace("</dt>", "").Replace("<dl>", "").Replace("</dl>", "").Replace("<p>", "");
                    ExecuteMethod(htmlContent);
                    //MatchCollection matchedAuthors = rx.Matches(htmlContent);

                    //for (int count = 0; count < matchedAuthors.Count; count++)
                    //    Console.WriteLine(matchedAuthors[count].Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex.Message);
            }
        }

        void ExecuteMethod(string htmlContent)
        {
            string pattern = "//dt/a";
            bool hasParseError = false;
            StringBuilder sbError = new StringBuilder();

            if (!string.IsNullOrEmpty(txtPatternToSearch.Text.Trim()))
            {
                pattern = txtPatternToSearch.Text.Trim();
            }

            string attribute = "HREF";

            if (!string.IsNullOrEmpty(txtAttributeValue.Text.Trim()))
            {
                attribute = txtAttributeValue.Text.Trim();
            }

            HtmlDocument htmlDoc = new HtmlDocument();

            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(htmlContent);
            // filePath is a path to a file containing the html
            //htmlDoc.Load(txtBMFilePath.Text);

            // Use:  htmlDoc.LoadHtml(xmlString);  to load from a string (was htmlDoc.LoadXML(xmlString)

            // ParseErrors is an ArrayList containing any errors from the Load statement
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required
                hasParseError = true;
                foreach (var err in htmlDoc.ParseErrors)
                {
                    sbError.AppendLine(err.Reason);
                }
            }
            else
            {

                if (htmlDoc.DocumentNode != null)
                {
                    HtmlAgilityPack.HtmlNodeCollection nodeCollection = htmlDoc.DocumentNode.SelectNodes(pattern);//"//dt/a"

                    if (nodeCollection != null)
                    {
                        List<string> dataList = new List<string>();
                        foreach (var node in nodeCollection)
                        {
                            dataList.Add(node.Attributes[attribute].Value);//"HREF"
                        }

                        FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ScrapOut", "extractedList.txt"), string.Join("\r\n", dataList), true);
                    }
                }
            }

            if (hasParseError)
                MessageBox.Show(sbError.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            MessageBox.Show("Done.");
        }


        void ExecuteMethod()
        {
            string pattern = "//dt/a";
            bool hasParseError = false;
            StringBuilder sbError = new StringBuilder();

            if (!string.IsNullOrEmpty(txtPatternToSearch.Text.Trim()))
            {
                pattern = txtPatternToSearch.Text.Trim();
            }

            string attribute = "HREF";

            if (!string.IsNullOrEmpty(txtAttributeValue.Text.Trim()))
            {
                attribute = txtAttributeValue.Text.Trim();
            }


            if (!string.IsNullOrEmpty(txtBMFilePath.Text.Trim()))
            {
                string htmlContent = File.ReadAllText(txtBMFilePath.Text);
                HtmlDocument htmlDoc = new HtmlDocument();

                htmlDoc.OptionFixNestedTags = true;

                // filePath is a path to a file containing the html
                htmlDoc.Load(txtBMFilePath.Text);

                // Use:  htmlDoc.LoadHtml(xmlString);  to load from a string (was htmlDoc.LoadXML(xmlString)

                // ParseErrors is an ArrayList containing any errors from the Load statement
                if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
                {
                    // Handle any parse errors as required

                }
                else
                {

                    if (htmlDoc.DocumentNode != null)
                    {
                        HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

                        if (bodyNode != null)
                        {
                            // Do something with bodyNode
                        }
                    }
                }
            }

            else
            {
                string htmlContent = new TextRange(rtHtmlString.Document.ContentStart, rtHtmlString.Document.ContentEnd).Text;
                HtmlDocument htmlDoc = new HtmlDocument();

                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.LoadHtml(htmlContent);
                // filePath is a path to a file containing the html
                //htmlDoc.Load(txtBMFilePath.Text);

                // Use:  htmlDoc.LoadHtml(xmlString);  to load from a string (was htmlDoc.LoadXML(xmlString)

                // ParseErrors is an ArrayList containing any errors from the Load statement
                if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
                {
                    // Handle any parse errors as required
                    hasParseError = true;
                    foreach (var err in htmlDoc.ParseErrors)
                    {
                        sbError.AppendLine(err.Reason);
                    }
                }
                else
                {

                    if (htmlDoc.DocumentNode != null)
                    {
                        HtmlAgilityPack.HtmlNodeCollection nodeCollection = htmlDoc.DocumentNode.SelectNodes(pattern);//"//dt/a"

                        if (nodeCollection != null)
                        {
                            List<string> dataList = new List<string>();
                            foreach (var node in nodeCollection)
                            {
                                dataList.Add(node.Attributes[attribute].Value);//"HREF"
                            }

                            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ScrapOut", "extractedList.txt"), string.Join("\r\n", dataList), true);
                        }
                    }
                }
            }

            MessageBox.Show(sbError.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            MessageBox.Show("Done.");
        }
    }
}
