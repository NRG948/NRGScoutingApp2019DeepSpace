using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using Plugin.Clipboard;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace NRGScoutingApp
{
    public partial class ExportDialog
    {
        public ExportDialog()
        {
            BindingContext = this;
            InitializeComponent();
            setExportEntries();
        }


        //public string ExportDisplay {
           // get {
           //     return exportEntries;
           //} }
        void cancelClicked(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
        void copyClicked(object sender, System.EventArgs e)
        {
            CrossClipboard.Current.SetText(exportDisplay.Text);
            PopupNavigation.Instance.PopAsync(true);
        }
        void setExportEntries()
        {
            if(!String.IsNullOrWhiteSpace(App.Current.Properties["matchEventsString"].ToString())) // || (JObject) App.Current.Properties["matchEventsString"].Count > 0)
            {
                String exportEntries = JsonConvert.SerializeObject((JObject)App.Current.Properties["matchEventsString"], Formatting.None);
                exportDisplay.Text = exportEntries;
            }
            else
            {
                exportDisplay.Text = "";
            }
        }
    }
}


