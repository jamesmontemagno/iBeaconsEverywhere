using System;
using CoreGraphics;
using Foundation;
using UIKit;
using CoreLocation;
using CoreBluetooth;
using CoreFoundation;

namespace iBeaconsEverywhere.iOS
{
	public partial class BeaconController : UIViewController
	{
		BTPeripheralDelegate peripheralDelegate;
		CBPeripheralManager peripheralManager;
		public BeaconController () : base ("BeaconController", null)
		{
			peripheralDelegate = new BTPeripheralDelegate ();
			peripheralManager = new CBPeripheralManager (peripheralDelegate, DispatchQueue.DefaultGlobalQueue);

		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		NSUuid beaconUUID;
		CLBeaconRegion beaconRegion;
		const ushort beaconMajor = 2755;
		const ushort beaconMinor = 5;
		const string beaconId ="com.refractored";
		const string uuid = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			var locationManager = new CLLocationManager ();
			locationManager.RequestWhenInUseAuthorization ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);


			beaconUUID = new NSUuid (uuid);
			beaconRegion = new CLBeaconRegion (beaconUUID, beaconMajor, beaconMinor, beaconId);



			//power - the received signal strength indicator (RSSI) value (measured in decibels) of the beacon from one meter away
			var power = new NSNumber (-59);

			var peripheralData = beaconRegion.GetPeripheralData (power);
			peripheralDelegate = new BTPeripheralDelegate ();
			peripheralManager.StartAdvertising (peripheralData);
		}

		class BTPeripheralDelegate : CBPeripheralManagerDelegate
		{
			public override void StateUpdated (CBPeripheralManager peripheral)
			{
				if (peripheral.State == CBPeripheralManagerState.PoweredOn) {
					Console.WriteLine ("powered on");
				}
			}
		}


	}
}

