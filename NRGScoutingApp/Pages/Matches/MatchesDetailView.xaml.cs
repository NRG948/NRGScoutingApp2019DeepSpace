using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NRGScoutingApp {
    public partial class MatchesDetailView : ContentPage {
        private int jsonIndex;
        public MatchesDetailView (int index) {
            InitializeComponent ();
            matchDetailJSON.Text = returnMatchJSONText (index);
            jsonIndex = index;
        }

        async void cancelClicked (object sender, System.EventArgs e) {
            cancelAction.IsEnabled = false;
            await Navigation.PopAsync ();
        }
        async void openClicked (object sender, System.EventArgs e) {
            await Task.Run (async () => {

                JObject val = JObject.Parse (returnMatchJSONText (jsonIndex));
                JObject parameters = new JObject ();
                foreach (var x in val) {
                    if (!x.Key.Equals ("numEvents")) {
                        parameters.Add (x.Key, x.Value);
                    } else {
                        break;
                    }
                }
                Console.WriteLine (parameters);
                Preferences.Set ("tempParams", JsonConvert.SerializeObject (parameters.ToObject<MatchFormat.EntryParams> ()));
                NewMatchStart.events = MatchFormat.JSONEventsToObject (val);
                CubeDroppedDialog.saveEvents ();
                Preferences.Set ("timerValue", Convert.ToInt32 (val.Property ("timerValue").Value));
                Preferences.Set ("teamStart", val.Property ("team").Value.ToString ());
                Device.BeginInvokeOnMainThread (() => {
                    Navigation.PushAsync (new MatchEntryEditTab () { Title = val.Property ("team").Value.ToString () });
                });
            });
        }

        async void deleteClicked (object sender, System.EventArgs e) {
            var delete = await DisplayAlert ("Alert", "Are you sure you want to delete this entry?", "No", "Yes");
            if (!delete) {
                deleteMatchAtIndex (jsonIndex);
                await Navigation.PopAsync ();
            }
        }

        //Returns the Json String based on the index of the given match selected in the Matches page
        String returnMatchJSONText (int index) {
            JObject matchesJSON = JObject.Parse (Preferences.Get ("matchEventsString", ""));
            JArray temp = (JArray) matchesJSON["Matches"];
            return temp[index].ToString ();
        }

        async void deleteMatchAtIndex (int index) {
            JObject matchesJSON = JObject.Parse (Preferences.Get ("matchEventsString", ""));
            JArray temp = (JArray) matchesJSON["Matches"];
            if (temp.Count == 1) {
                matchesJSON.Remove ("Matches");
                Preferences.Set ("matchEventsString", JsonConvert.SerializeObject (matchesJSON));
            } else {
                temp.RemoveAt (index);
                matchesJSON["Matches"] = temp;
                Preferences.Set ("matchEventsString", JsonConvert.SerializeObject (matchesJSON, Formatting.None));
            }
            if (matchesJSON.Count <= 0) {
                Preferences.Set ("matchEventsString", "");
            }
        }
    }
}