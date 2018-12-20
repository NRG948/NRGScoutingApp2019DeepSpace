using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Button), typeof(NRGScoutingApp.iOS.CustomButtonIos))]
namespace NRGScoutingApp.iOS{
    public class CustomButtonIos : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
                Control.TitleLabel.LineBreakMode = UIKit.UILineBreakMode.WordWrap;
            Control.TitleLabel.TextAlignment = UIKit.UITextAlignment.Center;
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

