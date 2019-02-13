using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
namespace NRGScoutingApp {
    public partial class NavTab : Xamarin.Forms.TabbedPage {
        public NavTab () {
            Children.Add (new Matches ());
            Children.Add (new Rankings ());
            Children.Add (new PitScouting ());
            InitializeComponent ();
        }
    }
}