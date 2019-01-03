using System;
namespace NRGScoutingApp
{
    public class ConstantVars
    {
        /*
         * IMPORTANT NOTE:
         * VARIABLES CONTAINING "LIVE" ARE USED IN the NewMatchStart page
         */        

        //CubeDropDialog
        public static readonly String DROP_1_DIALOG_TEXT = "Scale";
        public static readonly String DROP_2_DIALOG_TEXT = "Ally Switch";
        public static readonly String DROP_3_DIALOG_TEXT = "Opp Switch";
        public static readonly String DROP_COLLECTOR_DIALOG_TEXT = "Exchange";
        public static readonly String DROP_1_DIALOG_IMAGE = "ic_scale.png";
        public static readonly String DROP_2_DIALOG_IMAGE = "ic_switch.png";
        public static readonly String DROP_3_DIALOG_IMAGE = "ic_switch.png";

        //Match Events Page
        public static readonly String PICK_ITEM_IMAGE = "ic_picked_cube.png";
        public static readonly String DROP_ITEM_IMAGE = "ic_drop_cube_yellow.png";
        public static readonly String DROP_1_IMAGE = "ic_scale.png";
        public static readonly String DROP_2_IMAGE = "ic_switch.png";
        public static readonly String DROP_3_IMAGE = "ic_switch.png";
        public static readonly String DROP_COLLECTOR_IMAGE = "ic_exchange.png";
        public static readonly String DROP_NONE_IMAGE = "ic_cancel.png";
        public static readonly String START_CLIMB_IMAGE = "ic_climb_yellow.png";

        public static readonly String PICK_ITEM_TEXT = "Picked Cube";
        public static readonly String DROP_ITEM_TEXT = "Dropped Cube";
        public static readonly String DROP_1_TEXT = "Dropped Scale";
        public static readonly String DROP_2_TEXT = "Dropped Ally Switch";
        public static readonly String DROP_3_TEXT = "Dropped Opp. Switch";
        public static readonly String DROP_COLLECTOR_TEXT = "Dropped Exchange";
        public static readonly String DROP_NONE_TEXT = "Dropped None";
        public static readonly String START_CLIMB_TEXT = "Start Climb";
        public static readonly String DROP_KEYWORD = "Drop";
        public static readonly String PICK_KEYWORD = "Pick";

        //INTERNAL VARIABLES FOR SETTING IMPORTANT TIMER AND BUTTON VALUES (DO NOT CHANGE THIS)
        public static readonly double MATCH_SPAN_MS = 150000;
        public static readonly double MIN_MS = 60000;
        public static readonly double SEC_MS = 1000;
        public static readonly String ITEM_PICKED_TEXT_LIVE = "Cube Picked";
        public static readonly String ITEM_DROPPED_TEXT_LIVE = "Cube Dropped";
        public static readonly String ITEM_DROPPED_IMAGE_LIVE = "ic_drop_cube.png";
        public static readonly String ITEM_PICKED_IMAGE_LIVE = "ic_picked_cube.png";
        public static readonly String TIMER_START = "Start Timer";
        public static readonly String TIMER_PAUSE = "Pause Timer";

        public static readonly String ITEM_PICK = "itemPick";
        public static readonly String ITEM_DROP = "itemDrop";
        public static readonly String ROBOT_CLIMB = "robotclimb";
        public static readonly int NUM_DROP_OPTIONS = 4;

        public static readonly int TIMER_INTERVAL_ANDROID = 100;
        public static readonly int TIMER_INTERVAL_IOS = 1;



        public static readonly String red1Text = "Red 1";
        public static readonly String red2Text = "Red 2";
        public static readonly String red3Text = "Red 3";
        public static readonly String blue1Text = "Blue 1";
        public static readonly String blue2Text = "Blue 2";
        public static readonly String blue3Text = "Blue 3";
    }
}
