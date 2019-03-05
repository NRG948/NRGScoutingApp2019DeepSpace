using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Clipboard;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;

namespace NRGScoutingApp {
    public partial class ExportDialog {
        public ExportDialog () {
            InitializeComponent ();
            setExportEntries ();
        }

        private string exportText = "";
        private string csvString = "";
        void cancelClicked (object sender, System.EventArgs e) {
            PopupNavigation.Instance.PopAsync (true);
        }
        void copyClicked (object sender, System.EventArgs e) {
            CrossClipboard.Current.SetText (exportText);
            PopupNavigation.Instance.PopAsync (true);
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
        public async Task RankText () {
            initCSV();
            string path = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
            await Share.RequestAsync (new ShareTextRequest {
                Uri = "file://" + path + "/excel.csv",
                    Title = "Share Ranks"
            });
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
                csvString = "Team,Match Num,Side,Avg. Hatch,Avg. Cargo,Climb,Lvl1,Lvl2,Lvl3,Cargoship\n";
                JArray matches = rank.getDataAsJArray ();
                CSVRanker singleMatch = new CSVRanker ();
                foreach (JObject match in matches) {
                    csvString += singleMatch.matchCalc (match) + "\n";
                }
                string path = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
                File.WriteAllText (path + "/excel.csv", csvString);
            });
        }
    }
}