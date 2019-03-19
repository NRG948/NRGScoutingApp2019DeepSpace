using System;
using NRGScoutingApp.Droid;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


namespace NRGScoutingApp.Droid
{
    [Activity(Label = "NRGScoutingApp.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        App app;
        protected override void OnCreate(Bundle bundle)
        {
           // TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;


            base.OnCreate(bundle);

            Xamarin.Essentials.Platform.Init(this, bundle);
            Rg.Plugins.Popup.Popup.Init(this, bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

             app = new App();

            LoadApplication(app);
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }


        /*
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var app = App.Current;
            if (item.ItemId == 16908332) 
            {
                var navPage = ((app.MainPage.Navigation.ModalStack[0] as MasterDetailPage).Detail as NavigationPage);

                if (app != null && navPage.Navigation.NavigationStack.Count > 0)
                {
                    int index = navPage.Navigation.NavigationStack.Count - 1;

                    var currentPage = navPage.Navigation.NavigationStack[index];

                    var vm = currentPage.BindingContext as ViewModel;

                    if (vm != null)
                    {
                        var answer = vm.OnBackButtonPressed();
                        if (answer)
                            return true;
                    }

                }
            }

            return base.OnOptionsItemSelected(item);
        }*/

    }

}
