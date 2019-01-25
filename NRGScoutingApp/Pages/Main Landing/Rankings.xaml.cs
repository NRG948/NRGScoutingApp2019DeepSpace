using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Linq;

namespace NRGScoutingApp
{
    public partial class Rankings : ContentPage
    {
        public Rankings()
        {
            InitializeComponent();
        }

        //Initializes the ranking object
        Ranker mainRank = new Ranker(App.Current.Properties["matchEventsString"].ToString());

        protected override void OnAppearing()
        {
            //Updates string data from matches
            mainRank.setData(App.Current.Properties["matchEventsString"].ToString());
                //Gets all data and sets it into ascending order based on each team's average time
                Dictionary<string, double> x = mainRank.getPickAvgData((int)MatchFormat.ACTION.pick1);
                var y = from pair in x
                        orderby pair.Value ascending
                        select pair;
            //Checks if match string is empty
            listView.IsVisible = x.Count > 0;
            sadNoMatch.IsVisible = !listView.IsVisible;
            listView.ItemsSource = y;
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
        }
    }
}
