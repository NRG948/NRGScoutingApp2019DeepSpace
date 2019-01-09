using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms.Xaml;

namespace NRGScoutingApp
{
    public partial class MatchesDetailView : ContentPage
    {
        private int jsonIndex;
        public MatchesDetailView(int index)
        {
            InitializeComponent();
            matchDetailJSON.Text = returnMatchJSONText(index);
            jsonIndex = index;
        }

        async void cancelClicked(object sender, System.EventArgs e)
        {
            cancelAction.IsEnabled = false;
           await Navigation.PopAsync();
        }
        async void openClicked(object sender, System.EventArgs e)
        {
            JObject val = JObject.Parse(returnMatchJSONText(jsonIndex));
            JObject parameters = new JObject();
            foreach (var x in val)
            {
                if (!x.Key.Equals("numEvents"))
                {
                    parameters.Add(x.Key, x.Value);
                }
            }
            Console.WriteLine(parameters);
            App.Current.Properties["tempParams"] = parameters.ToObject<MatchFormat.EntryParams>();
            Console.WriteLine("before for ");
            for(int i = 0; i < Convert.ToInt32( val.Property("numEvents").Value); i++)
            {
                Console.WriteLine("after for");
                NewMatchStart.events.Add(new MatchFormat.Data { 
                time = Convert.ToInt32(val.Property("TE" + i + "_0").Value), 
                type = Convert.ToInt32(val.Property("TE" + i + "_1").Value) 
                });
                CubeDroppedDialog.saveEvents();
            }
            App.Current.Properties["timerValue"] = Convert.ToInt32(val.Property("timerValue").Value);
            App.Current.Properties["teamStart"] = val.Property("team").Value;
            await App.Current.SavePropertiesAsync();
            await Navigation.PushAsync(new MatchEntryEditTab());
        }

        async void deleteClicked(object sender, System.EventArgs e)
        {
            var delete = await DisplayAlert("Alert", "Are you sure you want to delete this entry?", "No", "Yes");
            if (!delete) {
                deleteMatchAtIndex(jsonIndex);
                await Navigation.PopAsync();
            }
        }

        //Returns the Json String based on the index of the given match selected in the Matches page
        String returnMatchJSONText(int index)
        {
            JObject matchesJSON = JObject.Parse(App.Current.Properties["matchEventsString"].ToString());
            JArray temp = (JArray)matchesJSON["Matches"];
            return temp[index].ToString();
        }

        void deleteMatchAtIndex(int index) {
            JObject matchesJSON = JObject.Parse(App.Current.Properties["matchEventsString"].ToString());
            JArray temp = (JArray)matchesJSON["Matches"];
            if(temp.Count-1 == index)
            {
                App.Current.Properties["matchEventsString"] = "";
                App.Current.SavePropertiesAsync();
            }
            else
            {
                temp.RemoveAt(index);
                matchesJSON["Matches"] = temp;
                App.Current.Properties["matchEventsString"] = JsonConvert.SerializeObject(matchesJSON, Formatting.None);
                App.Current.SavePropertiesAsync();
            }
        }
    }
}
