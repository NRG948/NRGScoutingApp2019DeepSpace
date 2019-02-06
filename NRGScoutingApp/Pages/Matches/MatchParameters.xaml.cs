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
using System.Threading.Tasks;

namespace NRGScoutingApp
{
    public partial class MatchParameters : ContentPage
    {
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

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
            onParamUpdate();
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
            Entry.team = teamName;
            onParamUpdate();
            if (popErrorsToScreen()) { }
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
                if (data.Count <= 0 || !data.ContainsKey("Matches"))
                {
                    data.Add(new JProperty("Matches", new JArray()));
                    pushBackToHome(data, new JArray(), parameters);
                }
                else
                {
                    JArray temp = (JArray)data["Matches"];
                    if (temp.ToList().Exists(x => x["matchNum"].Equals(parameters["matchNum"]) && x["side"].Equals(parameters["side"])))
                    {
                        var item = temp.ToList().Find(x => x["matchNum"].Equals(parameters["matchNum"]) && x["side"].Equals(parameters["side"]));
                        if (item["team"] != parameters["team"])
                        {
                            var remove = await DisplayAlert("Error", "Overwrite Old Match with New Data?", "No", "Yes");
                            if (!remove)
                            {
                                temp.Remove(item);
                                pushBackToHome(data, temp, parameters);
                            }
                            else
                            {
                                saveButton.IsEnabled = true;
                                return;
                            }
                        }
                        else
                        {
                            temp.Remove(item);
                            pushBackToHome(data, temp, parameters);
                        }
                    }
                    else
                    {
                        pushBackToHome(data, temp, parameters);
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

        void deathSelector(object sender, System.EventArgs e)
        {
            Entry.deathType = death.SelectedIndex;
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
                onParamUpdate();
            }
            catch (FormatException)
            {
                if (!String.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    DisplayAlert("Warning", "Match Number Contains Letters. Did Not Update Value", "OK");
                    matchnum.Text = "1";
                }
            }
        }

        void Fouls_Updated(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                Entry.fouls = Convert.ToInt32(e.NewTextValue);
                onParamUpdate();
            }
            catch (FormatException)
            {
                if (!String.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    DisplayAlert("Warning", "Match Number Contains Letters. Did Not Update Value", "OK");
                    fouls.Text = "0";
                }
            }
        }

        //Returns Jobject based on wheter match events string is empty or not
        public static JObject initializeEventsObject()
        {
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

                death.SelectedIndex = entries.deathType;
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
            else
            {
                death.SelectedIndex = (int)MatchFormat.DEATH_TYPE.noDeath;
                fouls.Text = "0";
            }
            setAutoButtons();
            setEndGameSelfButtons();
            setEndGameOtherButtons();
        }

        //Clears all properties for use in next match
        public static async void clearMatchItems()
        {
            App.Current.Properties["teamStart"] = "";
            App.Current.Properties["appState"] = 0;
            App.Current.Properties["timerValue"] = (int)0;
            App.Current.Properties["lastItemPicked"] = 0;
            App.Current.Properties["lastItemDropped"] = 0;
            App.Current.Properties["tempParams"] = "";
            App.Current.Properties["tempMatchEvents"] = "";
            await App.Current.SavePropertiesAsync();
            NewMatchStart.events.Clear();
        }

        //Takes all objects and adds items while returning the main page
        async void pushBackToHome(JObject data, JArray temp, JObject parameters)
        {
            temp.Add(new JObject(parameters));
            data["Matches"] = temp;
            App.Current.Properties["matchEventsString"] = JsonConvert.SerializeObject(data);
            await App.Current.SavePropertiesAsync();
            try
            {
                if (Matches.appRestore == false)
                {
                    Navigation.PopToRootAsync(true);
                }
                else if (Matches.appRestore == true)
                {
                    Matches.appRestore = false;
                    Navigation.PopAsync(true);
                }
            }
            catch (System.InvalidOperationException)
            {

            }
            clearMatchItems();
        }

        //Disables Auto Buttons if certain button is not toggled
        void setAutoButtons()
        {
            autoLvl.IsEnabled = crossbase.IsToggled;
            autoOTele.IsEnabled = crossbase.IsToggled;
            if (!crossbase.IsToggled)
            {
                autoLvl.SelectedIndex = -1;
                autoOTele.IsToggled = false;
            }
        }

        //Disables Self Climb EndGame Buttons if certain button is not toggled
        void setEndGameSelfButtons()
        {
            climbLvl.IsEnabled = climbSwitch.IsToggled;
            needed.IsEnabled = climbSwitch.IsToggled;
            if (!climbSwitch.IsToggled)
            {
                climbLvl.SelectedIndex = -1;
                needed.IsToggled = false;
            }
        }

        //Disables Help Climb EndGame Buttons if certain button is not toggled
        void setEndGameOtherButtons()
        {
            giveAssistClimbLvl.IsEnabled = assisted.IsToggled;
            if (!assisted.IsToggled)
            {
                giveAssistClimbLvl.SelectedIndex = -1;
            }
        }

        //Pops errors if fields are checked but their counterparts are not
        private bool popErrorsToScreen()
        {
            String errors = "";
            bool toPrint = false;
            if (string.IsNullOrWhiteSpace(matchnum.Text) || matchnum.Text.Substring(0,1).Equals("0"))
            {
                errors += "\n- Match Number";
                toPrint = true;
            }
            if (PositionPicker.SelectedIndex < 0)
            {
                errors += "\n- Position";
                toPrint = true;
            }
            if(crossbase.IsToggled && autoLvl.SelectedIndex < 0) {
                errors += "\n- Auto Level";
                toPrint = true;
            }
            if(climbSwitch.IsToggled && climbLvl.SelectedIndex < 0) {
                errors += "\n- Climb Options";
                toPrint = true;
            }
            if(assisted.IsToggled && giveAssistClimbLvl.SelectedIndex < 0) {
                errors += "\n- Give Climb Options";
                toPrint = true;
            }
            if (toPrint)
            {
                DisplayAlert("The Following Errors Occured", errors, "OK");
            }
            return toPrint;
        }
    }
}



