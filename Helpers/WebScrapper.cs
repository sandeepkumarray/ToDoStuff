using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ToDoStuff.Model;
using ToDoStuff.Utility;

namespace ToDoStuff.Helpers
{
    public class WebScrapper
    {
        WebScrapperSettings Settings = null;
        Logger log;

        public WebScrapper(WebScrapperSettings settings, Logger Log)
        {
            Settings = settings;
            log = Log;
        }

        public void Process(bool isProcess = true)
        {
            int reTryCounter = 0;
            HtmlWeb web = new HtmlWeb();

            web.PreRequest = delegate (HttpWebRequest webRequest)
            {
                webRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                webRequest.Timeout = 600000;
                webRequest.ReadWriteTimeout = 600000;
                return true;
            };
            do
            {
                try
                {
                    List<string> ScrapedLinkList = new List<string>();

                    if (this.Settings.Profile.Name == "directImages")
                    {
                        ScrapedLinkList.Add(this.Settings.WebsiteUrl);
                    }
                    else if (this.Settings.Profile.Name == "instagram" || this.Settings.Profile.Name == "insta")
                    {
                        process_Instagram_Url(web);
                    }
                    else
                    {
                        var htmlDoc = web.Load(this.Settings.WebsiteUrl);

                        log.Info("Website Loaded");
                        Thread.Sleep(9999);
                        if (this.Settings.Profile.PatternToSearch == "script")
                        {
                            if (this.Settings.Profile.Name == "youtube")
                            {
                                ScrapedLinkList = YouTubeScriptImages(htmlDoc);
                            }
                        }
                        else
                        {
                            HtmlAgilityPack.HtmlNodeCollection nodeCollection = htmlDoc.DocumentNode.SelectNodes("//" + this.Settings.Profile.PatternToSearch);////meta[@property='og:image']

                            log.Info("TypeOfData :- " + Settings.Profile.TypeOfData);
                            if (nodeCollection != null)
                                log.Info("Found count :-" + nodeCollection.Count);

                            bool gotValue = false;

                            if (String.IsNullOrEmpty(Settings.Profile.AttributeName))
                            {
                                foreach (HtmlNode td in nodeCollection)
                                {
                                    if (td.Attributes.Contains("content"))
                                    {
                                        gotValue = true;
                                        ScrapedLinkList.Add(td.Attributes["content"].Value);
                                    }

                                    else if (td.Attributes.Contains("src"))
                                    {
                                        gotValue = true;
                                        ScrapedLinkList.Add(td.Attributes["src"].Value);
                                    }

                                    else if (td.Attributes.Contains("data"))
                                    {
                                        gotValue = true;
                                        ScrapedLinkList.Add(td.Attributes["data"].Value);
                                    }

                                    else if (!string.IsNullOrEmpty(td.InnerHtml))
                                    {
                                        gotValue = true;
                                        ScrapedLinkList.Add(td.InnerHtml);
                                    }
                                }
                            }
                            else
                            {
                                foreach (HtmlNode td in nodeCollection)
                                {
                                    if (td.Attributes.Contains(Settings.Profile.AttributeName))
                                    {
                                        gotValue = true;
                                        ScrapedLinkList.Add(td.Attributes[Settings.Profile.AttributeName].Value);
                                    }
                                }

                            }

                            if (gotValue == false)
                            {
                                MessageBox.Show("No Values to process from URL.");
                                log.Warn("No Values to process from URL.");
                                return;
                            }


                        }

                    }
                    if (isProcess)
                    {
                        switch (Settings.Profile.TypeOfData)
                        {
                            case "Image":
                                DownloadAndSaveImages(ScrapedLinkList);
                                break;
                            case "Text":
                                DownloadAndSaveText(ScrapedLinkList);
                                break;
                            case "PDF":
                                DownloadAndSavePDF(ScrapedLinkList);
                                break;
                            case "Word":
                                DownloadAndSaveWord(ScrapedLinkList);
                                break;
                            case "Video":
                                DownloadAndSaveVideo(ScrapedLinkList);
                                break;
                            default:
                                break;
                        }
                    }


                    log.Info("All Files downloaded and saved.");
                    reTryCounter = 200;

                }
                catch (WebException ex)
                {
                    reTryCounter++;
                    if (reTryCounter == 19)
                    {
                        MessageBox.Show("Error Program 1121 , Download webpage \n" + ex.ToString());

                        log.Error(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            } while (reTryCounter < 19);
        }

        private void process_Instagram_Url(HtmlWeb web)
        {
            try
            {

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private List<string> YouTubeScriptImages(HtmlDocument htmlDoc)
        {
            List<string> result = new List<string>();
            HtmlAgilityPack.HtmlNodeCollection nodeCollection = htmlDoc.DocumentNode.SelectNodes("//" + this.Settings.Profile.PatternToSearch);////meta[@property='og:image']

            if (nodeCollection != null)
            {
                foreach (var node in nodeCollection)
                {
                    if (node.InnerText.StartsWith("var ytInitialData"))
                    {
                        string[] parts = node.InnerText.Split('=');
                        string jsonPart = parts[1].Trim().Substring(0, parts[1].Trim().Length - 1);
                        JObject jsonObject = JsonConvert.DeserializeObject<JObject>(jsonPart);
                    }
                }
            }
            return result;

        }

        private void DownloadAndSaveVideo(List<string> scrapedLinkList)
        {
            try
            {
                Thread.Sleep(2000);

                foreach (var link in scrapedLinkList)
                {
                    JObject jsonObject = JsonConvert.DeserializeObject<JObject>(link);
                    if (jsonObject != null)
                    {
                        var pckList = jsonObject["resourceResponses"];//== null ? jsonObject["resources"] : null;
                        if (pckList != null)
                        {
                            foreach (var pck in pckList)
                            {
                                if (Convert.ToString(((Newtonsoft.Json.Linq.JValue)pck["name"]).Value) == "PinResource")
                                {
                                    //((Newtonsoft.Json.Linq.JValue)pck["response"]["data"]["videos"]["video_list"]["V_720P"]["url"]).Value
                                    var ResponseDataChildren = ((Newtonsoft.Json.Linq.JObject)pck["response"]["data"]).Children().ToList();
                                    var videos = ResponseDataChildren.Find(x => ((Newtonsoft.Json.Linq.JProperty)x).Name == "videos");
                                    var videosChildren = ((Newtonsoft.Json.Linq.JProperty)videos).Children().ToList();

                                    JToken video_list = null;
                                    if (videosChildren != null && videosChildren.Count > 0)
                                    {
                                        Type childType = videosChildren[0].GetType();

                                        if (childType == typeof(Newtonsoft.Json.Linq.JValue))
                                            video_list = videosChildren.Find(x => Convert.ToString(((Newtonsoft.Json.Linq.JValue)x).Value) == "video_list");

                                        else if (childType == typeof(Newtonsoft.Json.Linq.JProperty))
                                            video_list = videosChildren.Find(x => Convert.ToString(((Newtonsoft.Json.Linq.JProperty)x).Value) == "video_list");

                                        else if (childType == typeof(Newtonsoft.Json.Linq.JObject))
                                        {
                                            var videosChildren_Children = ((Newtonsoft.Json.Linq.JObject)videosChildren[0]).Children().ToList();
                                            video_list = videosChildren_Children.Find(x => Convert.ToString(((Newtonsoft.Json.Linq.JProperty)x).Name) == "video_list");
                                        }

                                    }

                                    if (video_list == null)
                                    {
                                        var story_pin_data_video = pck["response"]["data"]["story_pin_data"]["pages"][0]["blocks"][0]["video"];
                                        var story_pin_data_video_Children = ((Newtonsoft.Json.Linq.JObject)story_pin_data_video).Children().ToList();

                                        video_list = story_pin_data_video_Children.Find(x => Convert.ToString(((Newtonsoft.Json.Linq.JProperty)x).Name) == "video_list");

                                    }

                                    string title = Convert.ToString(((Newtonsoft.Json.Linq.JValue)pck["response"]["data"]["title"]).Value);

                                    var video_list_Children = video_list.Children().ToList();

                                    if (video_list_Children != null && video_list_Children.Count() > 0)
                                    {
                                        var listItem = video_list_Children[0];
                                        var video_list_Children_Children = listItem.Children().ToList();

                                        var V_P = video_list_Children_Children.Find(x => Convert.ToString(((Newtonsoft.Json.Linq.JProperty)x).Name) == "V_EXP7"
                                        || Convert.ToString(((Newtonsoft.Json.Linq.JProperty)x).Name) == "V_720P");

                                        var V_PChildren = V_P.Children().ToList();
                                        var V_PChildren_Children = V_PChildren.Children().ToList();

                                        var V_EXP7_url = V_PChildren_Children.Find(x => Convert.ToString(((Newtonsoft.Json.Linq.JProperty)x).Name) == "url");
                                        string url = Convert.ToString(((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)V_EXP7_url).Value).Value);

                                        DownloadURLData(url, title.CleanFileName());
                                    }

                                    //if (video_list["V_720P"] != null)
                                    //{
                                    //    string url = Convert.ToString(((Newtonsoft.Json.Linq.JValue)video_list["V_720P"]["url"]).Value);
                                    //    DownloadURLData(url, title);
                                    //}
                                }
                            }
                        }
                        else
                        {
                            string title = Convert.ToString(((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)(jsonObject["resources"]["PinResource"].First)).Value["data"]["title"]).Value);

                            var url = Convert.ToString(((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)(jsonObject["resources"]["PinResource"].First)).Value["data"]["videos"]["video_list"]["V_720P"]["url"]).Value);

                            DownloadURLData(url, title.CleanFileName());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void DownloadURLData(string url, string title)
        {

            string subFolder = System.IO.Path.GetFileNameWithoutExtension(this.Settings.WebsiteUrl);
            string filename = title + DateTime.Now.ToString("_dd_MMM_yyy_hh_mm_sss") + ".mp4";

            Uri uri = new Uri(url);

            if (string.IsNullOrEmpty(title))
            {
                filename = "New_video_" + DateTime.Now.ToString("dd_MMM_yyy_hh_mm_sss") + ".mp4";
            }

            CallWebClient(url, filename, subFolder);
        }

        public List<string> GetScrapedLinkList()
        {
            List<string> scrapedLinkList = new List<string>();
            try
            {
                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(this.Settings.WebsiteUrl);

                log.Info("Website Loaded");

                HtmlAgilityPack.HtmlNodeCollection nodeCollection = htmlDoc.DocumentNode.SelectNodes("//" + this.Settings.Profile.PatternToSearch);////meta[@property='og:image']

                log.Info("TypeOfData :- " + Settings.Profile.TypeOfData);

                log.Info("Found count :-" + nodeCollection.Count);

                foreach (HtmlNode td in nodeCollection)
                {
                    if (td.Attributes.Contains("content"))
                        scrapedLinkList.Add(td.Attributes["content"].Value);

                    if (td.Attributes.Contains("src"))
                        scrapedLinkList.Add(td.Attributes["src"].Value);

                    if (td.Attributes.Contains("data"))
                        scrapedLinkList.Add(td.Attributes["data"].Value);
                }
            }
            catch (Exception ex)
            {
                log.Info("Exception : -" + ex.Message);
            }
            return scrapedLinkList;
        }

        public void DownloadAndSaveWord(List<string> scrapedLinkList)
        {

        }

        public void DownloadAndSavePDF(List<string> scrapedLinkList)
        {

        }

        public void DownloadAndSaveText(List<string> scrapedLinkList)
        {

        }

        public void DownloadAndSaveImages(List<string> scrapedLinkList, RichTextBox LogControl = null)
        {
            string subFolder = System.IO.Path.GetFileNameWithoutExtension(this.Settings.WebsiteUrl);

            if (string.IsNullOrEmpty(subFolder))
            {
                string[] urlParts = this.Settings.WebsiteUrl.Split('/');
                if (urlParts.Count() == 1)
                {
                    subFolder = urlParts[0];
                }

                if (urlParts.Count() == 2)
                {
                    subFolder = urlParts[1];
                }

                if (urlParts.Count() == 3)
                {
                    subFolder = urlParts[2];
                }

                if (urlParts.Count() > 3)
                {
                    subFolder = urlParts[urlParts.Length - 3] + "/" + urlParts[urlParts.Length - 2];
                }
            }

            log.Info("Sub Folder : " + subFolder);

            foreach (var link in scrapedLinkList)
            {
                string url = link;
                try
                {
                    string filename = "New_Image" + DateTime.Now.ToString("dd_MMM_yyy_hh_mm_sss") + ".jpg";

                    if (url.StartsWith("{") || url.StartsWith("["))
                    {
                        if (Settings.Profile.Name == "FB_Image")
                        {
                            FBImage fbimage = JsonConvert.DeserializeObject<FBImage>(url);

                            if (fbimage != null)
                            {
                                url = fbimage.image.contentUrl;
                            }
                        }
                    }

                    Uri uri = new Uri(url);

                    if (!string.IsNullOrEmpty(uri.LocalPath))
                    {
                        filename = System.IO.Path.GetFileName(uri.LocalPath);
                    }
                    CallWebClient(url, filename, subFolder);

                    log.Info("Link : " + url);
                }
                catch (Exception ex)
                {
                    if (this.Settings.Profile.SubFolder)
                    {
                        log.Error("Sub-Folder : " + subFolder + ",\tLink : " + link + ",\tException : " + ex.Message);
                    }
                    else
                        log.Error("Link : " + link + ",\tException : -" + ex.Message);
                }

            }
        }

        public void CallWebClient(string link, string filename, string subFolder = null)
        {
            WebClientNew wc = new WebClientNew();
            string folderPath = Path.Combine(this.Settings.Profile.OutputFolder, filename);

            if (this.Settings.Profile.SubFolder)
            {
                folderPath = Path.Combine(this.Settings.Profile.OutputFolder, subFolder, filename);
            }

            using (MemoryStream stream = new MemoryStream(wc.DownloadData(link)))
            {
                stream.CopyTo(FileUtility.GetFileStream(folderPath));
            }

        }
    }

    public class WebScrapperSettings
    {
        public string WebsiteUrl { get; set; }
        public bool IsProcess { get; set; }
        public WebScrapProfile Profile { get; set; }
        public WebScrapperSettings()
        {
            Profile = new WebScrapProfile();
        }
        public WebScrapperSettings(WebScrapProfile profile)
        {
            this.Profile = profile;
        }
    }

    public class WebScrapProfile
    {
        public string AttributeName { get; set; }
        public string PatternToSearch { get; set; }
        public string TypeOfData { get; set; }
        public string OutputFolder { get; set; }
        public bool SubFolder { get; set; }
        public string Name { get; set; }
    }
}
