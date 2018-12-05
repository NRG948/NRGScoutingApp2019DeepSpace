using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PracticeNRGScouting2018
{
    public partial class Matches : ContentPage
    {
        public Matches()
        {
            InitializeComponent();
        
        }

         void newMatch(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new TeamSelection());
        }
    }
}
