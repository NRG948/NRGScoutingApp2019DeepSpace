using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PracticeNRGScouting2018
{
	public partial class MatchTimer : ContentPage
	{
        public static readonly int matchLengthMs = 150000;
        public static readonly String cubePickedText = "Cube Picked";
        public static readonly String cubeDroppedText = "Cube Dropped";
        public static readonly String timerStart = "Start Timer";
        public static readonly String timerPause = "Pause Timer";
        public static readonly String cubePick = "Cube Pick";
        public static readonly String cubeDrop = "Cube Drop";

        public static readonly int timerDelay = 50;
        private int timerValue = 0;
        public static bool timerRunning = false;

        public MatchTimer ()
		{
			InitializeComponent ();
		}

        private void ResetTimer_Clicked(object sender, EventArgs e)
        {
            
        }

        private void TimeStart_Clicked(object sender, EventArgs e)
        {
            timerRunning = true;
            if (timeStart.Text.Equals(timerStart))
            {
                timeStart.Text = timerPause;
                Device.StartTimer(TimeSpan.FromMilliseconds(timerDelay), () =>
                {
                    if (timerValue >= matchLengthMs || timeStart.Text.Equals(timerStart) || !timerRunning)
                    {
                        timeValue.Text = numToTime(timerValue);
                        timeStart.Text = timerStart;
                        return false;
                    }
                    timerElapsed();
                    return true;
                });
            }
            else if (timeStart.Text.Equals(timerPause))
            {
                timeStart.Text = timerStart;
            }
            else
            {
                timeStart.Text = timerStart;
            }
        }

        private void timerElapsed()
        {
            timerValue += timerDelay;
            timeSlider.Value = timerValue;
            timeValue.TextColor = Color.FromRgb(255, (int) (timerValue * 200 / matchLengthMs), 0);
        }

        private void ClimbStart_Clicked(object sender, EventArgs e)
        {

        }

        private void CubePicked_Clicked(object sender, EventArgs e)
        {
            if (cubePicked.Text.Equals(cubePick))
            {
                cubePicked.Text = cubeDrop;
            }
            else if (cubePicked.Text.Equals(cubeDrop))
            {
                cubePicked.Text = cubePick;
            }
            else
            {
                cubePicked.Text = cubePick;
            }
        }

        private void TimeSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (timeSlider.Value != timerValue)
            {
                timeValue.TextColor = Color.FromRgb(225, 0, 0);
                timerRunning = false;
            }
            timeValue.Text = numToTime(e.NewValue);
            timerValue = (int) e.NewValue;
        }
        public static String numToTime(double a)
        {
            int num = (int) a;
            int min = num / 60000;
            int sec = (num % 60000) / 1000;
            int millisec = num % 1000;
            return (min + ":" + sec.ToString("D2") + "." + formatNum(millisec,2)); //formatNum(sec, 2) //formatNum(millisec, 2)

        }

        public static String formatNum(int num, int digits) // fill in 0 for better format
        {
            String result = "" + num;
            int change = result.Length - digits;
            if (change > 0)
            {
                result = result.Substring(0, digits);
            }
            else if (change < 0)
            {
                result = repeatString("0", -change) + result;
            }
            return result;
        }
        public static String repeatString(String str, int times)
        {
            String result = "";
            for (int i = 0; i < times; i++)
            {
                result += str;
            }
            return result;
        }
    }
}