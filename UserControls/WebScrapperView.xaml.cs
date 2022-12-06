using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using ToDoStuff.Utility;

namespace ToDoStuff.UserControls
{
    /// <summary>
    /// Interaction logic for WebScrapperView.xaml
    /// </summary>
    [ControlNameAttribute("Web Scrap")]
    public partial class WebScrapperView : UserControl, IControlsInterface
    {
        private readonly BackgroundWorker ProcessWorker = new BackgroundWorker();
        List<WebScrapperSettings> SettingsList = null;
        List<NlogViewer.LogEventViewModel> AllLogList = null;

        Logger log = LogManager.GetLogger("Web Scrap");

        CancellationTokenSource _cancelLogTask = new CancellationTokenSource();
        ObservableCollectionFast<WebScrapProfile> ProfileList = new ObservableCollectionFast<WebScrapProfile>();
        ObservableCollectionFast<string> TypeList = new ObservableCollectionFast<string>();

        string ProfileFileLocation = @"E:\AppDumps\ScrapOut\ScrapProfiles.json";

        public WebScrapperView()
        {
            InitializeComponent();
            logCtrl.ItemAdded += LogCtrl_ItemAdded;
            AllLogList = new List<NlogViewer.LogEventViewModel>();
            //SetTestData();

            TypeList.Add("All");
            TypeList.Add("Image");
            TypeList.Add("Video");
            TypeList.Add("Text");
            TypeList.Add("PDF");
            TypeList.Add("Word");
            cbTypeOfData.ItemsSource = TypeList;

            LoadProfiles();
        }

        private void LoadProfiles()
        {
            //WebScrapProfile profile = new WebScrapProfile();
            //profile.Name = "pinterest_video";
            //profile.PatternToSearch = "script[@id='initial-state']";
            //profile.SubFolder = false;
            //profile.TypeOfData = "Video";
            //profile.OutputFolder = @"C:\Users\sande\Desktop\ScrapOut\";
            //ProfileList.Add(profile);

            //profile = new WebScrapProfile();
            //profile.Name = "pinterest_image";
            //profile.PatternToSearch = "meta[@property='og:image']";
            //profile.SubFolder = false;
            //profile.TypeOfData = "Image";
            //profile.OutputFolder = @"C:\Users\sande\Desktop\ScrapOut\";
            //ProfileList.Add(profile);

            //profile = new WebScrapProfile();
            //profile.Name = "520mojing_image";
            //profile.PatternToSearch = "img[@class='zoom']";
            //profile.SubFolder = true;
            //profile.TypeOfData = "Image";
            //profile.OutputFolder = @"C:\Users\sande\Desktop\ScrapOut\";
            //ProfileList.Add(profile);

            string profileJson = File.ReadAllText(ProfileFileLocation);
            ProfileList = JsonConvert.DeserializeObject<ObservableCollectionFast<WebScrapProfile>>(profileJson);

            cbProfiles.ItemsSource = ProfileList;
            cbProfiles.DisplayMemberPath = "Name";

        }

        private void SetTestData()
        {
            rbUrl.IsChecked = true;
            txtLinkFilePath.Text = @"C:\Users\sande\Desktop\ScrapOut\mojing_List.txt";
            txtURL.Text = "https://in.pinterest.com/pin/48202658500647987/";
            txtPatternToSearch.Text = "script[@id='initial-state']";
            txtSubFolder.IsChecked = true;
            cbTypeOfData.SelectedIndex = 2;
            txtOutputPath.Text = @"C:\Users\sande\Desktop\ScrapOut\";
        }

        private void LogCtrl_ItemAdded(object sender, EventArgs e)
        {
            LogEventInfo logInfo = (NLogEvent)e;
            AllLogList.Add(new NlogViewer.LogEventViewModel(logInfo));
        }

        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }

        private void ProcessURLs(object obj)
        {
            List<WebScrapperSettings> settingsList = (List<WebScrapperSettings>)obj;

            int counter = 0;
            Logger log = NLog.LogManager.GetLogger("task");

            log.Debug("Backgroundtask started.");


            foreach (var scrapSet in SettingsList)
            {
                counter++;
                log.Info("Processing URL : " + scrapSet.WebsiteUrl);
                WebScrapper webScrapper = new WebScrapper(scrapSet, log);
                webScrapper.Process(scrapSet.IsProcess);
            }

            _cancelLogTask.Cancel();
            log.Debug("Backgroundtask stopped.");
        }
        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            List<string> URLList = new List<string>();
            try
            {
                SettingsList = new List<WebScrapperSettings>();

                if (rbList.IsChecked == true)
                {
                    URLList = File.ReadAllLines(txtLinkFilePath.Text.Trim()).ToList();
                }

                if (rbUrl.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(txtURL.Text.Trim()))
                    {
                        URLList.Add((txtURL.Text.Trim()));
                    }
                }

                if (rbPaste.IsChecked == true)
                {
                    URLList = txtPasteUrls.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
                }

                foreach (var url in URLList)
                {
                    WebScrapperSettings webScrapperSettings = GetWebScrapperSettings(url);
                    webScrapperSettings.IsProcess = true;
                    SettingsList.Add(webScrapperSettings);
                }



                _cancelLogTask = new CancellationTokenSource();
                var token = _cancelLogTask.Token;
                Task _logTask = new Task(ProcessURLs, SettingsList, token);
                
                _logTask.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            finally
            {
                FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ScrapOut", "Batch_" + DateTime.Now.ToString("dd_MMM_yyy_hh_mm_sss") + ".txt"), string.Join("\r\n", URLList), true);
            }
        }

        private WebScrapperSettings GetWebScrapperSettings(string Url)
        {
            WebScrapperSettings webScrapperSettings = new WebScrapperSettings();
            webScrapperSettings.WebsiteUrl = Url;
            webScrapperSettings.Profile = (WebScrapProfile)cbProfiles.SelectedItem;
            //webScrapperSettings.Profile.TypeOfData = Convert.ToString(cbTypeOfData.SelectionBoxItem);
            //webScrapperSettings.Profile.PatternToSearch = txtPatternToSearch.Text.Trim();
            //webScrapperSettings.Profile.SubFolder = txtSubFolder.IsChecked == true ? true : false;
            //webScrapperSettings.Profile.Name = ((WebScrapProfile)cbProfiles.SelectedItem).Name;

            if (string.IsNullOrEmpty(txtOutputPath.Text.Trim()))
            {
                webScrapperSettings.Profile.OutputFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ScrapOut");
            }
            else
            {
                webScrapperSettings.Profile.OutputFolder = txtOutputPath.Text.Trim();
            }

            return webScrapperSettings;
        }

        private void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            List<NlogViewer.LogEventViewModel> LogItemList = new List<NlogViewer.LogEventViewModel>();

            foreach (var item in logCtrl.LogView.ItemsSource)
            {
                LogItemList.Add((NlogViewer.LogEventViewModel)item);
            }

            DataSet ds = AllLogList.ConvertToDataSet("Log Table");

            if (ds != null)
            {
                ExcelUtility excelUtil = new ExcelUtility();
                excelUtil.CreateExcelFile(ds, System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ScrapOut", DateTime.Now.ToString("dd_MMM_yyy_hh_mm_sss") + ".xlsx"));
                MessageBox.Show("Exported.");
            }
        }

        private void cbProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProfiles.SelectedItem != null)
            {
                WebScrapProfile profile = (WebScrapProfile)cbProfiles.SelectedItem;

                txtPatternToSearch.Text = profile.PatternToSearch;
                txtSubFolder.IsChecked = profile.SubFolder;
                int indexTOD = cbTypeOfData.Items.IndexOf(profile.TypeOfData);
                cbTypeOfData.SelectedIndex = indexTOD;
                txtOutputPath.Text = profile.OutputFolder;
                txtEleAttr.Text = profile.AttributeName;
            }
        }

        private void btnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            string profileJson = File.ReadAllText(ProfileFileLocation);
            WebScrapProfile profile = new WebScrapProfile();

            GetTheValue getTheValue = new GetTheValue();
            getTheValue.ShowDialog();
            profile.Name = getTheValue.txtValue.Text.Trim();
            profile.TypeOfData = Convert.ToString(cbTypeOfData.SelectionBoxItem);
            profile.PatternToSearch = txtPatternToSearch.Text.Trim();
            profile.SubFolder = txtSubFolder.IsChecked == true ? true : false;
            profile.OutputFolder = txtOutputPath.Text.Trim();
            profile.AttributeName = txtEleAttr.Text.Trim();

            ProfileList.Add(profile);

            profileJson = JsonConvert.SerializeObject(ProfileList, Formatting.Indented);
            FileUtility.SaveDataToFile(ProfileFileLocation, profileJson, true);
        }

        private void btnCheckUrl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SettingsList = new List<WebScrapperSettings>();

                List<string> URLList = new List<string>();
                if (rbList.IsChecked == true)
                {
                    URLList = File.ReadAllLines(txtLinkFilePath.Text.Trim()).ToList();
                }

                if (rbUrl.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(txtURL.Text.Trim()))
                    {
                        URLList.Add((txtURL.Text.Trim()));
                    }
                }

                foreach (var url in URLList)
                {
                    WebScrapperSettings webScrapperSettings = GetWebScrapperSettings(url);
                    webScrapperSettings.IsProcess = false;
                    SettingsList.Add(webScrapperSettings);
                }

                _cancelLogTask = new CancellationTokenSource();
                var token = _cancelLogTask.Token;
                Task _logTask = new Task(ProcessURLs, SettingsList, token);
                _logTask.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void btnUpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            if (cbProfiles.SelectedItem != null)
            {
                WebScrapProfile profile = (WebScrapProfile)cbProfiles.SelectedItem;
                profile.TypeOfData = Convert.ToString(cbTypeOfData.SelectionBoxItem);
                profile.PatternToSearch = txtPatternToSearch.Text.Trim();
                profile.SubFolder = txtSubFolder.IsChecked == true ? true : false;
                profile.OutputFolder = txtOutputPath.Text.Trim();
                profile.AttributeName = txtEleAttr.Text.Trim();

                var found = ProfileList.Any(x => x.Name == profile.Name);
                if(found)
                {
                    int i = ProfileList.IndexOf(profile);
                    ProfileList[i] = profile;
                    cbProfiles.SelectedItem = profile;
                }

                var profileJson = JsonConvert.SerializeObject(ProfileList, Formatting.Indented);
                FileUtility.SaveDataToFile(ProfileFileLocation, profileJson, true);
            }
        }
    }
}
