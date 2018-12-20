using Xamarin.Forms;

namespace NRGScoutingApp
{
	public class NavTabCS : TabbedPage
	{
		public NavTabCS ()
		{
			var navigationPage = new NavigationPage (new WelcomePage ());
			//.Icon = "schedule.png";
			navigationPage.Title = "New Entry";

            Children.Add (new MatchEntryStart ());
			Children.Add (navigationPage);
			Children.Add (new Rankings ());
		}
	}
}
