using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.Security.Cryptography.X509Certificates;
//using Security;
using System.Globalization;

namespace NRGScoutingApp
{
    public partial class CubeDroppedDialog
    {
        public CubeDroppedDialog()
        {
            BindingContext = this; 
            InitializeComponent();
        }
        void scaleClicked(object sender, System.EventArgs e)
        {
            App.matchEvents += "cubeDroppedScale" + NewMatchStart.dropNum + ":" + NewMatchStart.droppedTime + "||";
            saveEvents();
            setDropTime();
            NewMatchStart.dropNum++;
            PopupNavigation.Instance.PopAsync(true);
        }
        void allySwitchClicked(object sender, System.EventArgs e)
        {
            App.matchEvents += "cubeDroppedAllySwitch" + NewMatchStart.dropNum + ":" + NewMatchStart.droppedTime + "||";
            saveEvents();
            setDropTime();
            NewMatchStart.dropNum++;
            PopupNavigation.Instance.PopAsync(true);
        }
        void noneClicked(object sender, System.EventArgs e)
        {
            App.matchEvents += "cubeDroppedNone" + NewMatchStart.dropNum + ":" + NewMatchStart.droppedTime + "||";
            saveEvents();
            setDropTime();
            NewMatchStart.dropNum++;
            PopupNavigation.Instance.PopAsync(true);
        }
        void oppSwitchClicked(object sender, System.EventArgs e)
        {
            App.matchEvents += "cubeDroppedOppSwitch" + NewMatchStart.dropNum + ":" + NewMatchStart.droppedTime + "||";
            saveEvents();
            setDropTime();
            NewMatchStart.dropNum++;
            PopupNavigation.Instance.PopAsync(true);
        }
        void exchangeClicked(object sender, System.EventArgs e)
        {
            App.matchEvents += "cubeDroppedExchange" + NewMatchStart.dropNum + ":" + NewMatchStart.droppedTime + "||";
            saveEvents();
            setDropTime();
            NewMatchStart.dropNum++;
            PopupNavigation.Instance.PopAsync(true);
        }
        void backClicked(object sender, System.EventArgs e)
        {
            Button cubePicked = (Button)sender;
           //cubePicked.Text = "New Value";
            //App.matchEvents += "cubeDropped:back||";

        }
        public static void saveEvents(){
            App.Current.Properties["tempEventString"] = App.matchEvents;
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