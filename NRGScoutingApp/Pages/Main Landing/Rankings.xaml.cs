using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;

namespace NRGScoutingApp
{
    /*ADD Ranking Chooser Replacement for iOS 
     *like a picker acts as the distribution center to choose the type
     */    
    public partial class Rankings : ContentPage
    {
        public Rankings()
        {
            InitializeComponent();
        }

        MatchFormat.CHOOSE_RANK_TYPE rankChoice = MatchFormat.CHOOSE_RANK_TYPE.overallRank;

        //Initializes the ranking object
        Ranker mainRank = new Ranker(App.Current.Properties["matchEventsString"].ToString());

        void overallRank(object sender, System.EventArgs e)
        {
            rankChoice = MatchFormat.CHOOSE_RANK_TYPE.overallRank;
            updateEvents();
        }

        void pick1Rank(object sender, System.EventArgs e)
        {
            rankChoice = MatchFormat.CHOOSE_RANK_TYPE.drop1;
            updateEvents();
        }

        void pick2Rank(object sender, System.EventArgs e)
        {
            rankChoice = MatchFormat.CHOOSE_RANK_TYPE.drop2;
            updateEvents();
        }

        void climbRank(object sender, System.EventArgs e)
        {
            rankChoice = MatchFormat.CHOOSE_RANK_TYPE.climb;
            updateEvents();
        }

        void drop1Rank(object sender, System.EventArgs e)
        {
            rankChoice = MatchFormat.CHOOSE_RANK_TYPE.drop1;
            updateEvents();
        }

        void drop2Rank(object sender, System.EventArgs e)
        {
            rankChoice = MatchFormat.CHOOSE_RANK_TYPE.drop2;
            updateEvents();
        }

        void drop3Rank(object sender, System.EventArgs e)
        {
            rankChoice = MatchFormat.CHOOSE_RANK_TYPE.drop3;
            updateEvents();
        }

        protected override void OnAppearing()
        {
            updateEvents();
        }

        //Updates events with given enum
        private void updateEvents()
        {
            //Updates string data from matches
            mainRank.setData(App.Current.Properties["matchEventsString"].ToString());
            //Gets all data and sets it into ascending order based on each team's average time
            Dictionary<string, double> x = mainRank.getRank(rankChoice);
            var y = from pair in x
                    orderby pair.Value ascending
                    select pair;
            setListVisibility(y.Count());
            listView.ItemsSource = y;
        }

        /*
         * Sets the visibility of the list based on boolean and the sad error opposite
         * So if list.IsVisible = true, then sadNoMatch.IsVisible = false
         */
    private void setListVisibility(int setList)
        {
            listView.IsVisible = setList > 0;
            sadNoMatch.IsVisible = !listView.IsVisible;
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
        }
    }
}
