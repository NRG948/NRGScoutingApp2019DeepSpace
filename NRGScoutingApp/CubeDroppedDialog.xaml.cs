using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using UIKit;

namespace NRGScoutingApp
{
    public partial class CubeDroppedDialog
    {
        readonly String drop1Text = "Scale";
        readonly String drop2Text = "Ally Switch";
        readonly String drop3Text = "Opp Switch";
        readonly String dropItemCollectorText = "Exchange";
        readonly String drop1Image = "ic_scale.png";
        readonly String drop2Image = "ic_switch.png";
        readonly String drop3Image = "ic_switch.png";

        public CubeDroppedDialog()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            setButtons();
        }

        //DROP_TYPE Specified in MatchFormat Class

        void drop1(object sender, System.EventArgs e)
        {
            NewMatchStart.matchEvents += NewMatchStart.ITEM_DROP + "_" + MatchFormat.DROP_TYPE.drop1 + ":" + NewMatchStart.droppedTime;
            setDropTime();
            saveEvents();
            Navigation.PopAsync(true);
        }
        void drop2(object sender, System.EventArgs e)
        {
            NewMatchStart.matchEvents += NewMatchStart.ITEM_DROP + "_" + MatchFormat.DROP_TYPE.drop2 + ":" + NewMatchStart.droppedTime;
            setDropTime();
            saveEvents();
            Navigation.PopAsync(true);
        }
        void noneClicked(object sender, System.EventArgs e)
        {
            NewMatchStart.matchEvents += NewMatchStart.ITEM_DROP + "_" + MatchFormat.DROP_TYPE.dropNone + ":" + NewMatchStart.droppedTime;
            saveEvents();
            setDropTime();
            Navigation.PopAsync(true);
        }
        void drop3(object sender, System.EventArgs e)
        {
            NewMatchStart.matchEvents += NewMatchStart.ITEM_DROP + "_" + MatchFormat.DROP_TYPE.drop3 + ":" + NewMatchStart.droppedTime;
            saveEvents();
            setDropTime();
            NewMatchStart.dropNum++;
            Navigation.PopAsync(true);
        }
        void dropItemCollector(object sender, System.EventArgs e)
        {
            NewMatchStart.matchEvents += NewMatchStart.ITEM_DROP + "_" + MatchFormat.DROP_TYPE.dropItemCollector + ":" + NewMatchStart.droppedTime;
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
            App.Current.Properties["matchEventString"] = NewMatchStart.matchEvents;
            App.Current.SavePropertiesAsync();
        }
        private void setDropTime(){
            App.Current.Properties["lastCubeDropped"] = (int)NewMatchStart.droppedTime;
            App.Current.SavePropertiesAsync();
        }
        private void setButtons() {
            drop1Button.Text = drop1Text;
            drop2Button.Text = drop2Text;
            drop3Button.Text = drop3Text;
            dropItemCollectorButton.Text = dropItemCollectorText;

            drop1Button.Image = drop1Image;
            drop2Button.Image = drop2Image;
            drop3Button.Image = drop3Image;

            drop1Button.FontSize = drop1Button.Width / drop1Button.Text.Length;
            drop2Button.FontSize = drop2Button.Width / drop2Button.Text.Length;
            drop3Button.FontSize = drop3Button.Width / drop3Button.Text.Length;
            dropItemCollectorButton.FontSize = dropItemCollectorButton.Width / dropItemCollectorButton.Text.Length;
        }

    }
}