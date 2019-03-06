using Xamarin.Essentials;
using Xamarin.Forms;
using System;

namespace NRGScoutingApp {
    public partial class MatchEntryEditTab : Xamarin.Forms.TabbedPage {
        public MatchEntryEditTab () {
            Children.Add (new NewMatchStart ());
            Children.Add (new MatchEvents ());
            Children.Add (new MatchParameters ());
            BindingContext = this;
            Preferences.Set ("newAppear", 1);
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
            NavigationPage.SetHasBackButton (this, false);
            InitializeComponent ();
        }

        protected override bool OnBackButtonPressed () {
            return true;
        }

        async void backClicked (object sender, System.EventArgs e) {
            var text = await DisplayAlert ("Alert", "Do you want to discard progress?", "Yes", "No");
            if (text) {
                MatchParameters.clearMatchItems ();
                Navigation.PopAsync();
            }
        }
    }
}