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
    /// Interaction logic for PantUMLView.xaml
    /// </summary>
    [ControlNameAttribute("PantUML View")]
    public partial class PantUMLView : UserControl, IControlsInterface
    {
        public PantUMLView()
        {
            InitializeComponent();
        }

        public string DisplayName { get; set; }
        string ProcessFileName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBasePath.Text.Trim()))
            {

                ProcessFileName = System.IO.Path.GetFileNameWithoutExtension(txtBasePath.Text.Trim());
                string allLines = File.ReadAllText(txtBasePath.Text.Trim());
                string[] splits = allLines.Split(new string[] { "\"GIVEN" }, StringSplitOptions.None);

                StringBuilder sbOut = new StringBuilder();
                sbOut.AppendLine("@startuml " + ProcessFileName);
                sbOut.AppendLine("start");
                foreach (var line in splits)
                {
                    sbOut.AppendLine(ProcessStatement(line));
                }
                sbOut.AppendLine("stop");
                sbOut.AppendLine("@enduml");

                FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                        "PlantUMLs", ProcessFileName + ".wsd"), sbOut.ToString(), true);

                MessageBox.Show("Complete");
            }
            else
                MessageBox.Show("No files to process.");
        }

        private string ProcessStatement(string line)
        {
            StringBuilder sbLine = new StringBuilder();
            List<PlantUMLCase> PlantUMLCaseList = new List<PlantUMLCase>();
            //Transform
            if (!string.IsNullOrEmpty(line))
            {
                string[] caseDetails = line.Split(new string[] { "WHEN", "THEN" }, StringSplitOptions.None);
                PlantUMLCase plantUMLCase = new PlantUMLCase();
                plantUMLCase.given.Text = caseDetails[0].Trim();
                plantUMLCase.when.Text = caseDetails[1].Trim();
                plantUMLCase.then.Text = caseDetails[2].Trim();
                PlantUMLCaseList.Add(plantUMLCase);
            }

            if (PlantUMLCaseList != null && PlantUMLCaseList.Count > 0)
            {
                foreach (var umlcase in PlantUMLCaseList)
                {
                    sbLine.AppendLine(":" + umlcase.given.Text.Replace("\"", "") + ";");
                    sbLine.AppendLine("if (" + umlcase.when.Text.Replace("\"", "") + ") then (True)");
                    sbLine.AppendLine(":" + umlcase.then.Text.Replace("\"", "") + ";");
                    sbLine.AppendLine("else (False)");
                    sbLine.AppendLine("    stop");
                    sbLine.AppendLine("endif");
                    sbLine.AppendLine("");
                }
            }

            return sbLine.ToString();
        }
    }

    public class PlantUMLCase
    {
        public PlantUMLCase()
        {
            given = new GivenClass();
            when = new WhenClass();
            then = new ThenClass();
        }

        public GivenClass given { get; set; }
        public WhenClass when { get; set; }
        public ThenClass then { get; set; }
    }

    public class GivenClass
    {
        public string Text { get; set; }
    }

    public class WhenClass : GivenClass
    {

    }

    public class ThenClass : GivenClass
    {

    }
}
