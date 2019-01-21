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
            InitializeComponent();
            setButtons();
        }

        //DROP_TYPE Specified in MatchFormat Class

        void drop1(object sender, System.EventArgs e)
        {
            NewMatchStart.events.Add(new MatchFormat.Data { time = NewMatchStart.droppedTime, type = (int)MatchFormat.ACTION.drop1 });
            setDropTime();
            saveEvents();
            Navigation.PopAsync(true);
        }
        void drop2(object sender, System.EventArgs e)
        {
            NewMatchStart.events.Add(new MatchFormat.Data { time = NewMatchStart.droppedTime, type = (int)MatchFormat.ACTION.drop2 });
            setDropTime();
            saveEvents();
            Navigation.PopAsync(true);
        }
        void noneClicked(object sender, System.EventArgs e)
        {
            NewMatchStart.events.Add(new MatchFormat.Data { time = NewMatchStart.droppedTime, type = (int)MatchFormat.ACTION.dropNone });
            saveEvents();
            setDropTime();
            Navigation.PopAsync(true);
        }
        void drop3(object sender, System.EventArgs e)
        {
            NewMatchStart.events.Add(new MatchFormat.Data { time = NewMatchStart.droppedTime, type = (int)MatchFormat.ACTION.drop3 });
            saveEvents();
            setDropTime();
            Navigation.PopAsync(true);
        }
        void drop4(object sender, System.EventArgs e)
        {
            NewMatchStart.events.Add(new MatchFormat.Data { time = NewMatchStart.droppedTime, type = (int)MatchFormat.ACTION.drop4 });
            saveEvents();
            setDropTime();
            Navigation.PopAsync(true);
        }
        void backClicked(object sender, System.EventArgs e)
        {
            Button cubePicked = (Button)sender;
            NewMatchStart.cubeSetDrop = true;
            Navigation.PopAsync(true);

        }
        public static void saveEvents()
        {
            App.Current.Properties["tempMatchEvents"] = NewMatchStart.events;
            App.Current.SavePropertiesAsync();
        }
        private void setDropTime()
        {
            App.Current.Properties["lastItemDropped"] = (int)NewMatchStart.droppedTime;
            App.Current.SavePropertiesAsync();
        }
        private void setButtons()
        {
            drop1Button.Text = ConstantVars.DROP_1_DIALOG_TEXT;
            drop2Button.Text = ConstantVars.DROP_2_DIALOG_TEXT;
            drop3Button.Text = ConstantVars.DROP_3_DIALOG_TEXT;
            drop4Button.Text = ConstantVars.DROP_4_DIALOG_TEXT;

            drop1Button.Image = ConstantVars.DROP_1_DIALOG_IMAGE;
            drop2Button.Image = ConstantVars.DROP_2_DIALOG_IMAGE;
            drop3Button.Image = ConstantVars.DROP_3_DIALOG_IMAGE;
            drop4Button.Image = ConstantVars.DROP_4_DIALOG_IMAGE;

            drop1Button.FontSize = drop1Button.Width / drop1Button.Text.Length;
            drop2Button.FontSize = drop2Button.Width / drop2Button.Text.Length;
            drop3Button.FontSize = drop3Button.Width / drop3Button.Text.Length;
            drop4Button.FontSize = drop4Button.Width / drop4Button.Text.Length;
        }

    }
}