using System;
using System.Collections.Generic;
using NRGScoutingApp;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.PlatformConfiguration;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Forms.Platform;
using Newtonsoft.Json;
using Xamarin.Forms.Xaml;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


namespace NRGScoutingApp
{
    public partial class MatchEntryStart : ContentPage
    {
        private bool goToMatch;

        public MatchEntryStart(bool ismatch)
        {
            goToMatch = ismatch;
            InitializeComponent();
            MatchesList.ItemsSource = TeamsNames.teams;
        }
        public Boolean goBack = false;
        public string teamName;

        protected override void OnAppearing()
        {
            if (goBack == true)
            {
                goBack = false;
                Navigation.PopAsync();
            }
            MatchesList.ItemsSource = TeamsNames.teams;
        }

        async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            teamName = e.Item.ToString();
            App.Current.Properties["teamStart"] = teamName;
            Console.WriteLine (teamName);
            await App.Current.SavePropertiesAsync();
            Console.WriteLine(App.Current.Properties["teamStart"].ToString());
            if (goToMatch) {
                await Navigation.PushAsync(new MatchEntryEditTab() { Title = teamName });
            }
            else {
                await Navigation.PushAsync(new PitEntry() { Title = teamName });
            }
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // MatchesList.BeginRefresh();
            if (!String.IsNullOrWhiteSpace(e.NewTextValue))
            {
                MatchesList.ItemsSource = teams.Where(teams => teams.ToLower().Contains(e.NewTextValue.ToLower()));
            }

            //MatchesList.EndRefresh();
        }
        List<string> teams = TeamsNames.teams;
    }




}
