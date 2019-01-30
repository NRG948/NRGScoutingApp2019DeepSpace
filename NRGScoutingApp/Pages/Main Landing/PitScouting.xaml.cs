using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NRGScoutingApp
{
    public partial class PitScouting : ContentPage
    {
        public PitScouting()
        {
            InitializeComponent();
        }

        void newPit(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new MatchEntryStart(false));
        }

        void SearchBar_OnTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {

        }

        void teamClicked(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {

        }

        /*
         * Sets the visibility of the list based on boolean and the sad error opposite
         * So if list.IsVisible = true, then sadNoMatch.IsVisible = false
         */
        private void setListVisibility(int setList)
        {
            listView.IsVisible = setList > 0;
            sadNoPit.IsVisible = !listView.IsVisible;
        }
    }
}
