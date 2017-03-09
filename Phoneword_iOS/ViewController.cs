using System;

using UIKit;

using Foundation;

namespace Phoneword_iOS
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			string translatedNumber = "";
			TranslateButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				translatedNumber = PhoneTranslator.ToNumber(PhoneNumberText.Text);
				PhoneNumberText.ResignFirstResponder();

				if (translatedNumber == "")
				{
					CallButton.SetTitle("Call ", UIControlState.Normal);
					CallButton.Enabled = false;
				}
				else
				{
					CallButton.SetTitle("Call " + translatedNumber, UIControlState.Normal);
					CallButton.Enabled = true; 
				}
			};

			CallButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				var url = new NSUrl("tel:" + translatedNumber);
				//...otherwise show an alert dialog. 
				if (!UIApplication.SharedApplication.OpenUrl(url))
				{
					var alert = UIAlertController.Create("Not supported", "Scheme 'tel:' is not supported on this device", UIAlertControllerStyle.Alert);
					alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
					PresentViewController(alert, true, null);
				}

			};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
