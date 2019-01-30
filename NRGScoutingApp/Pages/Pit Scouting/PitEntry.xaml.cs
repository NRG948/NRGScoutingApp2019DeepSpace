using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NRGScoutingApp
{
    public partial class PitEntry : ContentPage
    {
        string titleName = (App.Current.Properties["teamStart"].ToString());
        public string teamName { get { return titleName; } }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        public PitEntry()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            setLabels();
        }

        void setLabels() {
            q1.Text = ConstantVars.QUESTIONS[0];
            q2.Text = ConstantVars.QUESTIONS[1];
            q3.Text = ConstantVars.QUESTIONS[2];
            q4.Text = ConstantVars.QUESTIONS[3];
            q5.Text = ConstantVars.QUESTIONS[4];
            q6.Text = ConstantVars.QUESTIONS[5];
            q7.Text = ConstantVars.QUESTIONS[6];
            q8.Text = ConstantVars.QUESTIONS[7];
            q9.Text = ConstantVars.QUESTIONS[8];
            q10.Text = ConstantVars.QUESTIONS[9];
            q11.Text = ConstantVars.QUESTIONS[10];
        }

        void Comment_Box_Updated(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {

        }

        async void backClicked(object sender, System.EventArgs e)
        {
            var text = await DisplayAlert("Alert", "Do you want to discard progress?", "Yes", "No");
            if (text)
            {
                clearMatchItems();
                if (Matches.appRestore == false)
                {
                    Matches.appRestore = false;
                    await Navigation.PopToRootAsync(true);
                }
                else if (Matches.appRestore == true)
                {
                    Matches.appRestore = false;
                    await Navigation.PopAsync(true);
                }
            }
        }

        //Clears all properties for use in next match
        void clearMatchItems()
        {
            App.Current.Properties["teamStart"] = "";
            App.Current.Properties["appState"] = 0;
            App.Current.Properties["tempPitNotes"] = "";
            App.Current.SavePropertiesAsync();
            // CLEAR OBJECT CONTAINING THE STRINGS: .events.Clear();
        }

        void saveClicked(object sender, System.EventArgs e)
        {

        }
    }
}
