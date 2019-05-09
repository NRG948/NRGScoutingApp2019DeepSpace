using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using NRGScouting;

namespace NRGScoutingApp.Droid {
    [Activity (Label = "NRGScoutingApp.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        App app;
        protected override void OnCreate (Bundle bundle) {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate (bundle);
            Plugin.CloudFirestore.CloudFirestore.Init(this);
            Xamarin.Essentials.Platform.Init (this, bundle);
            Rg.Plugins.Popup.Popup.Init (this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init (this, bundle);
            Plugin.FirebaseAuth.FirebaseAuth.Init(this);
            global::Xamarin.Forms.Forms.Init (this, bundle);

            app = new App ();

            LoadApplication (app);
        }

        protected override void OnActivityResult (int requestCode, Result resultCode, Intent data) {
            base.OnActivityResult (requestCode, resultCode, data);
        }
    }
}