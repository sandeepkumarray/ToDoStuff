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
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace ToDoStuff.UserControls
{
    /// <summary>
    /// Interaction logic for FileFolderUtility.xaml
    /// </summary>
    [ControlNameAttribute("File Folder Utility")]
    public partial class FileFolderUtility : UserControl, IControlsInterface
    {
        StackPanel mainStackPanel = new StackPanel();

        Dictionary<string, string> GlobalPropertyList = new Dictionary<string, string>();
        Dictionary<string, UIElement> GlobalPropertyElements = new Dictionary<string, UIElement>();

        public FileFolderUtility()
        {
            InitializeComponent();
            DisplayName = "File Folder Utility";
            mainStackPanel.Orientation = Orientation.Vertical;
            ucProperties.Content = mainStackPanel;
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
                if (!string.IsNullOrEmpty(txtFolderPath.Text) && !string.IsNullOrEmpty(txtFileNameToBe.Text))
                {
                    string condition = "";
                    bool conditionChecked = false;

                    if (rdbContains.IsChecked == true)
                    {
                        conditionChecked = true;
                        condition = "Contains";
                    }
                    if (rdbEquals.IsChecked == true)
                    {
                        conditionChecked = true;
                        condition = "Equals";
                    }
                    if (rdbStartsWith.IsChecked == true)
                    {
                        conditionChecked = true;
                        condition = "StartsWith";
                    }
                    if (rdbEndsWith.IsChecked == true)
                    {
                        conditionChecked = true;
                        condition = "EndsWith";
                    }

                    if (conditionChecked == true)
                    {
                        ProcessDirectory(txtFolderPath.Text.Trim(), condition);
                        MessageBox.Show("Process completed.", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        MessageBox.Show("No Conditions Provided.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Folder Path or File Name is empty !!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ProcessDirectory(string targetDirectory, string option)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName, option);

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory, option);
        }

        public void ProcessFile(string path, string option)
        {
            string NewFileName = "";
            switch (option)
            {
                case "Contains":
                    if (path.Contains(txtFileNameToBe.Text))
                    {
                        NewFileName = path.Replace(txtFileNameToBe.Text, txtFileName.Text);
                        File.Move(path, NewFileName);
                    }
                    break;
                case "EqualsTo":
                    if (path.Equals(txtFileNameToBe.Text))
                    {
                        NewFileName = path.Replace(txtFileNameToBe.Text, txtFileName.Text);
                        File.Move(path, NewFileName);
                    }
                    break;
                case "StartsWith":
                    if (path.StartsWith(txtFileNameToBe.Text))
                    {
                        NewFileName = path.Replace(txtFileNameToBe.Text, txtFileName.Text);
                        File.Move(path, NewFileName);
                    }
                    break;
                case "EndsWith":
                    if (path.EndsWith(txtFileNameToBe.Text))
                    {
                        NewFileName = path.Replace(txtFileNameToBe.Text, txtFileName.Text);
                        File.Move(path, NewFileName);
                    }
                    break;
                default:
                    break;
            }
        }

        private void rbFileMetadata_Checked(object sender, RoutedEventArgs e)
        {
            LogActions("Initialising.....");
            Init();
        }

        private void Init()
        {
            LogActions("Creating Property List");
            List<FileItemProp> items = new List<FileItemProp>();

            items.Add(new FileItemProp() { Name = "None", Category = "" });
            items.Add(new FileItemProp() { Name = "Title", Category = "Description" });
            items.Add(new FileItemProp() { Name = "Subtitle", Category = "Description" });
            items.Add(new FileItemProp() { Name = "Rating", Category = "Description" });
            items.Add(new FileItemProp() { Name = "Tags", Category = "Description" });
            items.Add(new FileItemProp() { Name = "Comments", Category = "Description" });


            items.Add(new FileItemProp() { Name = "Contributing artists", Category = "Media" });
            items.Add(new FileItemProp() { Name = "Album artist", Category = "Media" });
            items.Add(new FileItemProp() { Name = "Album", Category = "Media" });
            items.Add(new FileItemProp() { Name = "Year", Category = "Media" });
            items.Add(new FileItemProp() { Name = "Genre", Category = "Media" });
            items.Add(new FileItemProp() { Name = "Length", Category = "Media" });


            items.Add(new FileItemProp() { Name = "Frame width", Category = "Video" });
            items.Add(new FileItemProp() { Name = "Frame height", Category = "Video" });
            items.Add(new FileItemProp() { Name = "Data rate", Category = "Video" });
            items.Add(new FileItemProp() { Name = "Total bitrate", Category = "Video" });
            items.Add(new FileItemProp() { Name = "Frame rate", Category = "Video" });


            items.Add(new FileItemProp() { Name = "Bit rate", Category = "Audio" });
            items.Add(new FileItemProp() { Name = "Channels", Category = "Audio" });
            items.Add(new FileItemProp() { Name = "Audio sample rate", Category = "Audio" });


            items.Add(new FileItemProp() { Name = "Directors", Category = "Origin" });
            items.Add(new FileItemProp() { Name = "Producers", Category = "Origin" });
            items.Add(new FileItemProp() { Name = "Writers", Category = "Origin" });
            items.Add(new FileItemProp() { Name = "Publisher", Category = "Origin" });
            items.Add(new FileItemProp() { Name = "Content provider", Category = "Origin" });
            items.Add(new FileItemProp() { Name = "Media created", Category = "Origin" });
            items.Add(new FileItemProp() { Name = "Encoded by", Category = "Origin" });
            items.Add(new FileItemProp() { Name = "Author URL", Category = "Origin" });
            items.Add(new FileItemProp() { Name = "Promotion URL", Category = "Origin" });
            items.Add(new FileItemProp() { Name = "Copyright", Category = "Origin" });


            items.Add(new FileItemProp() { Name = "Parental rating", Category = "Content" });
            items.Add(new FileItemProp() { Name = "Parental rating reason", Category = "Content" });
            items.Add(new FileItemProp() { Name = "Composers", Category = "Content" });
            items.Add(new FileItemProp() { Name = "Conductors", Category = "Content" });
            items.Add(new FileItemProp() { Name = "Group description", Category = "Content" });
            items.Add(new FileItemProp() { Name = "Period", Category = "Content" });
            items.Add(new FileItemProp() { Name = "Mood", Category = "Content" });
            items.Add(new FileItemProp() { Name = "Part of set", Category = "Content" });
            items.Add(new FileItemProp() { Name = "Initial key", Category = "Content" });
            items.Add(new FileItemProp() { Name = "Beats-per-minute", Category = "Content" });
            items.Add(new FileItemProp() { Name = "Protected", Category = "Content" });
            items.Add(new FileItemProp() { Name = "Part of a compilation", Category = "Content" });


            items.Add(new FileItemProp() { Name = "Name", Category = "File" });
            items.Add(new FileItemProp() { Name = "Item type", Category = "File" });
            items.Add(new FileItemProp() { Name = "Folder path", Category = "File" });
            items.Add(new FileItemProp() { Name = "Date created", Category = "File" });
            items.Add(new FileItemProp() { Name = "Date modified", Category = "File" });
            items.Add(new FileItemProp() { Name = "Size", Category = "File" });
            items.Add(new FileItemProp() { Name = "Attributes", Category = "File" });
            items.Add(new FileItemProp() { Name = "Availability", Category = "File" });
            items.Add(new FileItemProp() { Name = "Offine status", Category = "File" });
            items.Add(new FileItemProp() { Name = "Shared with", Category = "File" });
            items.Add(new FileItemProp() { Name = "Owner", Category = "File" });
            items.Add(new FileItemProp() { Name = "Computer", Category = "File" });


            ListCollectionView lcv = new ListCollectionView(items);
            lcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            cbMetaList.ItemsSource = lcv;
            cbMetaList.SelectedIndex = -1;

            LogActions("Added Property List to dropdown");
        }

        private void cbMetaList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                string propName = ((FileItemProp)(e.AddedItems[0])).Name.Replace(" ", "_");
                LogActions("Selected Property is " + propName);
                if (!GlobalPropertyList.ContainsKey(propName))
                {
                    GlobalPropertyList.Add(propName, "");
                    StackPanel spProp = new StackPanel();
                    spProp.Margin = new Thickness(3);
                    spProp.Orientation = Orientation.Horizontal;
                    spProp.Name = "sp" + propName;
                    spProp.Tag = propName;

                    Label lblProp = new Label();
                    lblProp.Width = 150;
                    lblProp.Content = ((FileItemProp)(e.AddedItems[0])).Name;

                    TextBox txtPropValue = new TextBox();
                    txtPropValue.Name = "txt" + propName;
                    txtPropValue.Tag = propName;
                    txtPropValue.MinWidth = 200;
                    txtPropValue.MaxWidth = 300;
                    txtPropValue.TextChanged += TxtPropValue_TextChanged;

                    ComboBox cbPropValue = new ComboBox();
                    cbPropValue.IsEditable = true;
                    cbPropValue.Name = "cb" + propName;
                    cbPropValue.Tag = propName;
                    cbPropValue.MinWidth = 200;
                    cbPropValue.MaxWidth = 300;

                    List<string> propValueList = new List<string>();
                    propValueList.Add("Same as Name");
                    propValueList.Add("Same as Name Last value after space");
                    propValueList.Add("Same as Name First value before space");
                    propValueList.Add("Same as Name value after first space");

                    cbPropValue.ItemsSource = propValueList;

                    Button btnClose = new Button();
                    btnClose.Name = "btnClose";
                    btnClose.Tag = spProp.Name;
                    btnClose.Width = 25;
                    btnClose.Height = 25;
                    btnClose.Content = "X";
                    btnClose.FontSize = 15;
                    btnClose.FontWeight = FontWeights.Bold;
                    btnClose.Foreground = new SolidColorBrush(Colors.Black);
                    btnClose.Background = new SolidColorBrush(Colors.Red);
                    btnClose.Click += BtnClose_Click;

                    Label lblPropDeriveValue = new Label();
                    lblPropDeriveValue.Width = 150;
                    lblPropDeriveValue.Content = "Value Derive Logic";

                    spProp.Children.Add(lblProp);
                    spProp.Children.Add(cbPropValue);
                    spProp.Children.Add(btnClose);

                    LogActions("Control added for Property " + propName);
                    GlobalPropertyElements.Add(spProp.Name, spProp);

                    mainStackPanel.Children.Add(spProp);
                }
                else
                {
                    MessageBox.Show(propName + " is already added for modification");
                }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Button btnClose = (Button)sender;
            string spNameTag = Convert.ToString(btnClose.Tag);
            if (GlobalPropertyElements.ContainsKey(spNameTag))
            {
                UIElement ele = GlobalPropertyElements[spNameTag];

                mainStackPanel.Children.Remove(ele);
                GlobalPropertyElements.Remove(spNameTag);
                GlobalPropertyList.Remove(Convert.ToString(((StackPanel)ele).Tag));
                LogActions("Removed Property " + spNameTag);
            }

        }

        private void TxtPropValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtPropValue = (TextBox)sender;

            LogActions("Value for " + Convert.ToString(txtPropValue.Tag) + " is " + txtPropValue.Text.Trim());
            string value = txtPropValue.Text.Trim();
            if (GlobalPropertyList.ContainsKey(Convert.ToString(txtPropValue.Tag)))
            {
                LogActions("GlobalPropertyList updated.");
                GlobalPropertyList[Convert.ToString(txtPropValue.Tag)] = value;
            }
        }

        private void LogActions(string message)
        {
            rtconsole.AppendText(message + "\r\n");
            rtconsole.ScrollToEnd();
        }

        private void btnMetaProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtMetaFolderPath.Text))
                {
                    string[] fileEntries = Directory.GetFiles(txtMetaFolderPath.Text);
                    foreach (string fileName in fileEntries)
                        ProcessFileForMeta(fileName);
                }

                if (!string.IsNullOrEmpty(txtMetaFileName.Text))
                {
                    ProcessFileForMeta(txtMetaFileName.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ProcessFileForMeta(string fileName)
        {
            ShellFile shellFile = ShellFile.FromFilePath(fileName);

            if (GlobalPropertyElements != null)
            {
                foreach (var ele in GlobalPropertyElements)
                {
                    StackPanel stackPanel = (StackPanel)ele.Value;

                    var lblProp = stackPanel.Children[0];
                    ComboBox comboBoxProp = (ComboBox)stackPanel.Children[1];

                    string propValue = comboBoxProp.Text;

                    if (comboBoxProp.SelectedItem != null)
                    {
                        string fileNameWExt = System.IO.Path.GetFileNameWithoutExtension(shellFile.Name);
                        string fileExt = System.IO.Path.GetExtension(shellFile.Name);

                        switch (comboBoxProp.SelectedValue)
                        {
                            case "Same as Name":
                                propValue = fileNameWExt;
                                break;
                            case "Same as Name Last value after space":
                                propValue = fileNameWExt.Split(' ')[fileNameWExt.Split(' ').Length - 1];
                                break;
                            case "Same as Name First value before space":
                                propValue = fileNameWExt.Split(' ')[0];
                                break;
                            case "Same as Name value after first space":
                                string[] nameValues = fileNameWExt.Split(' ');
                                propValue = string.Join(" ", nameValues.Where(s => s != fileNameWExt.Split(' ')[0]));
                                break;
                            default:
                                break;
                        }
                    }

                    switch (Convert.ToString(stackPanel.Tag).Replace("_", " "))
                    {
                        case "None":
                            break;
                        case "Title":
                            shellFile.Properties.System.Title.Value = propValue;
                            break;
                        case "Subtitle":
                            break;
                        case "Rating":
                            break;
                        case "Tags":
                            break;
                        case "Comments":
                            break;
                        case "Contributing artists":
                            shellFile.Properties.System.Music.Artist.Value = new string[] { propValue };
                            break;
                        case "Album artist":
                            break;
                        case "Album":
                            shellFile.Properties.System.Music.AlbumTitle.Value = propValue;
                            break;
                        case "Year":
                            break;
                        case "Genre":
                            break;
                        case "Length":
                            break;
                        case "Frame width":
                            break;
                        case "Frame height":
                            break;
                        case "Data rate":
                            break;
                        case "Total bitrate":
                            break;
                        case "Frame rate":
                            break;
                        case "Bit rate":
                            break;
                        case "Channels":
                            break;
                        case "Audio sample rate":
                            break;
                        case "Directors":
                            break;
                        case "Producers":
                            break;
                        case "Writers":
                            break;
                        case "Publisher":
                            break;
                        case "Content provider":
                            break;
                        case "Media created":
                            break;
                        case "Encoded by":
                            break;
                        case "Author URL":
                            break;
                        case "Promotion URL":
                            break;
                        case "Copyright":
                            break;
                        case "Parental rating":
                            break;
                        case "Parental rating reason":
                            break;
                        case "Composers":
                            break;
                        case "Conductors":
                            break;
                        case "Group description":
                            break;
                        case "Period":
                            break;
                        case "Mood":
                            break;
                        case "Part of set":
                            break;
                        case "Initial key":
                            break;
                        case "Beats-per-minute":
                            break;
                        case "Protected":
                            break;
                        case "Part of a compilation":
                            break;
                        case "Name":
                            break;
                        case "Item type":
                            break;
                        case "Folder path":
                            break;
                        case "Date created":
                            break;
                        case "Date modified":
                            break;
                        case "Size":
                            break;
                        case "Attributes":
                            break;
                        case "Availability":
                            break;
                        case "Offine status":
                            break;
                        case "Shared with":
                            break;
                        case "Owner":
                            break;
                        case "Computer":
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }


    public class FileItemProp
    {
        public string Name { get; set; }
        public string Category { get; set; }
    }
}
