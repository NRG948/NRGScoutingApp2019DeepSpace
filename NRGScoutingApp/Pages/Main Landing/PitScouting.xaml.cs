using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NRGScoutingApp {
    public partial class PitScouting : ContentPage {
        public PitScouting () {
            InitializeComponent ();
            setListView (Preferences.Get ("matchEventsString", ""));
        }

        public static List<string> pitItems = new List<string> ();

        protected override void OnAppearing () {
            setListView (Preferences.Get ("matchEventsString", ""));
        }

        void newPit (object sender, System.EventArgs e) {
            Navigation.PushAsync (new MatchEntryStart (ConstantVars.TEAM_SELECTION_TYPES.pit));
        }

        void SearchBar_OnTextChanged (object sender, Xamarin.Forms.TextChangedEventArgs e) {
            if (string.IsNullOrWhiteSpace (e.NewTextValue)) {
                listView.ItemsSource = pitItems;
            } else {
                listView.ItemsSource = pitItems.Where (pitItems => pitItems.ToLower ().Contains (e.NewTextValue.ToLower ()));
            }
        }

        async void teamClicked (object sender, Xamarin.Forms.ItemTappedEventArgs e) {
            String teamName = e.Item.ToString ();
            JArray pitValues = (JArray) JObject.Parse (Preferences.Get ("matchEventsString", "")) ["PitNotes"];
            Preferences.Set ("teamStart", teamName);
            await Navigation.PushAsync (new PitEntry (false, teamName, true) { Title = teamName });
        }

        /*
         * Sets the visibility of the list based on boolean and the sad error opposite
         * So if list.IsVisible = true, then sadNoMatch.IsVisible = false
         */
        private void setListVisibility (int setList) {
            listView.IsVisible = setList > 0;
            sadNoPit.IsVisible = !listView.IsVisible;
        }

        public static List<string> getListVals (JObject input) {
            List<string> teamsInclude = new List<string> ();
            if (input.ContainsKey ("PitNotes")) {
                JArray pits = (JArray) input["PitNotes"];
                foreach (var x in pits) {
                    teamsInclude.Add (x["team"].ToString ());
                }
            }
            return teamsInclude;
        }
        void setListView (String json) {
            JObject input;
            if (!String.IsNullOrWhiteSpace (json)) {
                try {
                    input = JObject.Parse (json);
                } catch (JsonException) {
                    input = new JObject ();
                }
                pitItems = getListVals (input);
                scoutView.IsVisible = true;
                sadNoPit.IsVisible = !scoutView.IsVisible;
            } else {
                pitItems = new List<string> ();
            }
            listView.ItemsSource = pitItems;
            scoutView.IsVisible = pitItems.Count > 0;
            sadNoPit.IsVisible = !scoutView.IsVisible;
        }
    }
}