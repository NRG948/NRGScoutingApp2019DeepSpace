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
            if (!String.IsNullOrWhiteSpace(e.NewTextValue))
            {
                importButton.IsEnabled = true;
            }
            else
            {
                importButton.IsEnabled = false;
            }
        }

        void importClicked(object sender, System.EventArgs e)
        {
            JObject data = MatchParameters.initializeEventsObject();
            try
            {
                JObject importJSON = (JObject) JsonConvert.DeserializeObject(importData.Text);
                if (importJSON.ContainsKey("Matches")) {
                    int numMatches;
                    if (data.Count <= 0)
                    {
                        data.Add(importJSON);
                        JArray matchesArray = (JArray)data["Matches"];
                        numMatches = matchesArray.Count;

                    }
                    else {
                        JArray matchesArray = (JArray)data["Matches"];
                        numMatches = matchesArray.Count;
                        addItemsChecker(data, importJSON);
                        numMatches = matchesArray.Count - numMatches;
                    }
                    DisplayAlert("Success", "Added " + numMatches + " entries.", "OK");
                    App.Current.Properties["matchEventsString"] = JsonConvert.SerializeObject(data);
                    App.Current.SavePropertiesAsync();
                    PopupNavigation.Instance.PopAsync(true);
                }
                else {
                    DisplayAlert("Error", "Error in Data", "OK");
                }
            }
            catch (JsonReaderException) {
                DisplayAlert("Alert", "Error in Data", "OK");
            }
        }

        async void addItemsChecker(JObject data, JObject importJSON) {
            JArray temp = (JArray)data["Matches"];
            JArray importData = (JArray)importJSON["Matches"];
            foreach (var match in temp.ToList())
            {
                foreach (var import in importData.ToList()) { 
                if (Convert.ToInt32(match["matchNum"]) == Convert.ToInt32(data["matchNum"]) && Convert.ToInt32(match["side"]) == Convert.ToInt32(data["side"]))
                {
                    if (!(match["team"].ToString().Equals(import["team"])))
                    {
                        var remove = await DisplayAlert("Error", "Overwrite Old Match with New Data?\nTeam Name: " + data["team"] + "\nMatch Number: " + data["matchNum"], "No", "Yes");
                        if (!remove)
                        {
                            temp.Remove(match);
                            temp.Add(import);

                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        temp.Remove(match);
                        temp.Add(import);
                    }
                }
                else
                {
                    temp.Add(import);
                }
            }
            }
        }

    }
}


