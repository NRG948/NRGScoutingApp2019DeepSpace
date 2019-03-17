using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Principal;
using Xamarin.Forms;

namespace NRGScoutingApp {
    public partial class App : Application {
        public static bool UseMockDataStore = true;
        public static string BackendUrl = "https://localhost:5000";

        public App () {
            InitializeComponent ();
            MainPage = new NavigationPage (new NavTab ());
            Application.Current.MainPage = new NavigationPage (new NavTab ());
            if (UseMockDataStore)
                DependencyService.Register<MockDataStore> ();
            else
                DependencyService.Register<CloudDataStore> ();
        }
    }
}