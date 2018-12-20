using System;
using System.Collections.Generic;
using System.Collections;
using Xamarin.Forms;
using System.ComponentModel;
using NRGScoutingApp;

namespace NRGScoutingApp
{
    public partial class MatchParameters : ContentPage
    {
        public static String pickerS;
        public static bool crossedB, switchB, scaleB, fswitchB, fscaleB, deathB, soloB, assistedB, neededB, platformB, 
        noclimbB, recyellowB, recredB;
        public ParametersFormat paramFormat = new ParametersFormat();
        public MatchEventsFormat eventsFormat = new MatchEventsFormat();

        public MatchParameters()
        {
            //NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }

        public MatchParameters(ArrayList list)
        {
            NavigationPage.SetHasBackButton(this, false);
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
            if (string.IsNullOrWhiteSpace(matchnum.Text)||string.IsNullOrWhiteSpace(pickerS)) //Checks if Match Number or Picker is Present
            {
                if (string.IsNullOrWhiteSpace(matchnum.Text) && string.IsNullOrWhiteSpace(pickerS)){
                    await DisplayAlert("Alert!", "Please Enter Match Number and Position", "OK");
                }
                else if (string.IsNullOrWhiteSpace(matchnum.Text))
                {
                    await DisplayAlert("Alert!", "Please Enter Match Number", "OK");
                }
                else if (string.IsNullOrWhiteSpace(pickerS))
                {
                    await DisplayAlert("Alert!", "Please Enter Position", "OK");
                }

            }
            else{
               /* if (String.IsNullOrWhiteSpace(App.matchEvents)){}
                else if (App.matchEvents[0] != '(' && App.matchEvents[0] != '*')
                {
                    App.matchEvents = "(" + App.matchEvents;
                }
                App.matchEvents += ")";
                App.Current.Properties["teamStart"] = "";
                App.Current.Properties["appState"] = 0;
                App.Current.Properties["timerValue"] = 0;
                App.Current.Properties["newAppear"] = 1;
                App.Current.Properties["lastCubePicked"] = 0;
                App.Current.Properties["lastCubeDropped"] = 0;
                if (App.matchEvents.Contains("*"))
                {

                }
                else if (!App.matchEvents.Contains("*"))
                {

                }
                */
                App.Current.Properties["tempEventString"] = "(";
                await App.Current.SavePropertiesAsync();
                if (Matches.appRestore == false)
                {
                    Matches.appRestore = false;
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
            var pos = PositionPicker.Items[PositionPicker.SelectedIndex];
            pickerS = pos;
            DisplayAlert(pickerS, "Position Selected", "OK");
        }

        void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var crossed = crossbase.IsToggled;
            crossedB = crossed;
        }

        void Handle_Toggled_1(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var y = switchP.IsToggled;
            switchB = y;
        }

        void Handle_Toggled_2(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var y = scale.IsToggled;
            scaleB = y;
        }

        void Handle_Toggled_3(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var y = farswitch.IsToggled;
            fswitchB = y;
        }

        void Handle_Toggled_4(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var y = farscale.IsToggled;
            fscaleB = y;
        }

        void Handle_Toggled_5(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var y = death.IsToggled;
            deathB = y;
        }

        void Handle_Toggled_6(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var y = solo.IsToggled;
            soloB = y;
        }

        void Handle_Toggled_7(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var y = assisted.IsToggled;
            assistedB = y;
        }

        void Handle_Toggled_8(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var y = needed.IsToggled;
            neededB = y;
        }

        void Handle_Toggled_9(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var y = platform.IsToggled;
            platformB = y;
        }

        void Handle_Toggled_10(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var y = noclimb.IsToggled;
            noclimbB = y;
        }

        void Handle_Toggled_11(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var y = yellow.IsToggled;
            recyellowB = y;
        }

        void Handle_Toggled_12(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var y = red.IsToggled;
            recredB = y;
        }
      }
}
                                  
