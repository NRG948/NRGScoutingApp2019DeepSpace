using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NRGScoutingApp {
    public partial class MatchEntryEditTab : Xamarin.Forms.TabbedPage {
        public MatchEntryEditTab () {
            Children.Add (new NewMatchStart ());
            Children.Add (new MatchEvents ());
            Children.Add (new MatchParameters ());
            BindingContext = this;
            Preferences.Set ("newAppear", 1);
            NavigationPage.SetHasNavigationBar (this, false);
            NavigationPage.SetHasNavigationBar (this, true);
            NavigationPage.SetHasBackButton (this, true);
            NavigationPage.SetHasBackButton (this, false);
            InitializeComponent ();
        }

        protected override bool OnBackButtonPressed () {
            return true;
        }

        private String teamName = Preferences.Get ("teamStart", "");

        protected override void OnAppearing () {
            if (!teamName.Equals (Preferences.Get ("teamStart", ""))) {
                teamName = Preferences.Get ("teamStart", "hello");
                this.Title = teamName;
            }
        }
        async void backClicked (object sender, System.EventArgs e) {
            var text = await DisplayAlert ("Alert", "Do you want to discard progress?", "Yes", "No");
            if (text) {
                MatchParameters.clearMatchItems ();
                Navigation.PopAsync ();
            }
        }
    }
}