using CoreGraphics;
using Foundation;
using UIKit;
using CoreLocation;

namespace iBeaconsEverywhere.iOS
{
	public partial class DetailViewController : UIViewController
	{
		private UIImage blueberry, icy, gray;
		public DetailViewController () : base ("DetailViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Detail", "Detail");

		}

		private CLBeacon beacon;
		/// <summary>
		/// Gets or sets the beacon.
		/// </summary>
		/// <value>The beacon.</value>
		public CLBeacon Beacon 
		{
			get { return beacon; }
			set {
				if (beacon == value)
					return;
				beacon = value;
				ConfigureView ();
			}
		}



		void ConfigureView ()
		{
			// Update the user interface for the beacon
			if (!IsViewLoaded || beacon == null)
				return;

			//Estimated distance between device and beacon
			LabelAccuracy.Text = "Accuracy: " + beacon.Accuracy.ToString("F") + "m";
			LabelMajor.Text = "Major: " + beacon.Major.ToString ();
			LabelMinor.Text = "Minor: " + beacon.Minor.ToString ();
			//The measured signal strength of the Bluetooth packet that led do this iBeacon detection. (decibels)
			LabelRSSI.Text = "RSSI: " + beacon.Rssi.ToString ();
			LabelUUID.Text = "UUID: " + beacon.ProximityUuid.AsString ();

			//switch image for correct beacon color (example only since I know the minors)
			switch (beacon.Minor.ToString ()) {
			case "1":
				ImageViewEstimote.Image = blueberry;
				break;
			case "2":
				ImageViewEstimote.Image = icy;
				break;
			case "3":
				ImageViewEstimote.Image = gray;
				break;
			}

			//update dot location
			var frame = ImageViewDot.Frame;
			switch (beacon.Proximity) {
			case CLProximity.Immediate:
				frame.Y = 60;
				break;
			case CLProximity.Near:
				frame.Y = 90;
				break;
			case CLProximity.Far:
				frame.Y = 150;
				break;
			case CLProximity.Unknown:
				frame.Y = 200;
				break;
			}
			ImageViewDot.Frame = frame;
		}
			

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			EdgesForExtendedLayout = UIRectEdge.None;

			blueberry = UIImage.FromBundle ("Images/beacon_blueberry");
			icy = UIImage.FromBundle ("Images/beacon_icy");
			gray = UIImage.FromBundle ("Images/beacon_gray");
			
			// Perform any additional setup after loading the view, typically from a nib.
			ConfigureView ();
		}
	}
}

