using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace NRGScoutingApp
{

    /* NOTICE: REFERENCE 
     * resetTimer when changing reset Timer text
     * startTimer when changing start timer text
     * climbStart when changing climb timer text
     * cubePicked when changing cube picked/dropped text
     * To change images for buttons, use the android names above, and use iosCubeImage.Source for the ios cube image
     */
    public partial class NewMatchStart : ContentPage
    {
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public NewMatchStart()
        {
            BindingContext = this;
            InitializeComponent();
            timeSlider.Maximum = ConstantVars.MATCH_SPAN_MS;
            App.Current.Properties["appState"] = "1";
            App.Current.SavePropertiesAsync();
            NavigationPage.SetHasBackButton(this, false);
            timerValueSetter();
            setEventButtons(isTimerRunning);
        }

        private static int min = 0, sec = 0, ms = 0; //Values for Timer
        public static int timerValue = 0;
        private bool firstTimerStart = true;
        public static int pickedTime = 0;
        public static int droppedTime = 0;
        int climbTime = 0;
        public static bool cubeSetDrop;
        private static bool isTimerRunning;
        public static bool setItemToDefault;
        public static List<MatchFormat.Data> events = new List<MatchFormat.Data>();

        protected override void OnAppearing()
        {
            
            if (cubeSetDrop)
            {
                cubePicked.Text = ConstantVars.ITEM_DROPPED_TEXT_LIVE;
                cubePicked.Image = ConstantVars.ITEM_DROPPED_IMAGE_LIVE;
                cubeSetDrop = false;
            }
            if (setItemToDefault) {
                cubePicked.Text = ConstantVars.ITEM_PICKED_TEXT_LIVE;
                cubePicked.Image = ConstantVars.ITEM_PICKED_IMAGE_LIVE;
                setItemToDefault = false;
            }
            setClimbButton();
        }

        async void resetClicked(object sender, System.EventArgs e)
        {
            var ensure = await DisplayActionSheet("Are you sure you want to reset everything about this match?", ConstantVars.CANCEL, null, ConstantVars.YES);
            if (ensure == ConstantVars.YES)
            {
                events.Clear();
                CubeDroppedDialog.saveEvents();
                timeSlider.Value = 0;
                isTimerRunning = false;
                climbTime = 0;
                await App.Current.SavePropertiesAsync();
            }
        }

        async void startClicked(object sender, System.EventArgs e)
        {
            if (!isTimerRunning)
            {
                if (!App.Current.Properties.ContainsKey("timerValue"))
                {
                    App.Current.Properties["timerValue"] = (int)0;
                    App.Current.SavePropertiesAsync();
                }
                else if (App.Current.Properties.ContainsKey("timerValue") && firstTimerStart == true)
                {
                    timerValue = Convert.ToInt32(App.Current.Properties["timerValue"]);
                    timerText.Text = timeToString((int)timerValue);
                    firstTimerStart = false;
                }
                timeSlider.IsEnabled = false;
                isTimerRunning = true;
                startTimer.Text = ConstantVars.TIMER_PAUSE;
                setEventButtons(true);
                setClimbButton();
                await Task.Run(async () =>
                {
                    if (Device.RuntimePlatform == "iOS")
                    {
                        Device.StartTimer(TimeSpan.FromMilliseconds(ConstantVars.TIMER_INTERVAL_IOS), () =>
                        {
                            if (timerValue >= ConstantVars.MATCH_SPAN_MS || !isTimerRunning)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    startTimer.Text = ConstantVars.TIMER_START;
                                });
                                isTimerRunning = false;
                                return false;
                            }
                            Timer_Elapsed(); return true;
                        });
                    }
                    else if (Device.RuntimePlatform == "Android")
                    {

                        while (!(timerValue >= ConstantVars.MATCH_SPAN_MS || !isTimerRunning))
                        {
                            await Task.Delay(ConstantVars.TIMER_INTERVAL_ANDROID);
                            Timer_Elapsed();
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            startTimer.Text = ConstantVars.TIMER_START;
                        });
                        isTimerRunning = false;
                    }
                });
            }
            else if (isTimerRunning)
            {
                startTimer.Text = ConstantVars.TIMER_START;
                isTimerRunning = false;
                timeSlider.IsEnabled = true;
            }
            setEventButtons(isTimerRunning);
            setClimbButton();
        }
        private void Timer_Elapsed()
        {
            if (Device.RuntimePlatform == "iOS")
            {
                ms += ConstantVars.TIMER_INTERVAL_IOS;
                timerValue += ConstantVars.TIMER_INTERVAL_IOS;
            }
            else if (Device.RuntimePlatform == "Android")
            {
                ms += ConstantVars.TIMER_INTERVAL_ANDROID;
                timerValue += ConstantVars.TIMER_INTERVAL_ANDROID;
            }
            if (ms >= ConstantVars.SEC_MS)
            {
                sec++;
                ms = 0;
            }
            if (sec == 60)
            {
                min++;
                sec = 0;
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                timeSlider.Value = timerValue;
                timerText.Text = timeToString((int)timerValue);
            });
            App.Current.Properties["timerValue"] = (int)timerValue;
            App.Current.SavePropertiesAsync();
        }

        void timerValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            if (!App.Current.Properties.ContainsKey("timerValue"))
            {
                App.Current.Properties["timerValue"] = (int)0;
                App.Current.SavePropertiesAsync();
            }
            else if (App.Current.Properties.ContainsKey("timerValue") && firstTimerStart == true)
            {
                timerValue = (int)(App.Current.Properties["timerValue"]);
                firstTimerStart = false;
            }
            else
            {
                App.Current.Properties["timerValue"] = (int)timeSlider.Value;
                App.Current.SavePropertiesAsync();
            }
            double value = e.NewValue;
            timerText.Text = timeToString((int)e.NewValue);
            timerValue = (int)(e.NewValue);
        }


        void climbClicked(object sender, System.EventArgs e)
        {
            if (!isTimerRunning)
            {
                DisplayAlert("Error", "Timer not Started", "OK");
            }
            else if (climbTime == 0)
            {
                //Adds info to to JSON about climb
                climbTime = (int)timerValue;
                events.Add(new MatchFormat.Data { time = climbTime, type = (int)MatchFormat.ACTION.startClimb });
                CubeDroppedDialog.saveEvents();
                setClimbButton();
            }
        }

        async void cubeClicked(object sender, System.EventArgs e)
        {
            if (!isTimerRunning)
            {
                DisplayAlert("Error", "Timer not Started", "OK");
            }

            else if (cubePicked.Text == ConstantVars.ITEM_PICKED_TEXT_LIVE)
            {
                //Performs actions to open popup for adding cube dropped, etc
                pickedTime = (int)timerValue;
                App.Current.Properties["lastItemPicked"] = (int)pickedTime;
                String action = "";
                while (String.IsNullOrWhiteSpace(action))
                {
                    action = await DisplayActionSheet("Choose Pick Type:", ConstantVars.CANCEL, null, ConstantVars.PICK_1_TEXT, ConstantVars.PICK_2_TEXT);
                }
               if (!action.ToString().Equals(ConstantVars.CANCEL))
                {
                    if (action.ToString().Equals(ConstantVars.PICK_1_TEXT))
                    {
                        events.Add(new MatchFormat.Data { time = (int)pickedTime, type = (int)MatchFormat.ACTION.pick1 });
                    }
                    else if (action.ToString().Equals(ConstantVars.PICK_2_TEXT))
                    {
                        events.Add(new MatchFormat.Data { time = (int)pickedTime, type = (int)MatchFormat.ACTION.pick2 });
                    }
                    cubePicked.Image = ConstantVars.ITEM_DROPPED_IMAGE_LIVE;
                    cubePicked.Text = ConstantVars.ITEM_DROPPED_TEXT_LIVE;
                    CubeDroppedDialog.saveEvents();
                    App.Current.SavePropertiesAsync();
                }

            }
            else if (cubePicked.Text == ConstantVars.ITEM_DROPPED_TEXT_LIVE)
            {
                //Performs action/s to open popup for adding cube dropped, etc
                droppedTime = (int)timerValue;
                Navigation.PushAsync(new CubeDroppedDialog());
                cubePicked.Image = ConstantVars.ITEM_PICKED_IMAGE_LIVE;
                cubePicked.Text = ConstantVars.ITEM_PICKED_TEXT_LIVE;
            }
        }

        //Sets buttons to not clickable if timer is/not running.
        private void setEventButtons(bool setter)
        {
            if (setter && isTimerRunning) {
                climbStart.BackgroundColor = Color.FromHex("fdad13");
                cubePicked.BackgroundColor = Color.FromHex("fdad13");
            }
            else {
                climbStart.BackgroundColor = Color.FromHex("ffcc6b");
                cubePicked.BackgroundColor = Color.FromHex("ffcc6b");
            }
            climbStart.IsEnabled = setter;
            cubePicked.IsEnabled = setter;
        }

        //Sets the value of the time if app crashed or match was restored
        private void timerValueSetter()
        {
            if (!App.Current.Properties.ContainsKey("lastItemPicked"))
            {
                App.Current.Properties["lastItemPicked"] = 0;
                App.Current.Properties["lastItemDroppped"] = 0;
                App.Current.Properties["tempEventString"] = "";
                App.Current.Properties["tempMatchEvents"] = "";
                App.Current.SavePropertiesAsync();
            }
            else if (Convert.ToInt32(App.Current.Properties["lastItemPicked"]) == 0 || Convert.ToInt32(App.Current.Properties["lastItemDropped"]) == 0) { }
            else if (Convert.ToInt32(App.Current.Properties["lastItemDroppped"]) > Convert.ToInt32(App.Current.Properties["lastItemDropped"]))
            {
                cubePicked.Image = ConstantVars.ITEM_DROPPED_IMAGE_LIVE;
                cubePicked.Text = ConstantVars.ITEM_DROPPED_TEXT_LIVE;
            }

            if (!App.Current.Properties.ContainsKey("timerValue"))
            {
                App.Current.Properties["timerValue"] = (int)timerValue;
                App.Current.SavePropertiesAsync();
            }
            else if (App.Current.Properties.ContainsKey("timerValue") && firstTimerStart == true)
            {
                timerValue = Convert.ToInt32(App.Current.Properties["timerValue"]);
                timeSlider.Value = timerValue;
                timerText.Text = timeToString((int)timerValue);
                firstTimerStart = false;
            }
        }

        //sets robot action buttons based on climb start
        private void setClimbButton()
        {
            if (events.Exists(x => x.type == (int)MatchFormat.ACTION.startClimb))
            {
                setEventButtons(false);
            }
            else
            {
                setEventButtons(true);
                climbTime = 0;
            }
        }

        public static string timeToString(int timeValue)
        {
            int minutes = 0;
            int seconds = 0;
            int milliseconds = 0;
            minutes = timeValue / (int)ConstantVars.MIN_MS;
            timeValue %= (int)ConstantVars.MIN_MS;
            seconds = timeValue / (int)ConstantVars.SEC_MS;
            timeValue %= (int)ConstantVars.SEC_MS;
            milliseconds = timeValue;
            return minutes + ":" + seconds.ToString("D2") + "." + (milliseconds / 10).ToString("D2");
        }
    }
}

