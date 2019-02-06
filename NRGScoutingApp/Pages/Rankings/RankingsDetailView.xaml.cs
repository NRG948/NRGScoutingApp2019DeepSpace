using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;

namespace NRGScoutingApp
{
    public partial class RankingsDetailView : ContentPage
    {
        public RankingsDetailView()
        {
            InitializeComponent();
            Rankings.teamSend = Rankings.teamSend.Split('-')[MatchFormat.teamNameOrNum].Trim();
            Console.WriteLine(Rankings.teamSend);
            listView.ItemsSource = Matches.matchesList.Where(matchesList => matchesList.teamNameAndSide.ToLower().Contains(Rankings.teamSend.ToLower()));
        }


        void matchTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {

        }
    }
}
