using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace NRGScoutingApp
{
    public partial class PitScouting : ContentPage
    {
        public PitScouting()
        {
            InitializeComponent();
            setListView(App.Current.Properties["matchEventsString"].ToString());
        }

        List<string> pitItems = new List<string>();

        protected override void OnAppearing()
        {
            setListView(App.Current.Properties["matchEventsString"].ToString());
        }

        void newPit(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new MatchEntryStart(false));
        }

        void SearchBar_OnTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                listView.ItemsSource = pitItems;
            }

            else
            {
                listView.ItemsSource = pitItems.Where(pitItems => pitItems.ToLower().Contains(e.NewTextValue.ToLower()));
            }
        }

        async void teamClicked(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            String teamName = e.Item.ToString();
            JArray pitValues = (JArray)JObject.Parse(App.Current.Properties["matchEventsString"].ToString())["PitNotes"];
            //var select = pitValues.ToList().Find(x => x["team"].Equals(teamName));
            App.Current.Properties["teamStart"] = teamName;
            await App.Current.SavePropertiesAsync();
            await Navigation.PushAsync(new PitEntry(false) { Title = teamName });
        }

        /*
         * Sets the visibility of the list based on boolean and the sad error opposite
         * So if list.IsVisible = true, then sadNoMatch.IsVisible = false
         */
        private void setListVisibility(int setList)
        {
            listView.IsVisible = setList > 0;
            sadNoPit.IsVisible = !listView.IsVisible;
        }

        private List<string> getListVals(JObject input){
            List<string> teamsInclude = new List<string>();
            if(input.ContainsKey("PitNotes")){
                JArray pits = (JArray)input["PitNotes"];
                foreach(var x in pits){
                    teamsInclude.Add(x["team"].ToString());
                }
            }
            return teamsInclude;
        }
        void setListView(String json){
            JObject input;
            if(!String.IsNullOrWhiteSpace(json)){
                try{
                    input = JObject.Parse(json);
                }
                catch(JsonException){
                    input = new JObject();
                }
                pitItems = getListVals(input);
                scoutView.IsVisible = true;
                sadNoPit.IsVisible = !scoutView.IsVisible;
            }
            else {
                pitItems = new List<string>();
            }
            listView.ItemsSource = pitItems;
            scoutView.IsVisible = pitItems.Count > 0;
            sadNoPit.IsVisible = !scoutView.IsVisible;
        }
    }
}
