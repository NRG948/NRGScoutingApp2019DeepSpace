using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Newtonsoft.Json.Schema;
using System.Linq;
using System.Threading.Tasks;

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
            try
            {
                JObject importJSON = (JObject) JsonConvert.DeserializeObject(importData.Text);
                if (importJSON.ContainsKey("Matches")) {
                    int numMatches;
                    if (data.Count <= 0)
                    {
                        JArray matchesArray = (JArray)importJSON["Matches"];
                        data = new JObject(importJSON);
                        numMatches = matchesArray.Count;
                    }
                    else {
                        JArray matchesArray = (JArray)data["Matches"];
                        numMatches = matchesArray.Count;
                        await addItemsChecker(data, importJSON);
                        numMatches = matchesArray.Count - numMatches;
                    }
                    await DisplayAlert("Success", "Added " + numMatches + " entries.", "OK");
                    App.Current.Properties["matchEventsString"] = JsonConvert.SerializeObject(data);
                    await App.Current.SavePropertiesAsync();
                    await PopupNavigation.Instance.PopAsync(true);
                }
                else {
                   await DisplayAlert("Error", "Error in Data", "OK");
                }
            }
            catch (JsonReaderException) {
                await DisplayAlert("Error", "Error in Data", "OK");
            }
        }

        async Task addItemsChecker(JObject data, JObject importJSON) {
            JArray temp = (JArray)data["Matches"];
            JArray importTemp = (JArray)importJSON["Matches"];
            var tempList = temp.ToList();
            foreach (var match in importTemp.ToList())
            {
                Console.WriteLine("in foreach");
                Console.WriteLine(match);
                if (tempList.Exists(x => x["matchNum"].Equals(match["matchNum"]) && x["side"].Equals(match["side"])))
                {
                    Console.WriteLine("inexist if");
                    var item = tempList.Find(x => x["matchNum"].Equals(match["matchNum"]) && x["side"].Equals(match["side"]));
                    if(item["team"] == match["team"])
                    {
                        temp.Add(match);
                    }
                    else
                    {
                        var add = await DisplayAlert("Warning!", "Match: " + item["matchNum"] +
                                                    "\nTeam: " + item["team"] +
                                                    "\nSide: " + MatchFormat.matchSideFromEnum(Convert.ToInt32(item["side"])) +
                                                    "\nConflicts with Existing Match", "Overwite", "Ignore");
                        if (!add)
                        {
                            temp.Remove(item);
                            temp.Add(match);
                        }
                    }
                    Console.WriteLine(item);
                }
                else
                {
                    Console.WriteLine("bypassed if");
                    temp.Add(match);
                }
            }

            //foreach (var match in temp.ToList())
            //{
            //    foreach (var import in importData.ToList()) { 
            //    if (Convert.ToInt32(match["matchNum"]) == Convert.ToInt32(data["matchNum"]) && Convert.ToInt32(match["side"]) == Convert.ToInt32(data["side"]))
            //    {
            //        if (!(match["team"].ToString().Equals(import["team"])))
            //        {
            //            var remove = await DisplayAlert("Error", "Overwrite Old Match with New Data?\nTeam Name: " + data["team"] + "\nMatch Number: " + data["matchNum"], "No", "Yes");
            //            if (!remove)
            //            {
            //                temp.Remove(match);
            //                temp.Add(import);

            //            }
            //            else
            //            {
            //                break;
            //            }
            //        }
            //        else
            //        {
            //            temp.Remove(match);
            //            temp.Add(import);
            //        }
            //    }
            //    else
            //    {
            //        temp.Add(import);
            //    }
            //}
            //}
        }

    }
}


