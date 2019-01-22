using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NRGScoutingApp
{
    public partial class Rankings : ContentPage
    {
        public Rankings()
        {
            InitializeComponent();
        }

        Ranker mainRank = new Ranker(App.Current.Properties["matchEventsString"].ToString());

        protected override void OnAppearing()
        {
            listView.ItemsSource = mainRank.getPickAvgData((int)MatchFormat.ACTION.pick1);
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
        
}

        // FOLLOWING BUTTON TO BE REMOVED IN PRODUCTION (DEBUG PURPOSES ONLY)
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var del = await DisplayAlert("notice", "clear ALL Entires??", "yes", "no");
            if (del)
            {
                App.Current.Properties.Clear();
                await App.Current.SavePropertiesAsync();
            }
        }
    }
}
