using System;
using System.Collections;
using System.Collections.Generic;
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
using ToDoStuff.Engine;

namespace ToDoStuff.UserControls
{
    /// <summary>
    /// Interaction logic for CreateHTMLControls.xaml
    /// </summary>


    [ControlNameAttribute("Create HTML Controls")]
    public partial class CreateHTMLControls : UserControl, IControlsInterface
    {
        public CreateHTMLControls()
        {
            InitializeComponent();
            LoadNotes();
            lstProperties.ContextMenu = CreateListBoxContextMenu("Paste");
        }

        private void LoadNotes()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("Notes : ");
            sb.AppendLine("Append [U] for all Uppercase.");
            sb.AppendLine("Append [l] for all Lowercase.");
            sb.AppendLine("Append [Uc] for first character Uppercase.");
            sb.AppendLine("Append [Cc] for camel case character.");
            sb.AppendLine("Example : [Placeholder][U]");
            
            rtbNotes.Text = (sb.ToString());
        }

        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }

        private ContextMenu CreateListBoxContextMenu(string Action)
        {
            ContextMenu ContextMenuObj = new ContextMenu();
            switch (Action)
            {
                case "Paste":
                    MenuItem pasteItem = new MenuItem();
                    pasteItem.Header = "Paste";
                    pasteItem.Click += PasteItem_Click;
                    ContextMenuObj.Items.Add(pasteItem);
                    break;
                default:
                    break;
            }
            return ContextMenuObj;
        }

        private void PasteItem_Click(object sender, RoutedEventArgs e)
        {
            var copyData = Clipboard.GetDataObject();
            if (copyData != null)
            {
                var datos = (string)copyData.GetData(DataFormats.Text);
                var stringRows = datos.Split(new Char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string stringRow in stringRows)
                {
                    lstProperties.Items.Add(stringRow);
                    lstProperties.Items.Refresh();
                }
            }
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            string content = new TextRange(rtbHtmlTemplate.Document.ContentStart, rtbHtmlTemplate.Document.ContentEnd).Text;
            var listItems = lstProperties.Items.Cast<string>().ToList();
            CreateHTMLControlsEngine engine = new CreateHTMLControlsEngine(listItems, content, txtPlaceholder.Text.Trim());

            rtbResponse.AppendText(engine.Process());
        }
    }
}
