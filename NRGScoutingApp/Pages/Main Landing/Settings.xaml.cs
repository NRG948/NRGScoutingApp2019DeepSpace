using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRGScoutingApp.Pages.Main_Landing
{

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
        public static readonly double MULT_SANDSTORM_MANUAL = 1;
        public static readonly double MULT_SANDSTORM_AUTO = 0.5;
        public static readonly double PTS_BASELINE_NONE = 0;
        public static readonly double PTS_BASELINE_LVL_1 = 1;
        public static readonly double PTS_BASELINE_LVL_2 = 3;

        //Game Piece Manipulation
        public static readonly double PTS_GAME_PIECE = 1;
        public static readonly double PTS_DROP_LVL_1 = 1;
        public static readonly double PTS_DROP_LVL_2 = 2;
        public static readonly double PTS_DROP_LVL_3 = 3;

        // This is just examples of multiplier, should be changed soon
        public static readonly double TIME_NERF = 10000;
        public static readonly double OVERALL_MULT = 10;
        public static readonly double CARGO_MULTIPLIER = 3 * ConstantVars.MATCH_SPAN_MS;
        public static readonly double HATCHER_MULTIPLIER = 2 * ConstantVars.MATCH_SPAN_MS;
        public static readonly double CLIMB_MULTIPLIER = 5;
        public static readonly double DROP_1_MULTIPLIER = 1 * ConstantVars.MATCH_SPAN_MS;
        public static readonly double DROP_2_MULTIPLIER = 2 * ConstantVars.MATCH_SPAN_MS;
        public static readonly double DROP_3_MULTIPLIER = 3 * ConstantVars.MATCH_SPAN_MS;
        public static readonly double DROP_4_MULTIPLIER = 3 * ConstantVars.MATCH_SPAN_MS;

        //Climb
        public static readonly double PTS_NEED_HELP_LVL_2 = 1;
        public static readonly double PTS_NEED_HELP_LVL_3 = 1;
        public static readonly double PTS_SELF_LVL_1 = 1;
        public static readonly double PTS_SELF_LVL_2 = 2;
        public static readonly double PTS_SELF_LVL_3 = 4;
        public static readonly double PTS_HELPED_LVL_2 = 1;
        public static readonly double PTS_HELPED_LVL_3 = 2;
        public static readonly int PTS_LVL_1_CLIMB = 3;
        public static readonly int PTS_LVL_2_CLIMB = 6;
        public static readonly int PTS_LVL_3_CLIMB = 12;

        public Settings ()
		{
			InitializeComponent ();
		}
	}
}