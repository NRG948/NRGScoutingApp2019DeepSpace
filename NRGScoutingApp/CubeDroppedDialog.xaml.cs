using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;

namespace NRGScoutingApp
{
    public partial class CubeDroppedDialog
    {
        public CubeDroppedDialog()
        {
            NavigationPage.SetHasBackButton(this, false);
            BindingContext = this; 
            InitializeComponent();
        }

        public static int drop_none = 0;
        public static int drop_1 = 1;
        public static int drop_2 = 2;
        public static int drop_3 = 3;
        public static int drop_4 = 4;
        /*
         * None = 0       
         * Scale = 1
         * Ally Switch = 2        
         * Opp. Switch = 3
         * Exchange = 4
         */
        void scaleClicked(object sender, System.EventArgs e)
        {
            saveEvents();
            NewMatchStart.matchEvents += NewMatchStart.ITEM_DROP + "_" + drop_1 + ":" + NewMatchStart.droppedTime;
            setDropTime();
            NewMatchStart.dropNum++;
            Navigation.PopAsync(true);
        }
        void allySwitchClicked(object sender, System.EventArgs e)
        {
            saveEvents();
            NewMatchStart.matchEvents += NewMatchStart.ITEM_DROP + "_" + drop_2 + ":" + NewMatchStart.droppedTime;
            setDropTime();
            NewMatchStart.dropNum++;
            Navigation.PopAsync(true);
        }
        void noneClicked(object sender, System.EventArgs e)
        {
            saveEvents();
            NewMatchStart.matchEvents += NewMatchStart.ITEM_DROP + "_" + drop_none + ":" + NewMatchStart.droppedTime;
            setDropTime();
            NewMatchStart.dropNum++;
            Navigation.PopAsync(true);
        }
        void oppSwitchClicked(object sender, System.EventArgs e)
        {
            saveEvents();
            NewMatchStart.matchEvents += NewMatchStart.ITEM_DROP + "_" + drop_3 + ":" + NewMatchStart.droppedTime;
            setDropTime();
            NewMatchStart.dropNum++;
            Navigation.PopAsync(true);
        }
        void exchangeClicked(object sender, System.EventArgs e)
        {
            saveEvents();
            NewMatchStart.matchEvents += NewMatchStart.ITEM_DROP + "_" + drop_4 + ":" + NewMatchStart.droppedTime;
            setDropTime();
            NewMatchStart.dropNum++;
            Navigation.PopAsync(true);
        }
        void backClicked(object sender, System.EventArgs e)
        {
            Button cubePicked = (Button)sender;
            NewMatchStart.cubeSetDrop = true;
            Navigation.PopAsync(true);

        }
        public static void saveEvents(){
            App.Current.SavePropertiesAsync();
        }
        public static void setDropTime(){
            App.Current.Properties["lastCubeDropped"] = (int)NewMatchStart.droppedTime;
            App.Current.SavePropertiesAsync();
        }

    }
    //public class NewMatchStart : ContentPage
    //{
    //    public NewMatchStart()
    //    {
    //        cubePicked.Text = "Cube Picked";
    //    }
    //}
}