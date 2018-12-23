using System;
using System.Collections.Generic;
using NRGScoutingApp;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Forms.Platform;
using Xamarin.Forms.Xaml;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using Rg.Plugins.Popup.Services;
using System.Linq.Expressions;
using System.Security.AccessControl;



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

        //INTERNAL VARIABLES FOR SETTING IMPORTANT TIMER AND BUTTON VALUES (DO NOT CHANGE THIS)
        public static readonly double MATCH_SPAN_MS = 150000;
        public static readonly double MIN_MS = 60000;
        public static readonly double SEC_MS = 1000;
        public static readonly String ITEM_PICKED_TEXT = "Cube Picked";
        public static readonly String ITME_DROPPED_TEXT = "Cube Dropped";
        public static readonly String TIMER_START = "Start Timer";
        public static readonly String TIMER_PAUSE = "Pause Timer";

        public static readonly String ITEM_PICK = "itemPick";
        public static readonly String ITEM_DROP = "itemDrop";
        public static readonly String ROBOT_CLIMB = "robotclimb";
        public static readonly int NUM_DROP_OPTIONS = 4;
        public static readonly String SEPARATOR = "####";

        public static readonly int TIMER_INTERVAL_ANDROID = 100;
        public static readonly int TIMER_INTERVAL_IOS = 1;

        public NewMatchStart()
        {
            BindingContext = this;
            InitializeComponent();
            timeSlider.Maximum = MATCH_SPAN_MS;
            App.Current.Properties["appState"] = "1";
            App.Current.SavePropertiesAsync();
            NavigationPage.SetHasBackButton(this, false);
            timerValueSetter();
        }

        private static int min = 0, sec = 0, ms = 0; //Values for Timer
        public static int timerValue = 0;
        private Boolean firstTimerStart = true;
        public static double pickedTime = 0;
        public static double droppedTime = 0;
        public static int pickNum = 0, dropNum = 0;
        int climbTime = 0;
        public static Boolean cubeSetDrop = false;
        private static Boolean isTimerRunning = false;
        public static String matchEvents = "";

        protected override void OnAppearing()
        {
            if (cubeSetDrop)
            {
                cubePicked.Text = ITME_DROPPED_TEXT;
                cubePicked.Image = "ic_drop_cube.png";
                cubeSetDrop = false;
            }
        }

        void resetClicked(object sender, System.EventArgs e)
        {
            if (timerText.Text == "0:00.00" || isTimerRunning) {}
            else if (!isTimerRunning)
            {
                timeSlider.Value = 0;
                min = 0; sec = 0; ms = 0;
                timerValue = 0;
                App.Current.Properties["timerValue"] = (int)0;
                App.Current.SavePropertiesAsync();
                timerText.Text = "0:00.00";
            }
        }

        void startClicked(object sender, System.EventArgs e)
        {
            if (!isTimerRunning) 
            {
                if (!App.Current.Properties.ContainsKey("timerValue")) {
                    App.Current.Properties["timerValue"] = (int) 0;
                    App.Current.SavePropertiesAsync();
                }
                else if (App.Current.Properties.ContainsKey("timerValue") && firstTimerStart== true)
                {
                    timerValue = Convert.ToInt32(App.Current.Properties["timerValue"]);
                    timerText.Text = timeToString((int)timerValue);
                    firstTimerStart = false;
                }
                timeSlider.IsEnabled = false;
                isTimerRunning = true;
                startTimer.Text = TIMER_PAUSE;
                if (Device.RuntimePlatform == "iOS"){
                    Device.StartTimer(TimeSpan.FromMilliseconds(TIMER_INTERVAL_IOS), () =>
                    {
                        if (timerValue >= MATCH_SPAN_MS || !isTimerRunning)
                        {
                            startTimer.Text = TIMER_START;
                            return false;
                        }
                        Timer_Elapsed(); return true;
                    });
                }
                else if (Device.RuntimePlatform == "Android"){
                    Device.StartTimer(TimeSpan.FromMilliseconds(TIMER_INTERVAL_ANDROID), () =>
                    {
                        if (timerValue >= MATCH_SPAN_MS || !isTimerRunning)
                        {
                            startTimer.Text = TIMER_START;
                            return false;
                        }
                        Timer_Elapsed(); 
                        return true;
                    });
                }

            }
            else if (isTimerRunning)
            {
                startTimer.Text = TIMER_START;
                isTimerRunning = false;
                timeSlider.IsEnabled = true;
            }
        }
        private void Timer_Elapsed()
        {
            if (Device.RuntimePlatform=="iOS"){
                ms += TIMER_INTERVAL_IOS;
                timerValue += TIMER_INTERVAL_IOS;
            }
            else if (Device.RuntimePlatform == "Android")
            {
                ms += TIMER_INTERVAL_ANDROID;
                timerValue += TIMER_INTERVAL_ANDROID;
            }
            if (ms >= SEC_MS)
            {
                sec++;
                ms = 0;
            }
            if (sec == 60)
            {
                min++;
                sec = 0;
            }
            timeSlider.Value = timerValue;
            App.Current.Properties["timerValue"] = (int)timerValue;
            App.Current.SavePropertiesAsync();
            timerText.Text = timeToString((int)timerValue);
        }

        void timerValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
                if (!App.Current.Properties.ContainsKey("timerValue")){
                    App.Current.Properties["timerValue"] = (int) 0;
                    App.Current.SavePropertiesAsync();
                }
                else if(App.Current.Properties.ContainsKey("timerValue") && firstTimerStart == true)
                {
                    timerValue = (int)(App.Current.Properties["timerValue"]);
                    firstTimerStart = false;
                    }
                else{
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
                NewMatchStart.matchEvents += ROBOT_CLIMB + ":" + climbTime;
                CubeDroppedDialog.saveEvents();
            }
        }

        void cubeClicked(object sender, System.EventArgs e)
        {
            if (!isTimerRunning)
            {
                DisplayAlert("Error", "Timer not Started", "OK");
            }

            else if (cubePicked.Text == ITEM_PICKED_TEXT || cubePicked.Text == ITEM_PICKED_TEXT)
            {
                //Performs actions to open popup for adding cube dropped, etc
                pickedTime = (int)timerValue;
                App.Current.Properties["lastCubePicked"] = (int)pickedTime;
                App.Current.SavePropertiesAsync();
                CubeDroppedDialog.saveEvents();
                matchEvents += ITEM_PICK + ":" + pickedTime + SEPARATOR;
                pickNum++;
                cubePicked.Image = "ic_drop_cube.png";
                cubePicked.Text = ITME_DROPPED_TEXT;

            }
            else if (cubePicked.Text == ITME_DROPPED_TEXT)
            {
                //Performs action/s to open popup for adding cube dropped, etc
                droppedTime =(int)timerValue;
                Navigation.PushAsync(new CubeDroppedDialog());
                cubePicked.Image = "ic_picked_cube.png";
                cubePicked.Text = ITEM_PICKED_TEXT;
            }
        }



        //TODO: Call Parse Method for match number called and figure out the pickNum and dropNum values and set them
        private void timerValueSetter()
        {
            if(!App.Current.Properties.ContainsKey("lastCubePicked")){
                App.Current.Properties["lastCubePicked"] = 0;
                App.Current.Properties["lastCubeDropped"] = 0;
                App.Current.Properties["tempEventString"] = "";
                App.Current.Properties["matchEventsString"] = "";
                App.Current.SavePropertiesAsync();
            }
            else if(Convert.ToInt32(App.Current.Properties["lastCubePicked"]) == 0 || Convert.ToInt32(App.Current.Properties["lastCubeDropped"]) == 0){}
            else if(Convert.ToInt32(App.Current.Properties["lastCubePicked"]) > Convert.ToInt32(App.Current.Properties["lastCubeDropped"])){
                cubePicked.Image = "ic_drop_cube.png";
                cubePicked.Text = ITME_DROPPED_TEXT;
            }

            if (!App.Current.Properties.ContainsKey("timerValue"))
            {
                App.Current.Properties["timerValue"] = (int)timerValue;
                App.Current.Properties["tempEventString"] = "(";
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

        public static string timeToString(int timeValue){
            int minutes = 0;
            int seconds = 0;
            int milliseconds = 0;
            minutes = timeValue / (int)MIN_MS;
            timeValue %= (int)MIN_MS;
            seconds = timeValue / (int)SEC_MS;
            timeValue %= (int)SEC_MS;
            milliseconds = timeValue;
            return minutes + ":" + seconds.ToString("D2") + "." + (milliseconds / 10).ToString("D2");
        }

    }

}


