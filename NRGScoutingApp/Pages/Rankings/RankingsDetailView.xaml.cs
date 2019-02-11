using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Forms.Xaml;

namespace NRGScoutingApp
{
    public partial class RankingsDetailView : ContentPage
    {
        /*
         * This is the order in which the array is ordered       
         * overall, cargoTime, hatchTime, climb, lvl1, lvl2, lvl3
         */
        public RankingsDetailView(String[] times)
        {
            InitializeComponent();
            setScoreValues(times);
            Rankings.teamSend = Rankings.teamSend.Split('-')[MatchFormat.teamNameOrNum].Trim();
            listView.ItemsSource = Matches.matchesList.Where(matchesList => matchesList.teamNameAndSide.ToLower().Contains(Rankings.teamSend.ToLower()));
        }

        void setScoreValues(String[] times)
        {
            score0.Text = ConstantVars.scoreBaseVals[0] + times[0];
            score1.Text = ConstantVars.scoreBaseVals[1] + times[1];
            score2.Text = ConstantVars.scoreBaseVals[2] + times[2];
            score3.Text = ConstantVars.scoreBaseVals[3] + times[3];
            score4.Text = ConstantVars.scoreBaseVals[4] + times[4];
            score5.Text = ConstantVars.scoreBaseVals[5] + times[5];
            score6.Text = ConstantVars.scoreBaseVals[6] + times[6];
            score7.Text = ConstantVars.scoreBaseVals[7] + times[7];
        }

        void matchTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {

        }
    }
}
