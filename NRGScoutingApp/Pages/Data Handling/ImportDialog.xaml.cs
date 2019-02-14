using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;

namespace NRGScoutingApp {
    public partial class ImportDialog {
        public ImportDialog () {
            InitializeComponent ();
        }

        void cancelClicked (object sender, System.EventArgs e) {
            PopupNavigation.Instance.PopAsync (true);
        }

        void Handle_TextChanged (object sender, Xamarin.Forms.TextChangedEventArgs e) {
            importButton.IsEnabled = !String.IsNullOrWhiteSpace (e.NewTextValue);
        }

        async void importClicked (object sender, System.EventArgs e) {
            JObject data = MatchParameters.initializeEventsObject ();
            try {
                JObject importJSON = (JObject) JsonConvert.DeserializeObject (importData.Text);
                if (importJSON.ContainsKey ("Matches") || importJSON.ContainsKey ("PitNotes")) {
                    if (importJSON.ContainsKey ("Matches")) {
                        if (!data.ContainsKey ("Matches")) {
                            data.Add (new JProperty ("Matches", new JArray ()));
                        }
                        await addMatchItemsChecker (data, importJSON);
                    }
                    if (importJSON.ContainsKey ("PitNotes")) {
                       if (!data.ContainsKey ("PitNotes")) {
                            data.Add (new JProperty ("PitNotes", new JArray ()));
                        }
                        await addPitItemsChecker (data, importJSON);
                    }
                    Preferences.Set ("matchEventsString", JsonConvert.SerializeObject (data));
                    await PopupNavigation.Instance.PopAsync (true);
                } else {
                    await DisplayAlert ("Alert", "Error in Data", "OK");
                }
            } catch (JsonReaderException) {
                await DisplayAlert ("Alert", "Error in Data", "OK");
            }
        }

        private async Task addPitItemsChecker (JObject data, JObject importJSON) {
            JArray temp = (JArray) data["PitNotes"];
            JArray importTemp = (JArray) importJSON["PitNotes"];
            var tempList = temp.ToList ();
            foreach (var match in importTemp.ToList ()) {
                if (tempList.Exists (x => x["team"].Equals (match["team"]))) {
                    var item = tempList.Find (x => x["team"].Equals (match["team"]));
                    temp.Remove (item);
                    for (int i = 0; i < ConstantVars.QUESTIONS.Length; i++) {
                        if (!item["q" + i].Equals (match["q" + i])) {
                            item["q" + i] += ConstantVars.entrySeparator + match["q" + i];
                        }
                    }
                    temp.Add (item);
                } else {
                    temp.Add (match);
                }
            }
        }

        private async Task addMatchItemsChecker (JObject data, JObject importJSON) {
            JArray temp = (JArray) data["Matches"];
            JArray importTemp = (JArray) importJSON["Matches"];
            var tempList = temp.ToList ();
            foreach (var match in importTemp.ToList ()) {
                if (tempList.Exists (x => x["matchNum"].Equals (match["matchNum"]) && x["side"].Equals (match["side"]))) {
                    var item = tempList.Find (x => x["matchNum"].Equals (match["matchNum"]) && x["side"].Equals (match["side"]));
                    if (!item["team"].Equals (match["team"])) {
                        var add = await DisplayAlert ("Warning!", "Match: " + item["matchNum"] +
                            "\nTeam: " + item["team"] +
                            "\nSide: " + MatchFormat.matchSideFromEnum (Convert.ToInt32 (item["side"])) +
                            "\nConflicts with Existing Match", "Overwite", "Ignore");
                        if (add) {
                            temp.Remove (item);
                            temp.Add (match);
                        }
                    }
                } else {
                    temp.Add (match);
                }
            }
        }
    }
}