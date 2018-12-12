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
		public MatchTimer ()
		{
			InitializeComponent ();
		}

        private void ResetTimer_Clicked(object sender, EventArgs e)
        {

        }

        private void TimeStart_Clicked(object sender, EventArgs e)
        {

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
        }
        public static String numToTime(double a)
        {
            int num = (int) a;
            int min = num / 60000;
            int sec = (num % 60000) / 1000;
            int millisec = num % 1000;
            return (min + ":" + formatNum(sec, 2) + "." + formatNum(millisec, 3));

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
                while (change < 0)
                {
                    result = "0" + result;
                    change++;
                }
            }
            return result;
        }
    }
}