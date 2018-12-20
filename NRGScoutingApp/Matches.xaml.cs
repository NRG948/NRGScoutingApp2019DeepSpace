using System;
using System.Collections.Generic;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Forms.Platform;
using NRGScoutingApp;
using Newtonsoft.Json;
using Xamarin.Forms.Xaml;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Rg.Plugins.Popup.Services;
using Data = System.Collections.Generic.KeyValuePair<string, string>;



namespace NRGScoutingApp
{
    public partial class Matches : ContentPage
    {
        /* For Blue Alliance Matches 
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new BlueAllianceMatches());
        } */

        public static Boolean appRestore;
        public Boolean popNav;

    public Matches()
        {
            InitializeComponent();
            matchConfirm();
            App.Current.Properties["newAppear"] = 0;
            App.Current.SavePropertiesAsync();
           // MatchesList.ItemsSource = teams;
         }

        void matchStart(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new MatchEntryStart());
        }

        List<Data> matches; 

        protected override void OnAppearing()
        {
            popNav = false;
            if (!App.Current.Properties.ContainsKey("matchEventsString"))
            {
                App.Current.Properties["matchEventsString"] = "";
                App.Current.Properties["tempEventString"] = "(";
                App.Current.SavePropertiesAsync();
            }
            if (!App.Current.Properties.ContainsKey("newAppear")){}  //DEBUG PURPOSES
            else if (App.Current.Properties["newAppear"].ToString() == "1")
            {
                App.Current.Properties["appState"] = 0;
                App.Current.Properties["timerValue"] = (int)0;
                App.Current.Properties["teamStart"] = "";
                App.Current.Properties["newAppear"] = 0;
                App.Current.Properties["tempEventString"] = "(";
                listView.ItemsSource = MatchFormat.matchesToSimpleData(MatchFormat.mainStringToSplit(App.Current.Properties["matchEventsString"].ToString()));
                matches = MatchFormat.matchesToSimpleData(MatchFormat.mainStringToSplit(App.Current.Properties["matchEventsString"].ToString()));
                if (!App.Current.Properties["matchEventsString"].ToString().Contains("|"))
                {
                    matches = null;
                    listView.ItemsSource = null;
                }
                App.Current.SavePropertiesAsync();
            }
            else if(App.Current.Properties["newAppear"].ToString() == "0"){
                listView.ItemsSource = MatchFormat.matchesToSimpleData(MatchFormat.mainStringToSplit(App.Current.Properties["matchEventsString"].ToString()));
                matches = MatchFormat.matchesToSimpleData(MatchFormat.mainStringToSplit(App.Current.Properties["matchEventsString"].ToString()));
            }
        }

       void importClicked(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new ImportDialog());
        }

        void exportClicked(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new ExportDialog());
        }

        void newClicked(object sender, System.EventArgs e)
        {
            popNav = false;
            appRestore = false;
            Navigation.PushAsync(new MatchEntryStart());
             
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                listView.ItemsSource = matches;
            else
                listView.ItemsSource = matches.Where( matches => matches.Key.Contains(e.NewTextValue) || matches.Value.Contains(e.NewTextValue));
        }

        void matchConfirm(){
            if(!App.Current.Properties.ContainsKey("appState")){
                appRestore = false;
                App.Current.Properties["appState"] = 0;
                App.Current.Properties["teamStart"] = "";
                App.Current.Properties["timerValue"] = (int)0;
                App.Current.Properties["matchEventsString"] = "";
                App.Current.SavePropertiesAsync();
            }

            else if (App.Current.Properties["appState"].ToString() == "1")
            {
                appRestore = true;
                Navigation.PushAsync(new MatchEntryEditTab());
            }
            else if (App.Current.Properties["appState"].ToString() == "0")
            {
                appRestore = false;
                App.Current.Properties["appState"] = 0;
                App.Current.Properties["timerValue"] = (int) 0;
                App.Current.Properties["teamStart"] = "";
                App.Current.Properties["tempEventString"] = "";
                App.Current.SavePropertiesAsync();
            }
            if (!App.Current.Properties.ContainsKey("matchEventsString"))
            {
                App.Current.Properties["matchEventsString"] = "";
                App.Current.Properties["tempEventString"] = "(";
                App.Current.SavePropertiesAsync();
            }
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {

        }

    }


}


