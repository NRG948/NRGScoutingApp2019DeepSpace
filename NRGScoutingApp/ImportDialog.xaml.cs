using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.Security.Cryptography.X509Certificates;
//using Security;
using System.Globalization;

namespace NRGScoutingApp
{
    public partial class ImportDialog
    {
        public ImportDialog()
        {
            BindingContext = this;
            InitializeComponent();
        }

        void cancelClicked(object sender, System.EventArgs e)
        {
                PopupNavigation.Instance.PopAsync(true);
        }
        void importClicked(object sender, System.EventArgs e)
        {
            if(!importData.Text.Contains("|") || !importData.Text.Contains("<") || !importData.Text.Contains(">") || !importData.Text.Contains("cube") || !importData.Text.Contains(":"))
            {
                DisplayAlert("Error", "Match String not valid", "OK");
            }
            DisplayAlert("Imported", importData.Text, "OK");
            App.Current.Properties["matchEventsString"] = App.Current.Properties["matchEventsString"].ToString() + importData.Text;
            App.Current.SavePropertiesAsync();
            PopupNavigation.Instance.PopAsync(true);
           
        }

    }
}


