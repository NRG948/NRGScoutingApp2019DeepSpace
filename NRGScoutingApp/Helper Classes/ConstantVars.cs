using System;
namespace NRGScoutingApp
{
    public class ConstantVars
    {
        /*
         * IMPORTANT NOTE:
         * VARIABLES CONTAINING "LIVE" ARE USED IN the NewMatchStart page
         */

        /*
         * CubeDropDialog
         */
        public static readonly String DROP_1_DIALOG_TEXT = "Low";
        public static readonly String DROP_2_DIALOG_TEXT = "Medium";
        public static readonly String DROP_3_DIALOG_TEXT = "High";
        public static readonly String DROP_4_DIALOG_TEXT = "Ship";

        /*
         * Match Events Page
         */
        public static readonly String PICK_ITEM_1_IMAGE = "ic_picked_cube.png";
        public static readonly String PICK_ITEM_2_IMAGE = "ic_picked_cube.png";
        public static readonly String DROP_ITEM_IMAGE = "ic_drop_cube_yellow.png";
        public static readonly String DROP_1_IMAGE = "ic_scale.png";
        public static readonly String DROP_2_IMAGE = "ic_switch.png";
        public static readonly String DROP_3_IMAGE = "ic_switch.png";
        public static readonly String DROP_4_IMAGE = "ic_switch.png";
        public static readonly String DROP_COLLECTOR_IMAGE = "ic_exchange.png";
        public static readonly String DROP_NONE_IMAGE = "ic_cancel.png";
        public static readonly String START_CLIMB_IMAGE = "ic_climb_yellow.png";

        public static readonly String PICK_ITEM_1_TEXT = "Picked Hatch";
        public static readonly String PICK_ITEM_2_TEXT = "Picked Cargo";
        public static readonly String DROP_ITEM_TEXT = "Dropped Item";
        public static readonly String DROP_1_TEXT = "Dropped Low";
        public static readonly String DROP_2_TEXT = "Dropped Medium";
        public static readonly String DROP_3_TEXT = "Dropped High";
        public static readonly String DROP_4_TEXT = "Dropped Ship";
        public static readonly String DROP_NONE_TEXT = "Dropped None";
        public static readonly String START_CLIMB_TEXT = "Start Climb";
        public static readonly String DROP_KEYWORD = "Drop";
        public static readonly String PICK_KEYWORD = "Pick";

        /*
         * INTERNAL VARIABLES FOR SETTING IMPORTANT TIMER AND BUTTON VALUES (DO NOT CHANGE THIS)
         */
        public static readonly double MATCH_SPAN_MS = 150000;
        public static readonly double MIN_MS = 60000;
        public static readonly double SEC_MS = 1000;
        public static readonly String ITEM_PICKED_TEXT_LIVE = "Item Picked";
        public static readonly String ITEM_DROPPED_TEXT_LIVE = "Item Dropped";
        public static readonly String ITEM_DROPPED_IMAGE_LIVE = "ic_drop_cube.png";
        public static readonly String ITEM_PICKED_IMAGE_LIVE = "ic_picked_cube.png";
        public static readonly String TIMER_START = "Start Timer";
        public static readonly String TIMER_PAUSE = "Pause Timer";
        public static readonly String PICK_1_TEXT = "Pick Hatch";
        public static readonly String PICK_2_TEXT = "Pick Cargo";
        public static readonly String CANCEL = "Cancel";
        public static readonly String YES = "Yes";

        public static readonly String ITEM_PICK = "itemPick";
        public static readonly String ITEM_DROP = "itemDrop";
        public static readonly String ROBOT_CLIMB = "robotclimb";
        public static readonly int NUM_DROP_OPTIONS = 4;

        public static readonly int TIMER_INTERVAL_ANDROID = 100;
        public static readonly int TIMER_INTERVAL_IOS = 1;

        public static readonly String RED_1_TEXT = "Red 1";
        public static readonly String RED_2_TEXT = "Red 2";
        public static readonly String RED_3_TEXT = "Red 3";
        public static readonly String BLUE_1_TEXT = "Blue 1";
        public static readonly String BLUE_2_TEXT = "Blue 2";
        public static readonly String BLUE_3_TEXT = "Blue 3";

        //PARAMETERS PAGE
        public static readonly String LVL_1_CLIMB = "Level 1";
        public static readonly String LVL_2_CLIMB = "Level 2";
        public static readonly String LVL_3_CLIMB = "Level 3";


        /*
         * RANKER VALUES
         * NOTE: THE LOWER THE VALUES FOR RANK, THE BETTER
         */        
        //Autonomous
        public static readonly int AUTO_LENGTH = 15000;
        public static readonly double MULT_SANDSTORM_MANUAL = 1;
        public static readonly double MULT_SANDSTORM_AUTO = 0.5;
        public static readonly double PTS_BASELINE_NONE = 0;
        public static readonly double PTS_BASELINE_LVL_1 = 1;
        public static readonly double PTS_BASELINE_LVL_2 = 3;

        //Game Piece Manipulation
        public static readonly double PTS_GAME_PIECE= 1;
        public static readonly double PTS_DROP_LVL_1 = 1;
        public static readonly double PTS_DROP_LVL_2 = 2;
        public static readonly double PTS_DROP_LVL_3 = 3;

        // This is just examples of multiplier, should be changed soon
        public static readonly double TIME_NERF = 10000;
        public static double CARGO_MULTIPLIER = 3;
        public static readonly double HATCHER_MULTIPLIER = 2 / TIME_NERF;
        public static readonly double CLIMB_MULTIPLIER = 1 / TIME_NERF;
        public static readonly double DROP_1_MULTIPLIER = 1 / TIME_NERF;
        public static readonly double DROP_2_MULTIPLIER = 2 / TIME_NERF;
        public static readonly double DROP_3_MULTIPLIER = 3 / TIME_NERF;
        public static readonly double DROP_4_MULTIPLIER = 3 / TIME_NERF;

        //Climb
        public static readonly double PTS_NEED_HELP_LVL_2 = 1;
        public static readonly double PTS_NEED_HELP_LVL_3 = 1;
        public static readonly double PTS_SELF_LVL_1 = 1;
        public static readonly double PTS_SELF_LVL_2 = 2;
        public static readonly double PTS_SELF_LVL_3 = 4;
        public static readonly double PTS_HELPED_LVL_2 = 1;
        public static readonly double PTS_HELPED_LVL_3 = 2;


        /*
         * Rankings Detail View Page
         */
        public static readonly String[] scoreBaseVals = { "Overall: ", "Cargo: ", "Hatch: ", "Climb: ", "Low: ", "Medium: ", "High: ", "Ship: "};
        public static readonly int numRankTypes = scoreBaseVals.Length;
        public static readonly String noVal = "Empty";

        /*
         * PIT SCOUTING
         */
        //Separates the entries if same team was scouted twice
        public static readonly String entrySeparator = "\nANOTHER PIT ENTRY:::::::\n";
        public static readonly String[] QUESTIONS =
        {"Hours practiced?",
        "Drive base?",
        "How many hatch panels/cargo do you average per match?",
        "What do you focus on (rocket/cargo ship)?",
        "What level(s) can your bot reach?",
        "All positions in auto for sandstorm?",
        "Auto vs tele for sandstorm?",
        "Can you get to the highest platform in the hab? What is your prefered method in endgame?",
        "Speed?",
        "Mechanism?",
        "Placement?" };
    }
}
