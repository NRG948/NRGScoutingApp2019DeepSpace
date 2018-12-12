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
        private int timerValue = 0;

        public MatchTimer ()
		{
			InitializeComponent ();
		}

        private void ResetTimer_Clicked(object sender, EventArgs e)
        {
            
        }

        private void TimeStart_Clicked(object sender, EventArgs e)
        {
            if (timeStart.Text.Equals(timerStart)) //timerStart
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                {
                    if (timerValue >= matchLengthMs || timeStart.Text.Equals(timerPause))
                    {
                        timeStart.Text = timerPause;
                        timerValue = matchLengthMs;
                        timeValue.Text = numToTime(timerValue);
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

            }
        }

        private void timerElapsed()
        {
            timerValue += 50;
            timeValue.Text = numToTime(timerValue);
            timeSlider.Value = timerValue;
        }

        private void ClimbStart_Clicked(object sender, EventArgs e)
        {

        }

        private void CubePicked_Clicked(object sender, EventArgs e)
        {

        }

        private void TimeSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
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