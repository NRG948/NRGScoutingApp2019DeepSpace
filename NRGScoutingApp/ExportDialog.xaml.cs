using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using Plugin.Clipboard;


namespace NRGScoutingApp
{
    public partial class ExportDialog
    {
        public ExportDialog()
        {
            BindingContext = this;
            InitializeComponent();
        }
        String exportEntries = App.Current.Properties["matchEventsString"].ToString(); //TODO: Add Data From Entries to this TextField

        public string ExportDisplay { get { return exportEntries; } }
        void cancelClicked(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
        void copyClicked(object sender, System.EventArgs e)
        {
            CrossClipboard.Current.SetText(exportDisplay.Text);
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}


