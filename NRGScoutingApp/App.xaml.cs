using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.ComponentModel;
using System.Security.Principal;


namespace NRGScoutingApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Application.Current.MainPage = new NavigationPage(new NavTab());
            MainPage = new NavigationPage(new NavTab());

        }
    }
}
