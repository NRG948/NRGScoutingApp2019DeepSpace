using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Clipboard;
using Plugin.CloudFirestore;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.FirebaseAuth;
using System.Linq;

namespace NRGScoutingApp
{
    public partial class ExportDialog
    {
        public ExportDialog()
        {
            InitializeComponent();
            setExportEntries();

        }

        private string excelFileBase = "scoutDataExport_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        private string pitFileBase = "pitDataExport_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        private string exportText = "";
        private string csvString = "";
        private JObject JSONData;
        void cancelClicked(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
        async void copyClicked(object sender, System.EventArgs e)
        {
            String exportText = null;
            String action = "";
            while (String.IsNullOrWhiteSpace(action))
            {
                action = await DisplayActionSheet("Choose Export Type:", ConstantVars.exportTypes[0], null, ConstantVars.exportTypes[1], ConstantVars.exportTypes[2], ConstantVars.exportTypes[3], ConstantVars.exportTypes[4]);
            }
            if (action.Equals(ConstantVars.exportTypes[1]))
            {
                exportText = this.exportText;
                CrossClipboard.Current.SetText(exportText);
            }
            else if (action.Equals(ConstantVars.exportTypes[2]))
            {
                JObject datas = JObject.Parse(this.exportText);
                if (datas.ContainsKey("Matches"))
                {
                    exportText = JsonConvert.SerializeObject(new JObject(new JProperty("Matches", (JArray)datas["Matches"])));
                    CrossClipboard.Current.SetText(exportText);
                }
                else
                {
                    await DisplayAlert("Oops!", "No " + ConstantVars.exportTypes[2] + " data", "OK");
                }
            }
            else if (action.Equals(ConstantVars.exportTypes[3]))
            {
                JObject datas = JObject.Parse(this.exportText);
                if (datas.ContainsKey("PitNotes"))
                {
                    exportText = JsonConvert.SerializeObject(new JObject(new JProperty("PitNotes", (JArray)datas["PitNotes"])));
                    CrossClipboard.Current.SetText(exportText);
                }
                else
                {
                    await DisplayAlert("Oops!", "No " + ConstantVars.exportTypes[3] + " data", "OK");
                }
            }
            else if (action.Equals(ConstantVars.exportTypes[4]))
            {
                await pasteRequest();
            }
            if (!action.Equals(ConstantVars.exportTypes[0]))
            {
                await PopupNavigation.Instance.PopAsync(true);
            }
        }

        async private Task pasteRequest()
        {
            String temp = exportText.Replace("&", "and");
            var s = new WebClient();
            String action = "";
            while (String.IsNullOrWhiteSpace(action))
            {
                action = await DisplayActionSheet("Choose PasteBin Export Type:", ConstantVars.exportTypes[0], null, ConstantVars.exportTypes[1], ConstantVars.exportTypes[2], ConstantVars.exportTypes[3]);
            }
            if (action.Equals(ConstantVars.exportTypes[1]))
            {
                temp = this.exportText;
            }
            else if (action.Equals(ConstantVars.exportTypes[2]))
            {
                JObject datas = JObject.Parse(temp);
                if (datas.ContainsKey("Matches"))
                {
                    temp = JsonConvert.SerializeObject(new JObject(new JProperty("Matches", (JArray)datas["Matches"])));
                }
                else
                {
                    await DisplayAlert("Oops!", "No " + ConstantVars.exportTypes[2] + " data", "OK");
                }
            }
            else if (action.Equals(ConstantVars.exportTypes[3]))
            {
                JObject datas = JObject.Parse(temp);
                if (datas.ContainsKey("PitNotes"))
                {
                    temp = JsonConvert.SerializeObject(new JObject(new JProperty("PitNotes", (JArray)datas["PitNotes"])));
                }
                else
                {
                    await DisplayAlert("Oops!", "No " + ConstantVars.exportTypes[3] + " data", "OK");
                }
            }

            try
            {
                WebRequest req = WebRequest.Create("http://pastebin.com/api/api_post.php");

                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                string postData = "api_option=" + "paste" + "&api_paste_code=" + temp + "&api_dev_key=" + "00baaa90c56ad5ed1e460c332b23f7c6";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                req.ContentLength = byteArray.Length;

                Stream ds = req.GetRequestStream();
                ds.Write(byteArray, 0, byteArray.Length);
                ds.Close();
                WebResponse wr = req.GetResponse();
                ds = wr.GetResponseStream();
                StreamReader reader = new StreamReader(ds);

                String ret = await reader.ReadToEndAsync();
                CrossClipboard.Current.SetText(ret);
                await DisplayAlert("Success", "Pastebin Link Copied to Clipboard", "OK");
            }
            catch
            {
                await DisplayAlert("Error", "No Internet!", "OK");
            }
        }

        async void fireBaseClicked(object sender, System.EventArgs e)
        {
            //if () { }
            //else { }
            if (!String.IsNullOrWhiteSpace(Preferences.Get("loginTeamNum", "")))
            {
                try
                {
                    fireBase.IsEnabled = false;
                    ImportDialog temp = new ImportDialog();
                    JObject data = new JObject(JSONData);
                    fireBase.Text = "Setting Data...";
                    IDocumentSnapshot teamAssociations = await CrossCloudFirestore.Current.Instance.
                    GetCollection("TeamLogins").
                    GetDocument("LoginJSON").
                    GetDocumentAsync();
                    JObject uid = JObject.Parse(teamAssociations.Data["UIDConnections"].ToString());
                    String loginTeam = Preferences.Get("loginTeamNum", "");
                    String uidCurrent = CrossFirebaseAuth.Current.Instance.CurrentUser.Uid;
                    if (uid.ContainsKey(loginTeam) && uid[loginTeam].ToList().Contains(uidCurrent))
                    {
                        IDocumentSnapshot document = await CrossCloudFirestore.Current
                                                    .Instance
                                                    .GetCollection(ConstantVars.APP_YEAR)
                                                    .GetDocument(loginTeam)
                                                    .GetDocumentAsync();
                        JObject cloudData = JObject.Parse(document.Data["AllData"].ToString());
                        await temp.getCombinedJSON(cloudData, data);
                        await CrossCloudFirestore.Current
                                .Instance
                                .GetCollection(ConstantVars.APP_YEAR)
                                .GetDocument(loginTeam)
                                .UpdateDataAsync(new Dictionary<string, object> { ["AllData"] = JsonConvert.SerializeObject(data) });
                        fireBase.IsEnabled = false;
                    }
                }
                catch (CloudFirestoreException s)
                {
                    fireBase.Text = "FireBase Test";
                    Console.WriteLine(s.StackTrace);
                }
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new AuthPage());
            }
        }

        void LogoutClicked(object sender, System.EventArgs e)
        {
            CrossFirebaseAuth.Current.Instance.SignOut();
            Preferences.Remove("loginTeamNum");
        }

        async void Share_Clicked(object sender, System.EventArgs e)
        {
            await ShareText(exportText);
        }

        public async Task ShareText(string text)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Share Match"
            });
        }

        async void Rank_Clicked(object sender, System.EventArgs e)
        {
            String action = "";
            while (String.IsNullOrWhiteSpace(action))
            {
                action = await DisplayActionSheet("Choose PasteBin Export Type:", ConstantVars.exportTypes[0], null, ConstantVars.exportTypes[2], ConstantVars.exportTypes[3]);
            }
            if (action.Equals(ConstantVars.exportTypes[2]))
            {
                await RankText();
            }
            else if (action.Equals(ConstantVars.exportTypes[3]))
            {
                await PitText();
            }

        }

        private async Task PitText()
        {
            await Task.Run(async () =>
            {
                Ranker rank = new Ranker(Preferences.Get("matchEventsString", ""));
                csvString = "Team";
                foreach (String s in ConstantVars.QUESTIONS)
                {
                    csvString += "," + s;
                }
                csvString += "\n";
                JObject tempJSON;
                JArray pits;
                if (!String.IsNullOrWhiteSpace(Preferences.Get("matchEventsString", "")))
                {
                    try
                    {
                        tempJSON = JObject.Parse(Preferences.Get("matchEventsString", ""));
                    }
                    catch (NullReferenceException)
                    {
                        System.Diagnostics.Debug.WriteLine("Caught NullRepEx for ranker JObject");
                        tempJSON = new JObject();
                    }
                }
                else
                {
                    tempJSON = new JObject(new JProperty("PitNotes"));
                }
                if (tempJSON.ContainsKey("PitNotes"))
                {
                    pits = (JArray)tempJSON["PitNotes"];
                }
                else
                {
                    pits = new JArray();
                }
                for (int i = 0; i < pits.Count; i++)
                {
                    csvString += pits[i]["team"];
                    for (int j = 0; j < ConstantVars.QUESTIONS.Length; j++)
                    {
                        try
                        {
                            String ss = (pits[i]["q" + j]).ToString().Replace("\n", "");
                            csvString += "," + ss;
                        }
                        catch
                        {
                            csvString += ",";
                        }
                    }
                    csvString += "\n";
                }
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                File.WriteAllText(path + "/" + pitFileBase + ".csv", csvString);
            });
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    await Share.RequestAsync(new ShareTextRequest
                    {
                        Uri = "file://" + path + "/" + pitFileBase + ".csv",
                        Title = "Share Pit"
                    });
                    break;
                default:
                    //var response = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    //String fileDir = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString(), pitFileBase + ".csv");
                    //File.WriteAllText(fileDir, "");
                    //File.WriteAllText(fileDir, csvString);
                    break;

            }
        }
        private async Task RankText()
        {
            initCSV();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    await Share.RequestAsync(new ShareTextRequest
                    {
                        Uri = "file://" + path + "/" + excelFileBase + ".csv",
                        Title = "Share Ranks"
                    });
                    break;
                default:
                    //var response = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    //String fileDir = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString(), excelFileBase + ".csv");
                    //File.WriteAllText(fileDir, "");
                    //File.WriteAllText(fileDir, csvString);
                    break;

            }
        }

        //Gets entries from device storage and sets them into the text field (or disables field if empty)
        void setExportEntries()
        {
            try
            {
                JSONData = (JObject)JsonConvert.DeserializeObject(Preferences.Get("matchEventsString", ""));
            }
            catch (JsonException)
            {
                JSONData = new JObject();
            }
            if (Object.Equals(JSONData, null))
            {
                JSONData = new JObject();
            }
            if (JSONData.Count > 0)
            {
                String exportEntries = JsonConvert.SerializeObject(
                    JObject.Parse(Preferences.Get("matchEventsString", "")), Formatting.None);
                exportText = exportEntries;
            }
            else
            {
                copyButton.IsEnabled = false;
                shareButton.IsEnabled = false;
                rankExportButton.IsEnabled = false;
                //exportDisplay.Text = "No Entries Yet!";
            }
        }

        async public void initCSV()
        {
            await Task.Run(async () =>
            {
                Ranker rank = new Ranker(Preferences.Get("matchEventsString", ""));
                csvString = "Team,Match Num,Side,Avg. Hatch,Num Hatch,Avg. Cargo,Num Cargo,Climb,Lvl1,Lvl2,Lvl3,Cargoship\n";
                JArray matches = rank.getDataAsJArray();
                CSVRanker singleMatch = new CSVRanker();
                foreach (JObject match in matches)
                {
                    csvString += singleMatch.matchCalc(match) + "\n";
                }
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                File.WriteAllText(path + "/" + excelFileBase + ".csv", csvString);
            });
        }
    }
}