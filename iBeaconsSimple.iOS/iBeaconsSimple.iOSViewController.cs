using System;
using CoreGraphics;

using Foundation;
using UIKit;
using CoreLocation;
using System.Linq;

namespace iBeaconsSimple.iOS
{
	public partial class iBeaconsSimple_iOSViewController : UIViewController
	{

		public iBeaconsSimple_iOSViewController (IntPtr handle) : base (handle)
		{
		}
			

		#region View lifecycle
		CLLocationManager locationmanager;
		NSUuid beaconUUID;
		CLBeaconRegion beaconRegion;

		const string beaconId ="com.xamarin";
		const string uuid = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";

		const ushort beaconMajor = 107;
		const ushort beaconMinor = 8;
	
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			locationmanager = new CLLocationManager ();
      if(UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
        locationmanager.RequestAlwaysAuthorization ();

			
      beaconUUID = new NSUuid (uuid);
      beaconRegion = new CLBeaconRegion (beaconUUID, 107, 8, beaconId);
      beaconRegion.NotifyEntryStateOnDisplay = true;
      beaconRegion.NotifyOnEntry = true;
      beaconRegion.NotifyOnExit = true;

      locationmanager.RegionEntered += (sender, e) => 
      {
        var notification = new UILocalNotification () { AlertBody = "The Xamarin beacon is close by!" };
        UIApplication.SharedApplication.CancelAllLocalNotifications();
        UIApplication.SharedApplication.PresentLocalNotificationNow (notification);

      };


      locationmanager.RegionLeft += (sender, e) => 
      {
        var notification = new UILocalNotification () { AlertBody = "The Xamarin beacon is far!" };
        UIApplication.SharedApplication.CancelAllLocalNotifications();
        UIApplication.SharedApplication.PresentLocalNotificationNow (notification);
      };

			//DidRangeBeacons
      locationmanager.DidRangeBeacons += (object sender, CLRegionBeaconsRangedEventArgs e) => 
      {

        if(e.Beacons.Length == 0)
          return;
        
        var beacon = e.Beacons[0];

        LabelBeacon.Text = "We found: " + e.Beacons.Length + " beacons" +  " " + beacon.Major + " " + beacon.Minor;
        LabelDistance.Text = beacon.Accuracy.ToString("##.000") + " " + beacon.Proximity.ToString();

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

          //locationmanager.StopRangingBeacons(beaconRegion);

          //var vc = UIStoryboard.FromName("MainStoryboard", null).InstantiateViewController("FoundViewController");
          //NavigationController.PushViewController(vc, true);
          break;
        case CLProximity.Unknown:
          return;
        }
      };






			//StartRanging
      locationmanager.StartRangingBeacons(beaconRegion);
      locationmanager.StartMonitoring (beaconRegion);
		}



		/* 
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

				LabelBeacon.Text = beacon.Major + " " + beacon.Minor;
				LabelDistance.Text = beacon.Accuracy.ToString();
		 */
		 


		//var vc = UIStoryboard.FromName("MainStoryboard", null).InstantiateViewController("found");
		//NavigationController.PushViewController(vc, true);

		/*
		  locationmanager.RegionEntered += (object sender, CLRegionEventArgs e) => {

					var notification = new UILocalNotification () { AlertBody = "The Xamarin beacon is close by!" };
					UIApplication.SharedApplication.CancelAllLocalNotifications();
					UIApplication.SharedApplication.PresentLocationNotificationNow (notification);
			};
		 */


		#endregion
	}
}

