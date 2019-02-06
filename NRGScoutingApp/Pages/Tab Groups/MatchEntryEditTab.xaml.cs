using System;
using System.Collections.Generic;
using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using System.Threading.Tasks;

namespace NRGScoutingApp
{
    public partial class MatchEntryEditTab : Xamarin.Forms.TabbedPage
    {
        public MatchEntryEditTab()
        {
            Children.Add(new NewMatchStart());
            Children.Add(new MatchEvents());
            if (checkParse()){
                Children.Add(new MatchParameters());
            }
            else{
                Children.Add(new MatchParameters());
            }
            BindingContext = this;
            App.Current.Properties["newAppear"] = 1;
            App.Current.SavePropertiesAsync();
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        ArrayList vals = new ArrayList();

        private Boolean checkParse(){
            if (!App.Current.Properties.ContainsKey(App.Current.Properties["teamStart"].ToString() + "converted")) { 
                return false; 
            }
            else{
                ParametersFormat s = new ParametersFormat();
                return true;
            }
        }

        async void backClicked(object sender, System.EventArgs e)
        {
            var text = await DisplayAlert("Alert", "Do you want to discard progress?", "Yes", "No");
            if (text)
            {
                MatchParameters.clearMatchItems();
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
    }
}