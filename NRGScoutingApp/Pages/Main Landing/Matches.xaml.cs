using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NRGScoutingApp {
    public partial class Matches : ContentPage {
        /* For Blue Alliance Matches
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new BlueAllianceMatches());
        } */
            
        public static List<MatchesListFormat> matchesList;

        public Matches () {
            InitializeComponent ();
            matchConfirm ();
        }

        protected override void OnAppearing () {
            populateMatchesList();
        }

        void importClicked (object sender, System.EventArgs e) {
            popupInit ();
        }

        private void popupInit () {
            var popup = new ImportDialog ();
            popup.Disappearing += (sender, e) => { this.OnAppearing (); };
            PopupNavigation.Instance.PushAsync (popup);
        }

        void exportClicked (object sender, System.EventArgs e) {
            PopupNavigation.Instance.PushAsync (new ExportDialog ());
        }

        async void newClicked (object sender, System.EventArgs e) {
            await Navigation.PushAsync (new MatchEntryStart (ConstantVars.TEAM_SELECTION_TYPES.match));
        }

        private void SearchBar_OnTextChanged (object sender, TextChangedEventArgs e) {
            if (string.IsNullOrWhiteSpace (e.NewTextValue)) {
                listView.ItemsSource = matchesList;
            } else {
                listView.ItemsSource = matchesList.Where (matchesList => matchesList.teamNameAndSide.ToLower ().Contains (e.NewTextValue.ToLower ()) || matchesList.matchNum.ToLower ().Contains (e.NewTextValue.ToLower ()));
            }
        }

        private void matchConfirm () {
            if (!String.IsNullOrWhiteSpace (Preferences.Get ("tempMatchEvents", "")) || !String.IsNullOrWhiteSpace (Preferences.Get ("tempParams", "")))
            {
                Navigation.PushAsync (new MatchEntryEditTab () { Title = Preferences.Get ("teamStart", "") });
            } else if (!String.IsNullOrWhiteSpace (Preferences.Get ("tempPitNotes", ""))) {
                Navigation.PushAsync (new PitEntry (true, Preferences.Get ("teamStart", ""), true) { Title = Preferences.Get ("teamStart", "") });
            }
            populateMatchesList();
        }

        void Handle_ItemTapped (object sender, Xamarin.Forms.ItemTappedEventArgs e) {
            int index;
            var x = listView.ItemsSource as List<MatchesListFormat>;
            if (!String.IsNullOrWhiteSpace (searchBar.Text)) {
                index = matchesList.IndexOf (e.Item as MatchesListFormat);
            } else {
                index = (listView.ItemsSource as List<MatchesListFormat>).IndexOf (e.Item as MatchesListFormat);
            }
            Navigation.PushAsync (new MatchesDetailView (index));
        }

        public class MatchesListFormat {
            public String matchNum { get; set; }
            public String teamNameAndSide { get; set; }
        }

        async void deleteClicked (object sender, System.EventArgs e) {
            await DisplayAlert ("Hold it", "Make sure export to data first", "OK");
            var del = await DisplayAlert ("Notice", "Do you want to delete all matches? Data CANNOT be recovered.", "Yes", "No");
            if (del) {
                JObject s = JObject.Parse(Preferences.Get("matchEventsString", ""));
                if (s.ContainsKey("Matches"))
                {
                    s.Remove("Matches");
                }
                Preferences.Set("matchEventsString", JsonConvert.SerializeObject(s));
                populateMatchesList ();
            }
        }

        async void refreshList()
        {
            listView.IsRefreshing = true;
            await Task.Run(async () => {
                populateMatchesList();
            });
            listView.IsRefreshing = false;
        }

        void populateMatchesList () {
            JObject x;
            if (!String.IsNullOrWhiteSpace(Preferences.Get("matchEventsString", "")))
            {
                try
                {
                    x = JObject.Parse(Preferences.Get("matchEventsString", ""));
                }
                catch
                {
                    Console.WriteLine("Caught NullRepEx for populateMatchesList");
                    x = new JObject();
                }
            }
            else
            {
                x = new JObject();
            }
            if (!x.HasValues)
            {
                matchesList = null;
            }
            else
            {
                JObject matchesJSON = JObject.Parse(Preferences.Get("matchEventsString", ""));
                JArray temp = (JArray)matchesJSON["Matches"];
                //Will Contain all items for matches list
                matchesList = new List<MatchesListFormat>();
                int count;
                try
                {
                    count = temp.Count;
                }
                catch
                {
                    count = -1;
                }

                for (int i = 0; i < count; i++)
                {
                    JObject match = (JObject)temp[i];
                    string teamTemp = "";
                    try
                    {
                        teamTemp = match["team"].ToString();
                    }
                    catch { }
                    String teamIdentifier = "";
                    try
                    {
                        teamIdentifier = teamTemp.Split("-", 2)[MatchFormat.teamNameOrNum].Trim();
                    }
                    catch
                    {
                        teamIdentifier = teamTemp;
                    }

                    matchesList.Add(new MatchesListFormat
                    {
                        matchNum = "Match " + match["matchNum"],
                        teamNameAndSide = teamIdentifier + " - " + MatchFormat.matchSideFromEnum((int)match["side"])
                    });
                }
            }
            try
            {
                matchesView.IsVisible = matchesList.Count > 0;
                sadNoMatch.IsVisible = !matchesView.IsVisible;
            }
            catch
            {
                matchesView.IsVisible = false;
                sadNoMatch.IsVisible = true;
            }
            listView.ItemsSource = matchesList;
        }
    }
}