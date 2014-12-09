using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;

namespace iBeaconsSimple.iOS
{
	public partial class iBeaconsSimple_iOSViewController : UIViewController
	{

		public iBeaconsSimple_iOSViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle
		CLLocationManager locationmanager;
		NSUuid beaconUUID;
		CLBeaconRegion beaconRegion;
		const ushort beaconMajor = 62646;
		const ushort beaconMinor = 64521;
		const string beaconId ="com.xamarin";
		const string uuid = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";
	
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			locationmanager = new CLLocationManager ();
			locationmanager.RequestWhenInUseAuthorization ();

			beaconUUID = new NSUuid (uuid);
			beaconRegion = new CLBeaconRegion (beaconUUID, beaconMajor, beaconMinor, beaconId);

			locationmanager.DidRangeBeacons += (sender, e) => 
			{
				if(e.Beacons.Length == 0)
					return;

				var beacon = e.Beacons[0];
				switch(beacon.Proximity)
				{
				case CLProximity.Far:
					View.BackgroundColor = UIColor.Blue;
					break;
				case CLProximity.Near:
					View.BackgroundColor = UIColor.Yellow;
					break;
				case CLProximity.Immediate:
					View.BackgroundColor = UIColor.Green;
					break;
				case CLProximity.Unknown:
					return;
				}

				LabelBeacon.Text = beacon.Accuracy.ToString();
			};

			locationmanager.StartRangingBeacons (beaconRegion);

			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

