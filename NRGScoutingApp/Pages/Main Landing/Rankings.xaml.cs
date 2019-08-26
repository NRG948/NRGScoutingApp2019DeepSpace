using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NRGScoutingApp {
    /*ADD Ranking Chooser Replacement for iOS 
     *like a picker acts as the distribution center to choose the type
     */
    public partial class Rankings : INotifyPropertyChanged {
        public Rankings () {
            InitializeComponent ();
            rankPicker.SelectedIndex = 0;
        }

        public static string teamSend;
        private List<RankStruct> rankList;
        public static List<string> pitTeams;

        MatchFormat.CHOOSE_RANK_TYPE rankChoice;

        //Initializes the ranking object
        Ranker mainRank = new Ranker (Preferences.Get ("matchEventsString", ""));

        //void settingsClicked (object sender, System.EventArgs e) {
        //    Navigation.PushAsync (new Settings ());
        //}

        void rankTypeDelta (object sender, System.EventArgs e) {
            rankChoice = (MatchFormat.CHOOSE_RANK_TYPE) rankPicker.SelectedIndex;
            updateEvents ();
        }

        protected override void OnAppearing () {
            updateEvents ();
            setPitTeams ();

        }

        //Updates events with given enum
        private void updateEvents () {
            //Updates string data from matches
            mainRank.setDat (Preferences.Get ("matchEventsString", ""));
            //Gets all data and sets it into ascending order based on each team's average time
            Dictionary<string, double> x = mainRank.getRank (rankChoice);
            Dictionary<string, double> y = new Dictionary<string, double> ();
            List<RankStruct> ranks = new List<RankStruct> ();
            if (!(rankPicker.SelectedIndex == 7)) {
                y = (from pair in x orderby pair.Value descending select pair).ToDictionary (pair => pair.Key, pair => pair.Value);
            } else {
                try {
                    y = (from pair in x orderby Convert.ToInt32 (pair.Key.Split (" - ", 2) [0]) ascending select pair).ToDictionary (pair => pair.Key, pair => pair.Value);
                } catch {
                    y = new Dictionary<string, double> ();
                }
            }

            foreach (var s in y) {
                ranks.Add (new RankStruct { Key = s.Key, Value = s.Value, color = getTeamColor (s.Key) });
            }
            listView.ItemsSource = ranks;
            rankList = ranks;
            setListVisibility (y.Count ());
        }

        public class RankStruct {
            public string Key { get; set; }
            public double Value { get; set; }
            public Color color { get; set; }
        }

        private Color getTeamColor (String team) {
            return mainRank.getColors () [team];
        }

        /*
         * Sets the visibility of the list based on boolean and the sad error opposite
         * So if list.IsVisible = true, then sadNoMatch.IsVisible = false
         */
        private void setListVisibility (int setList) {
            rankingsView.IsVisible = setList > 0;
            sadNoMatch.IsVisible = !rankingsView.IsVisible;
        }

        void SearchBar_TextChanged (object sender, Xamarin.Forms.TextChangedEventArgs e) {
            if (string.IsNullOrWhiteSpace (e.NewTextValue)) {
                listView.ItemsSource = rankList;
            } else {
                listView.ItemsSource = rankList.Where (rankList => rankList.Key.ToLower ().Contains (e.NewTextValue.ToLower ()) ||
                    rankList.Key.ToLower ().Contains (e.NewTextValue.ToLower ()) ||
                    getColorString (rankList.color).ToLower ().Contains (e.NewTextValue.ToLower ()));
            }
        }

        private String getColorString (Color input) {
            if (input.Equals (Color.Red)) {
                return "Red";
            } else if (input.Equals (Color.Yellow)) {
                return "Yellow";
            } else {
                return "None";
            }
        }

        async void teamClicked (object sender, Xamarin.Forms.ItemTappedEventArgs e) {
            var x = (listView.ItemsSource as IEnumerable<RankStruct>).ToList ();
            String item = x.Find (y => y.Equals (e.Item)).Key;
            teamSend = item;
            await Navigation.PushAsync (new RankingsDetailView (mainRank.returnTeamTimes (item)) { Title = item });
        }

        private void setPitTeams () {
            JObject input;
            try {
                input = JObject.Parse (Preferences.Get ("matchEventsString", ""));
            } catch (Newtonsoft.Json.JsonException) {
                input = new JObject ();
            }
            pitTeams = PitScouting.getListVals (input);
        }

        void allianceClicked (object sender, System.EventArgs e) {

        }
    }
}