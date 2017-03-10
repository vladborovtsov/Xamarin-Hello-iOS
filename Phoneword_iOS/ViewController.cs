using System;

using UIKit;

using Foundation;

using System.Collections.Generic; 


namespace Phoneword_iOS
{
	public partial class ViewController : UIViewController
	{
		public List<string> PhoneNumbers { get; set; }
		string translatedNumber = "";

		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
			this.PhoneNumbers = new List<string>();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.


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
				PhoneNumbers.Add(translatedNumber);

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

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			// set the View Controller that’s powering the screen we’re
			// transitioning to
			var callHistoryController = segue.DestinationViewController as CallHistoryController;

			//set the Table View Controller’s list of phone numbers to the
			// list of dialed phone numbers
			if (callHistoryController != null)
			{
				callHistoryController.PhoneNumbers = PhoneNumbers;
			}

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
