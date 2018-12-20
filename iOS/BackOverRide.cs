using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MatchEntryEditTab), typeof(NRGScoutingApp.iOS.MyAdvantagePageRenderer))]
namespace NRGScoutingApp.iOS.Renderers
{
    public class MyAdvantagePageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (((MatchEntryEditTab)Element).EnableBackButtonOverride)
            {
                SetCustomBackButton();
            }
        }
        private void SetCustomBackButton()
        {
            UIButton btn = new UIButton();
            btn.Frame = new CGRect(0, 0, 50, 40);
            btn.BackgroundColor = UIColor.Clear;

            btn.TouchDown += (sender, e) =>
            {
                // Whatever your custom back button click handling
                if (((MatchEntryEditTab)Element)?.
                CustomBackButtonAction != null)
                {
                    ((MatchEntryEditTab)Element)?.
                       CustomBackButtonAction.Invoke();
                }
            };
            NavigationController.NavigationBar.AddSubview(btn);
        }
    }
}

//namespace NRGScoutingApp.iOS
//{
//    public partial class ButtonWrapText : UIViewController
//    {
//        public ButtonWrapText() : base("ButtonWrapText", null)
//        {
//        }

//        public override void ViewDidLoad()
//        {
//            base.ViewDidLoad();
//            // Perform any additional setup after loading the view, typically from a nib.
//        }

//        public override void DidReceiveMemoryWarning()
//        {
//            base.DidReceiveMemoryWarning();
//            // Release any cached data, images, etc that aren't in use.
//        }
//    }
//}

