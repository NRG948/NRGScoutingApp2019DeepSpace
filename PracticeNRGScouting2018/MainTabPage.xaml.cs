using Xamarin.Forms;
using BottomBar.XamarinForms;

namespace PracticeNRGScouting2018
{
    public partial class MainPageTab : BottomBarPage
    {
        public MainPageTab()
        {
            Children.Add(new Matches());
            Children.Add(new Rankings());
            InitializeComponent();
        }
    }
}