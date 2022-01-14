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
    /// Interaction logic for CreateComicsHtmlView.xaml
    /// </summary>
    [ControlNameAttribute("Create Comics Html")]
    public partial class CreateComicsHtmlView : UserControl, IControlsInterface
    {
        public CreateComicsHtmlView()
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
            string title = txtTitle.Text.Trim();
            string basePath = txtBasePath.Text.Trim();
            string chapterTemplatePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "comicChapterTemplate.html");

            DirectoryInfo dir = new DirectoryInfo(basePath);

            var chapterList = dir.GetDirectories();

            //int chapterCount = 1;
            foreach (var chp in chapterList)
            {
                var chapterTemp = File.ReadAllText(chapterTemplatePath);

                string chapterNo = chp.Name.Split('-')[1];
                int chapterNum = Convert.ToInt32(chapterNo);

                chapterTemp = chapterTemp.Replace("{TITLE}", title);
                chapterTemp = chapterTemp.Replace("{CHAPTER}", "chapter " + chapterNum);

                StringBuilder sbImgList = new StringBuilder();
                var allImgs = Directory.GetFiles(chp.FullName);

                foreach (var imf in allImgs)
                {
                    sbImgList.AppendLine("<li class='list - group - item'><img class='img-fluid' src='./chapter-" + chapterNum + "/" + System.IO.Path.GetFileName(imf) + "'/></li>");
                }

                chapterTemp = chapterTemp.Replace("{IMAGELIST}", sbImgList.ToString());
                chapterTemp = chapterTemp.Replace("{PREV}", chapterNum == 1 ? "#" : "chapter-" + (chapterNum - 1) + ".html");
                chapterTemp = chapterTemp.Replace("{NEXT}", "chapter-" + (chapterNum + 1) + ".html");

                File.WriteAllText(System.IO.Path.Combine(chp.Parent.FullName, "chapter-" + chapterNum + ".html"), chapterTemp);
            }


            MessageBox.Show("Done");
        }
    }
}
