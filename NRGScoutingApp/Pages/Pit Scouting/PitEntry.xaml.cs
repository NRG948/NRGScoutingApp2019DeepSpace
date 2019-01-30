using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NRGScoutingApp
{
    public partial class PitEntry : ContentPage
    {
        string titleName = (App.Current.Properties["teamStart"].ToString());
        public string teamName { get { return titleName; } }

        public PitEntry()
        {
            InitializeComponent();
        }

        void backClicked(object sender, System.EventArgs e)
        {
            
        }

        void saveClicked(object sender, System.EventArgs e)
        {
            
        }
    }
}
