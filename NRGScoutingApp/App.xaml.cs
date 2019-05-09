using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace NRGScoutingApp {
    public partial class App : Application {
        public static bool UseMockDataStore = true;
        public static string BackendUrl = "https://localhost:5000";

        public App () {
            InitializeComponent ();
            MainPage = new NavigationPage (new NavTab ());
            Application.Current.MainPage = new NavigationPage (new NavTab ());
            if (UseMockDataStore)
                DependencyService.Register<MockDataStore> ();
            else
                DependencyService.Register<CloudDataStore> ();
        }
    }
}