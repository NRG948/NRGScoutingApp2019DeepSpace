using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using System.Collections;
using System.Collections.Generic;

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
                        try
                        {
                            if (!item["q" + i].Equals(match["q" + i]))
                            {
                                List<String> vals = new List<String>();
                                String[] import = { match["q" + i].ToString() };
                                String[] existing = { item["q" + i].ToString() };
                                if (match["q" + i].ToString().Contains(ConstantVars.entrySeparator))
                                {
                                    try
                                    {
                                        import = match["q" + i].ToString().Split(ConstantVars.entrySeparator);
                                    }
                                    catch { }
                                }
                                if (item["q" + i].ToString().Contains(ConstantVars.entrySeparator))
                                {
                                    try
                                    {
                                        existing = item["q" + i].ToString().Split(ConstantVars.entrySeparator);
                                    }
                                    catch { }
                                }
                                foreach (String input in import)
                                {
                                    if (!vals.Contains(input)) {
                                        vals.Add(input);
                                        Console.WriteLine("input exists");
                                    }
                                    Console.WriteLine(input);
                                }
                                foreach (String exist in existing)
                                {
                                    if (!vals.Contains(exist))
                                    {
                                        vals.Add(exist);
                                        Console.WriteLine("exist exists");
                                    }
                                    Console.WriteLine(exist);
                                }
                                String total = vals[0];
                                for(int j = 1; j < vals.Count; j++) {
                                    total += ConstantVars.entrySeparator + vals[j];
                                }
                                Console.WriteLine(total);
                                item["q" + i] = total;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("oof");
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