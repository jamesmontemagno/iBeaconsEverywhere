using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using EstimoteSdk;
using Android.Content;

namespace iBeaconsEverywhereAndroid
{
	[Activity (Label = "iBeacons Everywhere", MainLauncher = true)]
	public class MainActivity : ListActivity, BeaconManager.IServiceReadyCallback
	{
		//UI stuff
		private ProgressBar progressBar;
		private TextView noBeacons;

		private Region beaconRegion;
		private BeaconManager beaconManager;


		private readonly Java.Lang.Integer major = new Java.Lang.Integer(2755);
		private readonly Java.Lang.Integer minor = new Java.Lang.Integer(1);

		const string beaconId ="com.refractored";
		const string uuid = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";
		private bool beaconsEnabled = true;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);
			progressBar = FindViewById<ProgressBar> (Resource.Id.progressBar);
			noBeacons = FindViewById<TextView> (Resource.Id.no_beacons);

			ListAdapter = new BeaconAdapter (this);

			//create a new beacon manager to handle starting and stopping ranging
			beaconManager = new BeaconManager (this);

			//manually check for BLE
			var ble = Android.Content.PM.PackageManager.FeatureBluetoothLe;
			if (PackageManager.HasSystemFeature(ble)) {
				Toast.MakeText(this, "BLE not supported", ToastLength.Short).Show();
			}

			//Validation checks
			if (!beaconManager.HasBluetooth) {
				//no bluetooth :(
				DisplayMessage ("No bluetooth on your device.", ":(");
				beaconsEnabled = false;
			} else if (!beaconManager.IsBluetoothEnabled) {
				//bluetooth is not enabled
				DisplayMessage ("Please turn on bluetooth.", ":(");
				beaconsEnabled = false;
			} else if (!beaconManager.CheckPermissionsAndService ()) {
				//issues with permissions and service
				DisplayMessage ("Issues with service and persmissions.", ":(");
				beaconsEnabled = false;
			}

			//major and minor are optional, pass in null if you don't need them
			beaconRegion = new Region (beaconId, uuid, null, null);


			//Event for when ranging happens
			beaconManager.Ranging += (object sender, BeaconManager.RangingEventArgs e) => RunOnUiThread (() => {

				if (e.Beacons.Count == 0)
					noBeacons.Visibility = ViewStates.Visible;
				else if(noBeacons.Visibility == ViewStates.Visible)
					noBeacons.Visibility = ViewStates.Gone;

				if (progressBar.Visibility == ViewStates.Visible)
					progressBar.Visibility = ViewStates.Invisible;

				((BeaconAdapter)ListAdapter).Beacons.Clear ();
				((BeaconAdapter)ListAdapter).Beacons.AddRange (e.Beacons);
				((BeaconAdapter)ListAdapter).NotifyDataSetChanged ();

			});


			//estimote loggin, optional
			#if DEBUG

            Estimote.EnableDebugLogging (true);
			#endif


		}
			
		public void OnServiceReady()
		{
			// This method is called when BeaconManager is up and running.

			beaconManager.StartRanging (beaconRegion);

			RunOnUiThread (() => progressBar.Visibility = ViewStates.Visible);
		}

		protected override void OnDestroy()
		{
			// Make sure we disconnect from the Estimote.
			base.OnDestroy();

			beaconManager.Disconnect();
		}

		protected override void OnPause ()
		{
			// Make sure we disconnect on pause.
			base.OnPause ();
			beaconManager.Disconnect ();
		}

		protected override void OnResume()
		{
			//on resume and come back reconnect to manager
			base.OnResume();
			if(beaconsEnabled)
				beaconManager.Connect(this);
		}

		protected override void OnListItemClick (ListView l, View v, int position, long id)
		{
			base.OnListItemClick (l, v, position, id);
			var beacon = ((BeaconAdapter)ListAdapter).Beacons [position];

			//on click navigate to details page. This means on pause will be called
			Intent intent = new Intent (this, typeof(DetailsActivity));
			intent.PutExtra ("uuid", beacon.ProximityUUID);
			intent.PutExtra ("rssi", beacon.Rssi.ToString());
			intent.PutExtra ("accuracy", Utils.ComputeAccuracy (beacon).ToString ("F") + "m");
			intent.PutExtra ("major", beacon.Major.ToString ());
			intent.PutExtra ("minor", beacon.Minor.ToString ());
			intent.PutExtra ("proximity", Utils.ComputeProximity(beacon).ToString());
			StartActivity (intent);

		}

		#region entered region

		/*beaconManager.EnteredRegion += (object sender, BeaconManager.EnteredRegionEventArgs e) => {

			if(e.Region.Identifier != beaconId)
				return;

			DisplayMessage("You just entered a new region: " + e.Region.Major + "." + e.Region.Minor, "You win!");

		};
		//In Service Ready
		beaconManager.StartMonitoring(beaconRegion);*/
		#endregion

		private void DisplayMessage(string message, string title)
		{
			RunOnUiThread (() => {
				var builder = new AlertDialog.Builder (this);
				builder.SetMessage (message)
				.SetTitle (title ?? string.Empty)
				.SetPositiveButton ("OK", delegate {
				});
				var dialog = builder.Create ();
				dialog.Show ();
			});
		}

	}
}


