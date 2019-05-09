using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.CloudFirestore;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using System.Net;
using System.Collections.Generic;

namespace NRGScoutingApp
{
    public partial class ImportDialog
    {
        public ImportDialog()
        {
            InitializeComponent();
        }

        void cancelClicked(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            importButton.IsEnabled = !String.IsNullOrWhiteSpace(e.NewTextValue);
        }

        async void importClicked(object sender, System.EventArgs e)
        {
            JObject data = MatchParameters.initializeEventsObject();
            if (importData.Text.ToLower().Contains("https://pastebin.com/raw/"))
            {
                try
                {
                    String response = new WebClient().DownloadString(importData.Text);
                    importData.Text = response;
                }
                catch
                {
                    await DisplayAlert("Error", "No Internet!", "OK");
                    return;
                }
            }
            try
            {
                JObject importJSON = (JObject)JsonConvert.DeserializeObject(importData.Text);
                await getCombinedJSON(importJSON, data);
            }
            catch (Exception s)
            {
                Console.WriteLine(s.StackTrace);
                await DisplayAlert("Alert", "Error in Data", "OK");
            }
            Preferences.Set("matchEventsString", JsonConvert.SerializeObject(data));
        }

        async public Task getCombinedJSON(JObject importJSON, JObject data)
        {
            try
            {
                if (importJSON.ContainsKey("Matches") || importJSON.ContainsKey("PitNotes"))
                {
                    if (importJSON.ContainsKey("Matches"))
                    {
                        if (!data.ContainsKey("Matches"))
                        {
                            data.Add(new JProperty("Matches", new JArray()));
                        }
                        await addMatchItemsChecker(data, importJSON);
                    }
                    if (importJSON.ContainsKey("PitNotes"))
                    {
                        if (!data.ContainsKey("PitNotes"))
                        {
                            data.Add(new JProperty("PitNotes", new JArray()));
                        }
                        await addPitItemsChecker(data, importJSON);
                    }
                    await PopupNavigation.Instance.PopAsync(true);
                }
                else
                {
                    await DisplayAlert("Alert", "Error in Data", "OK");
                }
            }
            catch (JsonReaderException)
            {
                await DisplayAlert("Alert", "Error in Data", "OK");
            }
        }

        private async Task addPitItemsChecker(JObject data, JObject importJSON)
        {
            JArray temp = (JArray)data["PitNotes"];
            JArray importTemp = (JArray)importJSON["PitNotes"];
            var tempList = temp.ToList();
            foreach (var match in importTemp.ToList())
            {
                if (tempList.Exists(x => x["team"].Equals(match["team"])))
                {
                    var item = tempList.Find(x => x["team"].Equals(match["team"]));
                    temp.Remove(item);
                    for (int i = 0; i < ConstantVars.QUESTIONS.Length; i++)
                    {
                        try
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
                                String replaced = input.Replace("&", "and");
                                if (!vals.Any(s => s.ToLower().Contains(replaced.ToLower())))
                                {
                                    vals.Add(replaced);
                                }
                                if (vals.Any(s => replaced.ToLower().Contains(s.ToLower())))
                                {
                                    var test = vals.Find(s => replaced.ToLower().Contains(s.ToLower()));
                                    vals.Remove(test);
                                    vals.Add(replaced);
                                }
                            }
                            foreach (String input in existing)
                            {
                                String replaced = input.Replace("&", "and");
                                if (!vals.Any(s => s.ToLower().Contains(replaced.ToLower())))
                                {
                                    vals.Add(replaced);
                                }
                                if (vals.Any(s => replaced.ToLower().Contains(s.ToLower())))
                                {
                                    var test = vals.Find(s => replaced.ToLower().Contains(s.ToLower()));
                                    vals.Remove(test);
                                    vals.Add(replaced);
                                }
                            }
                            String total = vals[0];
                            for (int j = 1; j < vals.Count; j++)
                            {
                                total += ConstantVars.entrySeparator + vals[j];
                            }
                            Console.WriteLine(total);
                            item["q" + i] = total;
                        }
                        catch
                        {
                            Console.WriteLine("oof");
                        }
                    }
                    temp.Add(item);
                }
                else
                {
                    temp.Add(match);
                }
            }
        }

        async void fireBaseClicked(object sender, System.EventArgs e)
        {
            try
            {
                importButton.IsEnabled = false;
                fireBase.Text = "Getting Data...";
                IDocumentSnapshot document = await CrossCloudFirestore.Current
                                            .Instance
                                            .GetCollection("2019")
                                            .GetDocument("948")
                                            .GetDocumentAsync();
                String s = document.Data["AllData"].ToString();
                importData.Text = s;
                fireBase.IsEnabled = false;
                importClicked(sender, e);
            }
            catch (Exception s)
            {
                fireBase.Text = "FireBase Test";
                importButton.IsEnabled = true;
                Console.WriteLine(s.StackTrace);
            }
        }


        private async Task addMatchItemsChecker(JObject data, JObject importJSON)
        {
            int tooMuch = 0;
            int mode = 0; // 1 for overwite all, 2 for ignore all
            JArray temp = (JArray)data["Matches"];
            JArray importTemp = (JArray)importJSON["Matches"];
            var tempList = temp.ToList();
            foreach (var match in importTemp.ToList())
            {
                if (tempList.Exists(x => x["matchNum"].Equals(match["matchNum"]) && x["side"].Equals(match["side"])))
                {
                    var item = tempList.Find(x => x["matchNum"].Equals(match["matchNum"]) && x["side"].Equals(match["side"]));
                    if (!item["team"].Equals(match["team"]))
                    {
                        if (mode == 1)
                        {
                            temp.Remove(item);
                            temp.Add(match);
                        }
                        else if (mode == 0)
                        {
                            tooMuch++;
                            if (tooMuch <= 1)
                            {
                                var add = await DisplayAlert("Warning!", "Match: " + item["matchNum"] +
                                    "\nTeam: " + item["team"] +
                                    "\nSide: " + MatchFormat.matchSideFromEnum(Convert.ToInt32(item["side"])) +
                                    "\nConflicts with Existing Match", "Overwite", "Ignore");
                                if (add)
                                {
                                    temp.Remove(item);
                                    temp.Add(match);
                                }
                            }
                            else
                            {
                                var add = await DisplayActionSheet("Warning!" + "\nMatch: " + item["matchNum"] +
                                    "\nTeam: " + item["team"] +
                                    "\nSide: " + MatchFormat.matchSideFromEnum(Convert.ToInt32(item["side"])) +
                                    "\nConflicts with Existing Match", null, null, "Overwite", "Ignore", "Overwite All", "Ignore All");
                                if (add.Equals("Overwite"))
                                {
                                    temp.Remove(item);
                                    temp.Add(match);
                                }
                                else if (add.Equals("Overwite All"))
                                {
                                    temp.Remove(item);
                                    temp.Add(match);
                                    mode = 1;
                                }
                                else if (add.Equals("Ignore All"))
                                {
                                    mode = 2;
                                }
                            }
                        }
                    }
                }
                else
                {
                    temp.Add(match);
                }
            }
        }
    }
}