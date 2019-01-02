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
            InitializeComponent();
            setExportEntries();
        }
       void cancelClicked(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
        void copyClicked(object sender, System.EventArgs e)
        {
            CrossClipboard.Current.SetText(exportDisplay.Text);
            PopupNavigation.Instance.PopAsync(true);
        }

        //Gets entries from device storage and sets them into the text field (or disables field if empty)
        void setExportEntries()
        {
            if(!String.IsNullOrWhiteSpace(App.Current.Properties["matchEventsString"].ToString()))
            {
                String exportEntries = JsonConvert.SerializeObject(
                JObject.Parse(App.Current.Properties["matchEventsString"].ToString()),Formatting.None);
                exportDisplay.Text = exportEntries;
            }
            else
            {
                copyButton.IsEnabled = false;
                exportDisplay.Text = "No Entries Yet!";
            }
        }
    }
}


