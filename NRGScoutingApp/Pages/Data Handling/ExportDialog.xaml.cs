using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Clipboard;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NRGScoutingApp {
    public partial class ExportDialog {
        public ExportDialog () {
            InitializeComponent ();
            setExportEntries ();

        }

        private string excelFileBase = "scoutDataExport_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        private string exportText = "";
        private string csvString = "";
        void cancelClicked (object sender, System.EventArgs e) {
            PopupNavigation.Instance.PopAsync (true);
        }
        async void copyClicked (object sender, System.EventArgs e) {
            String exportText = null;
            String action = "";
            while (String.IsNullOrWhiteSpace (action)) {
                action = await DisplayActionSheet ("Choose Export Type:", ConstantVars.exportTypes[0], null, ConstantVars.exportTypes[1], ConstantVars.exportTypes[2], ConstantVars.exportTypes[3]);
            }
            if (action.Equals (ConstantVars.exportTypes[1])) {
                exportText = this.exportText;
                CrossClipboard.Current.SetText (exportText);
            } else if (action.Equals (ConstantVars.exportTypes[2])) {
                JObject datas = JObject.Parse (this.exportText);
                if (datas.ContainsKey ("Matches")) {
                    exportText = JsonConvert.SerializeObject (new JObject (new JProperty ("Matches", (JArray) datas["Matches"])));
                    CrossClipboard.Current.SetText (exportText);
                } else {
                    await DisplayAlert ("Oops!", "No " + ConstantVars.exportTypes[2] + " data", "OK");
                }
            } else if (action.Equals (ConstantVars.exportTypes[3])) {
                JObject datas = JObject.Parse (this.exportText);
                if (datas.ContainsKey ("Matches")) {
                    exportText = JsonConvert.SerializeObject (new JObject (new JProperty ("PitNotes", (JArray) datas["PitNotes"])));
                    CrossClipboard.Current.SetText (exportText);
                } else {
                    await DisplayAlert ("Oops!", "No " + ConstantVars.exportTypes[3] + " data", "OK");
                }
            }
            if (!action.Equals (ConstantVars.exportTypes[0])) {
                await PopupNavigation.Instance.PopAsync (true);
            }
        }

        async void Share_Clicked (object sender, System.EventArgs e) {
            await ShareText (exportText);
        }
        public async Task ShareText (string text) {
            await Share.RequestAsync (new ShareTextRequest {
                Text = text,
                    Title = "Share Match"
            });
        }

        async void Rank_Clicked (object sender, System.EventArgs e) {
            await RankText ();
        }
        private async Task RankText () {
            initCSV ();

            switch (Device.RuntimePlatform) {
                case Device.iOS:
                    string path = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
                    await Share.RequestAsync (new ShareTextRequest {
                        Uri = "file://" + path + "/" + excelFileBase + ".csv",
                            Title = "Share Ranks"
                    });
                    break;
                default:
                    var response = await CrossPermissions.Current.RequestPermissionsAsync (Permission.Storage);
                    String fileDir = Path.Combine (Android.OS.Environment.GetExternalStoragePublicDirectory (Android.OS.Environment.DirectoryDownloads).ToString (), excelFileBase + ".csv");
                    File.WriteAllText (fileDir, "");
                    File.WriteAllText (fileDir, csvString);
                    break;

            }
        }

        //Gets entries from device storage and sets them into the text field (or disables field if empty)
        void setExportEntries () {
            JObject x;
            try {
                x = (JObject) JsonConvert.DeserializeObject (Preferences.Get ("matchEventsString", ""));
            } catch (JsonException) {
                x = new JObject ();
            }
            if (Object.Equals (x, null)) {
                x = new JObject ();
            }
            if (x.Count > 0) {
                String exportEntries = JsonConvert.SerializeObject (
                    JObject.Parse (Preferences.Get ("matchEventsString", "")), Formatting.None);
                exportText = exportEntries;
            } else {
                copyButton.IsEnabled = false;
                shareButton.IsEnabled = false;
                rankExportButton.IsEnabled = false;
                //exportDisplay.Text = "No Entries Yet!";
            }
        }

        async public void initCSV () {
            await Task.Run (async () => {
                Ranker rank = new Ranker (Preferences.Get ("matchEventsString", ""));
                csvString = "Team,Match Num,Side,Avg. Hatch,Num Hatch,Avg. Cargo,Num Cargo,Climb,Lvl1,Lvl2,Lvl3,Cargoship\n";
                JArray matches = rank.getDataAsJArray ();
                CSVRanker singleMatch = new CSVRanker ();
                foreach (JObject match in matches) {
                    csvString += singleMatch.matchCalc (match) + "\n";
                }
                string path = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
                File.WriteAllText (path + "/" + excelFileBase + ".csv", csvString);
            });
        }
    }
}