using System;
using System.Collections.Generic;
using System.Collections;
using Xamarin.Forms;
using System.ComponentModel;
using NRGScoutingApp;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Xml;
using Newtonsoft.Json;
using System.Threading;
using System.Linq;

namespace NRGScoutingApp
{
    public partial class MatchParameters : ContentPage
    {
        public String teamName = App.Current.Properties["teamStart"].ToString();
        public ParametersFormat paramFormat = new ParametersFormat();
        public static MatchFormat.EntryParams Entry = new MatchFormat.EntryParams
        {
            team = App.Current.Properties["teamStart"].ToString(),
            matchNum = 0,
            side = 0,

            crossBaseline = false,
            autoLvl = 0,
            autoOTele = false,

            deathType = 0,
            climb = false,
            climbLvl = 0,
            giveAstClimb = false,
            giveAstClimbLvl = 0,
            needAstClimb = false,

            fouls = 0,
            yellowCard = false,
            redCard = false,
            comments = ""
        };

        public MatchParameters()
        {
            InitializeComponent();
            cacheCheck();
        }

        //Confirms user action to go back and clears all data for next match
        async void backClicked(object sender, System.EventArgs e)
        {
            var text = await DisplayAlert("Alert", "Do you want to discard progress?", "Yes", "No");
            if (text)
            {
                clearMatchItems();
                if (Matches.appRestore == false)
                {
                    Matches.appRestore = false;
                    await Navigation.PopToRootAsync(true);
                }
                else if (Matches.appRestore == true)
                {
                    Matches.appRestore = false;
                    await Navigation.PopAsync(true);
                }

            }
        }

        //Checks if all neccesary Items exist, clears match data, and goes to Matches Page
        async void saveClicked(object sender, System.EventArgs e)
        {
            if (isSaveConditionNotMet()) {
                popErrorsToScreen();
            }
            else
            {
                //Disables save button so app doesn't crash when user taps many times
                saveButton.IsEnabled = false;
                //Gets and combines all of the match's events to a JObject
                JObject events = MatchFormat.eventsListToJSONEvents(NewMatchStart.events);
                events.Add("timerValue", NewMatchStart.timerValue);
                JObject parameters = JObject.FromObject(Entry);
                parameters.Merge(events);

                //Adds or creates new JObject to start all data in app cache
                JObject data = initializeEventsObject();
                if (data.Count <= 0)
                {
                    data.Add(new JProperty("Matches", new JArray(new JObject(parameters))));
                    pushBackToHome(data, new JArray(), parameters);
                }
                else
                {
                    JArray temp = (JArray)data["Matches"];
                    foreach (var match in temp.ToList()) {
                        if (Convert.ToInt32(match["matchNum"]) == Entry.matchNum && Convert.ToInt32(match["side"]) == Entry.side) {
                            if (!(match["team"].ToString().Equals(Entry.team))) {
                                var remove = await DisplayAlert("Error", "Overwrite Old Match with New Data?", "No", "Yes");
                                if (!remove) {
                                    temp.Remove(match);
                                    pushBackToHome(data, temp, parameters);
                                }
                                else {
                                    break;
                                }
                            }
                            else {
                                temp.Remove(match);
                                pushBackToHome(data, temp, parameters);
                            }
                        }
                        else {
                            pushBackToHome(data, temp, parameters);
                        }
                    }
                }
            }
        }

        void Handle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Entry.side = PositionPicker.SelectedIndex;
            
            onParamUpdate();
        }

        void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.crossBaseline = e.Value;
            setAutoButtons();
            onParamUpdate();
        }

        void Auto_Level_Changed(object sender, System.EventArgs e)
        {
            Entry.autoLvl = autoLvl.SelectedIndex;
            onParamUpdate();
        }

        void autoOrTeleSandstorm(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.autoOTele = e.Value; //0 is auto 1 is tele
            onParamUpdate();
        }

        void Handle_Toggled_5(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.deathType = e.Value;
            onParamUpdate();
        }

        void climb(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.climb = e.Value;
            setEndGameSelfButtons();
            onParamUpdate();
        }

        void climbLvlSelector(object sender, System.EventArgs e)
        {
            Entry.climbLvl = climbLvl.SelectedIndex;
        }

        void needAssistToggle(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.needAstClimb = e.Value;
            onParamUpdate();
        }

        void helpedClimb(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.giveAstClimb = e.Value;
            setEndGameOtherButtons();
            onParamUpdate();
        }

        void giveAssistClimbLvlSelector(object sender, System.EventArgs e)
        {
            Entry.giveAstClimbLvl = giveAssistClimbLvl.SelectedIndex;
            onParamUpdate();
        }

        void Handle_Toggled_11(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.yellowCard = e.Value;
            onParamUpdate();
        }

        void Handle_Toggled_12(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.redCard = e.Value;
            onParamUpdate();
        }

        void Comment_Box_Updated(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            Entry.comments = e.NewTextValue;
            onParamUpdate();
        }

        void Match_Num_Updated(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                Entry.matchNum = Convert.ToInt32(e.NewTextValue);
            }
            catch (FormatException)
            {
                if (!String.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    DisplayAlert("Warning", "Match Number Contains Letters. Did Not Update Value", "OK");
                }
            }

            onParamUpdate();
        }

        void Fouls_Updated(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                Entry.fouls = Convert.ToInt32(e.NewTextValue);
            }
            catch (FormatException)
            {
                if (!String.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    DisplayAlert("Warning", "Match Number Contains Letters. Did Not Update Value", "OK");
                }
            }
            onParamUpdate();
        }

        //Returns Jobject based on wheter match events string is empty or not
        public static JObject initializeEventsObject() {
            JObject data;
            if (!String.IsNullOrWhiteSpace(App.Current.Properties["matchEventsString"].ToString()))
            {
                data = JObject.Parse(App.Current.Properties["matchEventsString"].ToString());
            }
            else
            {
                data = new JObject();
            }
            return data;
        }

        //Updates tempParam Data Cache every time Parameters are updated
        void onParamUpdate()
        {
            App.Current.Properties["tempParams"] = Entry;
            App.Current.SavePropertiesAsync();
        }

        //Checks if old data exists in app and sets all toggles to reflect the options
        void cacheCheck()
        {
            if (!String.IsNullOrWhiteSpace(App.Current.Properties["tempParams"].ToString()))
            {
                MatchFormat.EntryParams entries = (MatchFormat.EntryParams)App.Current.Properties["tempParams"];
                matchnum.Text = entries.matchNum.ToString();
                PositionPicker.SelectedIndex = entries.side;

                crossbase.IsToggled = entries.crossBaseline;
                autoLvl.SelectedIndex = entries.autoLvl;
                autoOTele.IsToggled = entries.autoOTele;

                death.IsToggled = entries.death;
                climbSwitch.IsToggled = entries.climb;
                climbLvl.SelectedIndex = entries.climbLvl;
                assisted.IsToggled = entries.needAstClimb;
                needed.IsToggled = entries.giveAstClimb;
                giveAssistClimbLvl.SelectedIndex = entries.giveAstClimbLvl;

                fouls.Text = entries.fouls.ToString();
                yellow.IsToggled = entries.yellowCard;
                red.IsToggled = entries.redCard;
                comments.Text = entries.comments;
                Entry = entries;
               }
            setAutoButtons();
            setEndGameSelfButtons();
            setEndGameOtherButtons();
        }

        //Clears all properties for use in next match
         void clearMatchItems()
        {
            App.Current.Properties["teamStart"] = "";
            App.Current.Properties["appState"] = 0;
            App.Current.Properties["timerValue"] = (int)0;
            App.Current.Properties["lastItemPicked"] = 0;
            App.Current.Properties["lastItemDropped"] = 0;
            App.Current.Properties["tempParams"] = "";
            App.Current.Properties["tempMatchEvents"] = "";
             App.Current.SavePropertiesAsync();
            NewMatchStart.events.Clear();
        }

        //Takes all objects and adds items while returning the main page
        async void pushBackToHome(JObject data, JArray temp, JObject parameters) {
            temp.Add(new JObject(parameters));
            data["Matches"] = temp;
            if (Matches.appRestore == false)
            {
                Navigation.PopToRootAsync(true);
            }
            else if (Matches.appRestore == true)
            {
                Matches.appRestore = false;
                Navigation.PopAsync(true);
            }
            App.Current.Properties["matchEventsString"] = JsonConvert.SerializeObject(data);
            await App.Current.SavePropertiesAsync();
            clearMatchItems();
        }

        //Disables Auto Buttons if certain button is not toggled
        void setAutoButtons() {
            autoLvl.IsEnabled = crossbase.IsToggled;
            autoOTele.IsEnabled = crossbase.IsToggled;
            if (!crossbase.IsToggled)
            {
                autoLvl.SelectedIndex = -1;
                autoOTele.IsToggled = false;
            }
       }

        //Disables Self Climb EndGame Buttons if certain button is not toggled
        void setEndGameSelfButtons() {
            climbLvl.IsEnabled = climbSwitch.IsToggled;
            assisted.IsEnabled = climbSwitch.IsToggled;
            needed.IsEnabled = climbSwitch.IsToggled;
            if (!climbSwitch.IsToggled)
            {
                climbLvl.SelectedIndex = -1;
                assisted.IsToggled = false;
                needed.IsToggled = false;
            }
        }

        //Disables Help Climb EndGame Buttons if certain button is not toggled
        void setEndGameOtherButtons() {
            giveAssistClimbLvl.IsEnabled = assisted.IsToggled;
            if (!assisted.IsToggled)
            {
                giveAssistClimbLvl.SelectedIndex = -1;
            }
        }
    
        //returns True if required fields are empty
         bool isSaveConditionNotMet() {
            return string.IsNullOrWhiteSpace(matchnum.Text) || PositionPicker.SelectedIndex < 0; //Checks if Match Number or Picker is Present
        }

        void popErrorsToScreen() {
            if (string.IsNullOrWhiteSpace(matchnum.Text) && PositionPicker.SelectedIndex < 0)
            {
                 DisplayAlert("Alert!", "Please Enter Match Number and Position", "OK");
            }
            else if (string.IsNullOrWhiteSpace(matchnum.Text))
            {   
                DisplayAlert("Alert!", "Please Enter Match Number", "OK");
            }
            else if (PositionPicker.SelectedIndex < 0)
            {
                 DisplayAlert("Alert!", "Please Enter Position", "OK");
            }
        }
    }
}



