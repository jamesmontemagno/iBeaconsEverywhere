using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.CoreBluetooth;
using MonoTouch.CoreFoundation;

namespace iBeaconsEverywhere.iOS
{
	public partial class BeaconController : UIViewController
	{
		public BeaconController () : base ("BeaconController", null)
		{
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

		CBPeripheralManager peripheralMgr;
		BTPeripheralDelegate peripheralDelegate;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			beaconUUID = new NSUuid (uuid);
			beaconRegion = new CLBeaconRegion (beaconUUID, beaconMajor, beaconMinor, beaconId);



			//power - the received signal strength indicator (RSSI) value (measured in decibels) of the beacon from one meter away
			var power = new NSNumber (-59);

			NSMutableDictionary peripheralData = beaconRegion.GetPeripheralData (power);
			peripheralDelegate = new BTPeripheralDelegate ();
			peripheralMgr = new CBPeripheralManager (peripheralDelegate, DispatchQueue.DefaultGlobalQueue);

			peripheralMgr.StartAdvertising (peripheralData);
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

