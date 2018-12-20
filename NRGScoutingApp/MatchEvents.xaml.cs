using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.ComponentModel;
using System.Linq;
using Data = System.Collections.Generic.KeyValuePair<string, string>;

namespace NRGScoutingApp
{
    public partial class MatchEvents : ContentPage
    {
                //StringFormat paramFormat = new StringFormat();
        public MatchEvents()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }


        protected override void OnAppearing()
        {

        }

        void Handle_Clicked(object sender, System.EventArgs e, List<Data> data)
        {
            listView.ItemsSource = data;
           
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {

        }
    }
}
