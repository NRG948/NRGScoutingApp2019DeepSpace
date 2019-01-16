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
        public static readonly String DROP_1_DIALOG_TEXT = "Level 1";
        public static readonly String DROP_2_DIALOG_TEXT = "Level 2";
        public static readonly String DROP_3_DIALOG_TEXT = "Level 3";
        public static readonly String DROP_4_DIALOG_TEXT = "Cargo Ship";
        public static readonly String DROP_1_DIALOG_IMAGE = "ic_scale.png";
        public static readonly String DROP_2_DIALOG_IMAGE = "ic_scale.png";
        public static readonly String DROP_3_DIALOG_IMAGE = "ic_scale.png";
        public static readonly String DROP_4_DIALOG_IMAGE = "ic_switch.png";

        //Match Events Page
        public static readonly String PICK_ITEM_1_IMAGE = "ic_picked_cube.png";
        public static readonly String PICK_ITEM_2_IMAGE = "ic_picked_cube.png";
        public static readonly String DROP_ITEM_IMAGE = "ic_drop_cube_yellow.png";
        public static readonly String DROP_1_IMAGE = "ic_scale.png";
        public static readonly String DROP_2_IMAGE = "ic_switch.png";
        public static readonly String DROP_3_IMAGE = "ic_switch.png";
        public static readonly String DROP_COLLECTOR_IMAGE = "ic_exchange.png";
        public static readonly String DROP_NONE_IMAGE = "ic_cancel.png";
        public static readonly String START_CLIMB_IMAGE = "ic_climb_yellow.png";

        public static readonly String PICK_ITEM_1_TEXT = "Picked Hatch";
        public static readonly String PICK_ITEM_2_TEXT = "Picked Cargo";
        public static readonly String DROP_ITEM_TEXT = "Dropped Item";
        public static readonly String DROP_1_TEXT = "Dropped Level 1";
        public static readonly String DROP_2_TEXT = "Dropped Level 2";
        public static readonly String DROP_3_TEXT = "Dropped Level 3";
        public static readonly String DROP_4_TEXT = "Dropped Cargo Ship";
        public static readonly String DROP_NONE_TEXT = "Dropped None";
        public static readonly String START_CLIMB_TEXT = "Start Climb";
        public static readonly String DROP_KEYWORD = "Drop";
        public static readonly String PICK_KEYWORD = "Pick";

        //INTERNAL VARIABLES FOR SETTING IMPORTANT TIMER AND BUTTON VALUES (DO NOT CHANGE THIS)
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

        //RANKER 
        public static readonly int AUTO_LENGTH = 15000;
    }
}
