using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Clipboard;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;

namespace NRGScoutingApp {
    public partial class ExportDialog {
        public ExportDialog () {
            InitializeComponent ();
            setExportEntries ();
        }
        void cancelClicked (object sender, System.EventArgs e) {
            PopupNavigation.Instance.PopAsync (true);
        }
        void copyClicked (object sender, System.EventArgs e) {
            CrossClipboard.Current.SetText (exportDisplay.Text);
            PopupNavigation.Instance.PopAsync (true);
        }

        //Gets entries from device storage and sets them into the text field (or disables field if empty)
        void setExportEntries () {
            JObject x;
            try {
                x = (JObject) JsonConvert.DeserializeObject (Preferences.Get ("matchEventsString", ""));
            } catch (JsonException) {
                x = new JObject ();
            }
            if (x.Count > 0) {
                String exportEntries = JsonConvert.SerializeObject (
                    JObject.Parse (Preferences.Get ("matchEventsString", "")), Formatting.None);
                exportDisplay.Text = exportEntries;
            } else {
                copyButton.IsEnabled = false;
                exportDisplay.Text = "No Entries Yet!";
            }
        }
    }
}