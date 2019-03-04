using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NRGScoutingApp {
    public partial class MatchEntryStart : ContentPage {
        private bool goToMatch;

        public MatchEntryStart (bool ismatch) {
            goToMatch = ismatch;
            InitializeComponent ();
            MatchesList.ItemsSource = TeamsNames.teams;
        }
        public Boolean goBack = false;
        public string teamName;

        protected override void OnAppearing () {
            if (goBack == true) {
                goBack = false;
                Navigation.PopAsync ();
            }
            MatchesList.ItemsSource = TeamsNames.teams;
        }

        async void Handle_ItemTapped (object sender, Xamarin.Forms.ItemTappedEventArgs e) {
            teamName = e.Item.ToString ();
            Preferences.Set ("teamStart", teamName);
            await App.Current.SavePropertiesAsync ();
            if (goToMatch) {
                await Navigation.PushAsync(new MatchEntryEditTab() { Title = teamName });
                Navigation.RemovePage(this);
            } else {
                await Navigation.PushAsync (new PitEntry (true) { Title = teamName });
                Navigation.RemovePage(this);
            }
        }

        private void SearchBar_OnTextChanged (object sender, TextChangedEventArgs e) {
            // MatchesList.BeginRefresh();
            if (!String.IsNullOrWhiteSpace (e.NewTextValue)) {
                MatchesList.ItemsSource = teams.Where (teams => teams.ToLower ().Contains (e.NewTextValue.ToLower ()));
            }

            //MatchesList.EndRefresh();
        }
        List<string> teams = TeamsNames.teams;
    }

}