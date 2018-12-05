using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PracticeNRGScouting2018
{
	public partial class TeamSelection : ContentPage
	{
        public TeamSelection()
        {
            InitializeComponent();

            List<String> teams = TeamsNames.teams;
            TeamsList.ItemsSource = teams;

        }

        private void TeamsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            App.Current.Properties["teamName"] = e.Item.ToString();
            App.Current.SavePropertiesAsync();
            Navigation.PushAsync(new MatchTimer());
        }
    }
}