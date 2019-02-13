using System;
using System.Collections.Generic;
using NRGScoutingApp;
using Xamarin.Forms;
//using Android.Provider;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms.Xaml;

namespace NRGScoutingApp {
    public partial class BlueAllianceMatches : ContentPage {

        public BlueAllianceMatches () {
            InitializeComponent ();

            var url;
            Browser.Source = url;
            url = "https://www.thebluealliance.com";
            webView.Navigating += (object sender, WebNavigatingEventArgs e) => {
                url = e.Url;
            };
        }
        void Home_Clicked (object sender, System.EventArgs e) {
            Browser.Source = "https://www.thebluealliance.com";
        }
        private void backClicked (object sender, EventArgs e) {
            if (Browser.CanGoBack) {
                Browser.GoBack ();
            } else {
                Navigation.PopAsync ();

            }
        }

        private void forwardClicked (object sender, EventArgs e) {
            if (Browser.CanGoForward) {
                Browser.GoForward ();
            }
        }
    }
}