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
        void scaleClicked(object sender, System.EventArgs e)
        {
            saveEvents();
            setDropTime();
            NewMatchStart.dropNum++;
            Navigation.PopAsync(true);
        }
        void allySwitchClicked(object sender, System.EventArgs e)
        {
            saveEvents();
            setDropTime();
            NewMatchStart.dropNum++;
            Navigation.PopAsync(true);
        }
        void noneClicked(object sender, System.EventArgs e)
        {
            saveEvents();
            setDropTime();
            NewMatchStart.dropNum++;
            Navigation.PopAsync(true);
        }
        void oppSwitchClicked(object sender, System.EventArgs e)
        {
            saveEvents();
            setDropTime();
            NewMatchStart.dropNum++;
            Navigation.PopAsync(true);
        }
        void exchangeClicked(object sender, System.EventArgs e)
        {
            saveEvents();
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