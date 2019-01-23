using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using Rg.Plugins.Popup.Services;
using Newtonsoft.Json.Linq;

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
        List<MatchesListFormat> matchesList;

        public Matches()
        {
            InitializeComponent();
            matchConfirm();
            App.Current.Properties["newAppear"] = 0;
            App.Current.SavePropertiesAsync();
            populateMatchesList();
        }

        protected override void OnAppearing()
        {
            popNav = false;
            if (!App.Current.Properties.ContainsKey("matchEventsString"))
            {
                App.Current.Properties["matchEventsString"] = "";
                App.Current.Properties["tempMatchEvents"] = "";
                App.Current.SavePropertiesAsync();
            }
            if (!App.Current.Properties.ContainsKey("newAppear")) { }  //DEBUG PURPOSES
            else if (App.Current.Properties["newAppear"].ToString() == "1")
            {
                App.Current.Properties["appState"] = 0;
                App.Current.Properties["timerValue"] = (int)0;
                App.Current.Properties["teamStart"] = "";
                App.Current.Properties["newAppear"] = 0;
                App.Current.Properties["tempMatchEvents"] = "";
                populateMatchesList();
                App.Current.SavePropertiesAsync();
            }
            else if (App.Current.Properties["newAppear"].ToString() == "0")
            {
                App.Current.Properties["appState"] = 0;
                App.Current.Properties["timerValue"] = (int)0;
                App.Current.Properties["teamStart"] = "";
                App.Current.Properties["newAppear"] = 0;
                App.Current.Properties["tempMatchEvents"] = "";
            }
            populateMatchesList();
        }

        void importClicked(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new ImportDialog());
        }

        void exportClicked(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new ExportDialog());
        }

        async void newClicked(object sender, System.EventArgs e)
        {
            popNav = false;
            appRestore = false;
            await Navigation.PushAsync(new MatchEntryStart());
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                listView.ItemsSource = matchesList;
            }

            else
            {
                listView.ItemsSource = matchesList.Where(matchesList => matchesList.teamNameAndSide.Contains(e.NewTextValue) || matchesList.matchNum.Contains(e.NewTextValue));
            }
        }

        void matchConfirm()
        {
            if (!App.Current.Properties.ContainsKey("appState"))
            {
                appRestore = false;
                App.Current.Properties["appState"] = 0;
                App.Current.Properties["teamStart"] = "";
                App.Current.Properties["timerValue"] = (int)0;
                App.Current.Properties["tempParams"] = "";
                App.Current.Properties["tempMatchEvents"] = "";
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
                App.Current.Properties["timerValue"] = (int)0;
                App.Current.Properties["teamStart"] = "";
                App.Current.Properties["tempMatchEvents"] = "";
                App.Current.Properties["tempParams"] = "";
                App.Current.SavePropertiesAsync();
            }
            if (!App.Current.Properties.ContainsKey("matchEventsString"))
            {
                App.Current.Properties["matchEventsString"] = "";
                App.Current.Properties["tempMatchEvents"] = "";
                App.Current.SavePropertiesAsync();
            }
            populateMatchesList();
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            int index;
            var x = listView.ItemsSource as List<MatchesListFormat>;
            if (!String.IsNullOrWhiteSpace(searchBar.Text))
            {
                index = matchesList.IndexOf(e.Item as MatchesListFormat);
            }
            else
            {
                index = (listView.ItemsSource as List<MatchesListFormat>).IndexOf(e.Item as MatchesListFormat);
            }
            Navigation.PushAsync(new MatchesDetailView(index));
        }

        public class MatchesListFormat
        {
            public String matchNum { get; set; }
            public String teamNameAndSide { get; set; }
        }

        void populateMatchesList()
        {
            JObject x;
            if (!String.IsNullOrWhiteSpace(App.Current.Properties["matchEventsString"].ToString()))
            {
                try
                {
                    x = JObject.Parse(App.Current.Properties["matchEventsString"].ToString());
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("Caught NullRepEx for populateMatchesList");
                    x = new JObject();
                }
            }
            else
            {
                x = new JObject();
            }
            if (!x.HasValues)
            {
                matchesList = null;
                listView.ItemsSource = null;
            }
            else
            {

                JObject matchesJSON = JObject.Parse(App.Current.Properties["matchEventsString"].ToString());
                JArray temp = (JArray)matchesJSON["Matches"];
                //Will Contain all items for matches list
                matchesList = new List<MatchesListFormat>();
                for (int i = 0; i < temp.Count; i++)
                {
                    JObject match = (JObject)temp[i];                    
                    String teamIdentifier = match["team"].ToString().Split('-')[MatchFormat.teamNameOrNum];
                    if (MatchFormat.teamNameOrNum == 0)
                    {
                        teamIdentifier = teamIdentifier.Substring(0, teamIdentifier.Length - 1);
                    }
                    else
                    {
                        teamIdentifier = teamIdentifier.Substring(1);
                    }

                    matchesList.Add(new MatchesListFormat
                    {
                        matchNum = "Match " + match["matchNum"].ToString(),
                        teamNameAndSide = teamIdentifier + " - " + MatchFormat.matchSideFromEnum((int)match["side"]
                    )
                    });
                }
                listView.ItemsSource = matchesList;
            }
        }
    }
}