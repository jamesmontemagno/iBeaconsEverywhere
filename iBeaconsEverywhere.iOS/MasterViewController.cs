using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;

namespace iBeaconsEverywhere.iOS
{
	public partial class MasterViewController : UITableViewController
	{
		DataSource dataSource;

		public MasterViewController () : base ("MasterViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Master", "iBeacons Everywhere");

		}

		public DetailViewController DetailViewController {
			get;
			set;
		}


		CLLocationManager locationmanager;
		NSUuid beaconUUID;
		CLBeaconRegion beaconRegion;
		const ushort beaconMajor = 2755;
		const ushort beaconMinor = 1;
		const string beaconId ="com.refractored";
		const string uuid = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";
		public UIImage Near, Far, Immediate, Unknown;

		CLBeaconRegion beaconRegionNotifications;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Near = UIImage.FromBundle ("Images/square_near");
			Far = UIImage.FromBundle ("Images/square_far");
			Immediate = UIImage.FromBundle ("Images/square_immediate");
			Unknown = UIImage.FromBundle ("Images/square_unknown");

			beaconUUID = new NSUuid (uuid);
			beaconRegion = new CLBeaconRegion (beaconUUID, beaconMajor, beaconId);


			beaconRegion.NotifyEntryStateOnDisplay = true;
			beaconRegion.NotifyOnEntry = true;
			beaconRegion.NotifyOnExit = true;

			locationmanager = new CLLocationManager ();

			locationmanager.RegionEntered += (object sender, CLRegionEventArgs e) => {
				if (e.Region.Identifier == beaconId) {

					var notification = new UILocalNotification () { AlertBody = "The Xamarin beacon is close by!" };
					UIApplication.SharedApplication.CancelAllLocalNotifications();
					UIApplication.SharedApplication.PresentLocationNotificationNow (notification);
				}
			};


			locationmanager.DidRangeBeacons += (object sender, CLRegionBeaconsRangedEventArgs e) => {
				if (e.Beacons.Length > 0) {

					CLBeacon beacon = e.Beacons [0];
					//this.Title = beacon.Proximity.ToString() + " " +beacon.Major + "." + beacon.Minor;
				}
					
				dataSource.Beacons = e.Beacons;
				TableView.ReloadData();
			};







			locationmanager.StartMonitoring (beaconRegion);
			locationmanager.StartRangingBeacons (beaconRegion);



			TableView.Source = dataSource = new DataSource (this);
		}


	
		class DataSource : UITableViewSource
		{
			static readonly NSString CellIdentifier = new NSString ("Cell");
			CLBeacon[] beacons = new CLBeacon[]{};
			readonly MasterViewController controller;

			public DataSource (MasterViewController controller)
			{
				this.controller = controller;
			}

			public CLBeacon[] Beacons {
				get { return beacons; }
				set { beacons = value;}
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return beacons.Length;
			}

			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (CellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, CellIdentifier);
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				}

				var beacon = beacons [indexPath.Row];


				string message = string.Empty;

				switch (beacon.Proximity) {
				case CLProximity.Immediate:
					message = "Immediate";
					cell.ImageView.Image = controller.Immediate;
					break;
				case CLProximity.Near:
					message = "Near";
					cell.ImageView.Image = controller.Near;
					break;
				case CLProximity.Far:
					message = "Far";
					cell.ImageView.Image = controller.Far;
					break;
				case CLProximity.Unknown:
					message = "?";
					cell.ImageView.Image = controller.Unknown;
					break;
				}

				cell.TextLabel.Text = "M: " + beacon.Major + " m:" + beacon.Minor;

				cell.DetailTextLabel.Text = message + " " + beacon.ProximityUuid.AsString ();

				return cell;
			}

		
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				if (controller.DetailViewController == null)
					controller.DetailViewController = new DetailViewController ();

				controller.DetailViewController.Beacon =  (beacons [indexPath.Row]);

				// Pass the selected object to the new view controller.
				controller.NavigationController.PushViewController (controller.DetailViewController, true);
			}
		}

		#region Notified
		/*
			beaconRegion.NotifyEntryStateOnDisplay = true;
			beaconRegion.NotifyOnEntry = true;
			beaconRegion.NotifyOnExit = true;
			locationmanager.RegionEntered += (object sender, CLRegionEventArgs e) => {
				if (e.Region.Identifier == beaconId) {

					var notification = new UILocalNotification () { AlertBody = "The Xamarin beacon is close by!" };
					UIApplication.SharedApplication.CancelAllLocalNotifications();
					UIApplication.SharedApplication.PresentLocationNotificationNow (notification);
				}
			};
		locationmanager.StartMonitoring (beaconRegion);

		CLBeaconRegion beaconRegionNotifications;
		*/
		#endregion


		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			Unknown.Dispose ();
			Unknown = null;
			Near.Dispose ();
			Near = null;
			Far.Dispose ();
			Far = null;
			Immediate.Dispose ();
			Immediate = null;
		}
	}
}
