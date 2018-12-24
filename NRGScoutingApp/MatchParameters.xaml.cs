using System;
using System.Collections.Generic;
using System.Collections;
using Xamarin.Forms;
using System.ComponentModel;
using NRGScoutingApp;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Xml;

namespace NRGScoutingApp
{
    public partial class MatchParameters : ContentPage
    {
        public String teamName = App.Current.Properties["teamStart"].ToString();
        public ParametersFormat paramFormat = new ParametersFormat();
        public MatchEventsFormat eventsFormat = new MatchEventsFormat();
        MatchFormat.EntryParams Entry = new MatchFormat.EntryParams {
        team = App.Current.Properties["teamStart"].ToString(),
        matchNum = 0,
        side = 0,

        crossedB = false,
        allyItem1 = false,
        allyItem2 = false,
        oppItem1 = false,
        oppItem2 = false,

        death = false,
        noClimb = false,
        soloClimb = false,
        giveAssistClimb = false,
        needAssistClimb = false,
        onClimbArea = false,

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

        public MatchParameters(ArrayList list)
        {
            InitializeComponent();
            matchnum.Text = list[0].ToString();
            PositionPicker.Items[PositionPicker.SelectedIndex] = list[1].ToString();
            crossbase.IsToggled = Convert.ToBoolean(list[2].ToString());
            switchP.IsToggled = Convert.ToBoolean(list[3].ToString());
            scale.IsToggled = Convert.ToBoolean(list[4].ToString());
            farswitch.IsToggled = Convert.ToBoolean(list[5].ToString());
            farscale.IsToggled = Convert.ToBoolean(list[6].ToString());
            death.IsToggled = Convert.ToBoolean(list[7].ToString());
            solo.IsToggled = Convert.ToBoolean(list[8].ToString());
            assisted.IsToggled = Convert.ToBoolean(list[9].ToString());
            needed.IsToggled = Convert.ToBoolean(list[10].ToString());
            platform.IsToggled = Convert.ToBoolean(list[11].ToString());
            noclimb.IsToggled = Convert.ToBoolean(list[12].ToString());
            fouls.Text = list[13].ToString();
            yellow.IsToggled = Convert.ToBoolean(list[14].ToString());
            red.IsToggled = Convert.ToBoolean(list[15].ToString());
            comments.Text = list[16].ToString();
            list.Clear();
        }

        async void backClicked(object sender, System.EventArgs e)
        {
           var text = await DisplayAlert("Alert", "Do you want to discard progress?", "Yes", "No");
            if (text)
            {
                App.Current.Properties["teamStart"] = "";
                App.Current.Properties["appState"] = 0;
                App.Current.Properties["timerValue"] = (int)0;
                App.Current.Properties["lastCubePicked"] = 0;
                App.Current.Properties["lastCubeDropped"] = 0;
                App.Current.Properties["tempEventString"] = "(";
                await App.Current.SavePropertiesAsync();
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

        async void saveClicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(matchnum.Text)|| PositionPicker.SelectedIndex < 0) //Checks if Match Number or Picker is Present
            {
                if (string.IsNullOrWhiteSpace(matchnum.Text) && PositionPicker.SelectedIndex < 0)
                {
                    await DisplayAlert("Alert!", "Please Enter Match Number and Position", "OK");
                }
                else if (string.IsNullOrWhiteSpace(matchnum.Text))
                {
                    await DisplayAlert("Alert!", "Please Enter Match Number", "OK");
                }
                else if (PositionPicker.SelectedIndex < 0)
                {
                    await DisplayAlert("Alert!", "Please Enter Position", "OK");
                }
            }
            else{
                String parameters = JsonConvert.SerializeObject(Entry, Newtonsoft.Json.Formatting.Indented);
                Console.WriteLine(parameters);
                string events = MatchFormat.eventsListToJSONEvents(NewMatchStart.events);
                Console.WriteLine(events);
                App.Current.Properties["tempParams"] = "";
                App.Current.Properties["tempMatchEvents"] = "";
                await App.Current.SavePropertiesAsync();
                if (Matches.appRestore == false)
                {
                 await  Navigation.PopToRootAsync(true);
                }
                else if (Matches.appRestore == true)
                {
                    Matches.appRestore = false;
                    await Navigation.PopAsync(true);
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
            Entry.crossedB = e.Value;
            onParamUpdate();
        }

        void Handle_Toggled_1(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.allyItem2 = e.Value;
            onParamUpdate();
        }

        void Handle_Toggled_2(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.allyItem1 = e.Value;
            onParamUpdate();
        }

        void Handle_Toggled_3(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.oppItem1 = e.Value;
            onParamUpdate();
        }

        void Handle_Toggled_4(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.oppItem2 = e.Value;
            onParamUpdate();
        }

        void Handle_Toggled_5(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
           Entry.death = e.Value;
            onParamUpdate();
        }

        void Handle_Toggled_6(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.soloClimb = e.Value;
            onParamUpdate();
        }

        void Handle_Toggled_7(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.giveAssistClimb = e.Value; 
            onParamUpdate();
        }

        void Handle_Toggled_8(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.needAssistClimb = e.Value;
            onParamUpdate();
        }

        void Handle_Toggled_9(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.onClimbArea = e.Value;
            onParamUpdate();
        }

        void Handle_Toggled_10(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Entry.noClimb = e.Value;
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

        void onParamUpdate() {
            App.Current.Properties["tempParams"] = Entry;
            App.Current.SavePropertiesAsync();
            Console.WriteLine(JsonConvert.SerializeObject((MatchFormat.EntryParams)App.Current.Properties["tempParams"], Newtonsoft.Json.Formatting.Indented));
        }

        void cacheCheck() {
            if (!String.IsNullOrWhiteSpace(App.Current.Properties["tempParams"].ToString())) {
                MatchFormat.EntryParams entries = (MatchFormat.EntryParams)App.Current.Properties["tempParams"];
                matchnum.Text = entries.matchNum.ToString();
                PositionPicker.SelectedIndex = entries.side;
                crossbase.IsToggled = entries.crossedB;
                switchP.IsToggled = entries.allyItem2;
                scale.IsToggled = entries.allyItem1;
                farswitch.IsToggled = entries.oppItem1;
                farscale.IsToggled = entries.oppItem2;
                death.IsToggled = entries.death;

                solo.IsToggled = entries.soloClimb;
                assisted.IsToggled = entries.giveAssistClimb;
                needed.IsToggled = entries.needAssistClimb;
                platform.IsToggled = entries.onClimbArea;
                noclimb.IsToggled = entries.noClimb;

                fouls.Text = entries.fouls.ToString();
                yellow.IsToggled = entries.yellowCard;
                red.IsToggled = entries.redCard;
                comments.Text = entries.comments;
                Entry = entries;
            }
        }
    }
}

                                  
