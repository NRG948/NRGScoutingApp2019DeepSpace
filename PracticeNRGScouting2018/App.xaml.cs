using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PracticeNRGScouting2018
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage =new NavigationPage( new Matches());
        }

        protected override void OnStart()
        {
            Console.WriteLine("Starting");// Handle when your app starts
        }

        protected override void OnSleep()
        {
            Console.WriteLine("Sleeping");// Handle when your app sleeps
        }

        protected override void OnResume()
        {
            Console.WriteLine("Resuming");// Handle when your app resumes
        }
    }
}
