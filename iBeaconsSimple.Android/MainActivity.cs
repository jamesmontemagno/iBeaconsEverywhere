
using Android.App;
using Android.Widget;
using Android.OS;
using EstimoteSdk;
using Android.Graphics;

namespace iBeaconsSimple.Android
{
	[Activity (Label = "iBeaconsSimple.Android", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : Activity, BeaconManager.IServiceReadyCallback
	{

		private EstimoteSdk.Region beaconRegion;
		private BeaconManager beaconManager;

		const string beaconId = "com.refractored";
		const string uuid = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";
		private bool beaconsEnabled = true;
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//create a new beacon manager to handle starting and stopping ranging
			beaconManager = new BeaconManager (this);


			//major and minor are optional, pass in null if you don't need them
			beaconRegion = new EstimoteSdk.Region (beaconId, uuid, null, null);

			var background = FindViewById<LinearLayout> (Resource.Id.background);
			var beaconsFound = FindViewById<TextView> (Resource.Id.beacons_found);
			var accuracy = FindViewById<TextView> (Resource.Id.accuracy);

			beaconManager.Ranging += (object sender, BeaconManager.RangingEventArgs e) => RunOnUiThread (() => {


				background.SetBackgroundColor (Color.White);
				beaconsFound.Text = e.Beacons.Count.ToString ();
				if (e.Beacons.Count == 0)
					return;

				var prox = Utils.ComputeProximity (e.Beacons [0]);

				if (prox == Utils.Proximity.Far)
					background.SetBackgroundColor (Color.Blue);
				else if (prox == Utils.Proximity.Near)
					background.SetBackgroundColor (Color.Yellow);
				else if (prox == Utils.Proximity.Immediate)
					background.SetBackgroundColor (Color.Green);
				else
					background.SetBackgroundColor (Color.Black);

				var distance = Utils.ComputeAccuracy (e.Beacons [0]);
				accuracy.Text = distance.ToString ("##.0000000");

			});
		}

		public void OnServiceReady ()
		{
			// This method is called when BeaconManager is up and running.

			beaconManager.StartRanging (beaconRegion);

		}

		protected override void OnDestroy ()
		{
			// Make sure we disconnect from the Estimote.
			base.OnDestroy ();

			beaconManager.Disconnect ();
		}

		protected override void OnPause ()
		{
			// Make sure we disconnect on pause.
			base.OnPause ();
			beaconManager.Disconnect ();
		}

		protected override void OnResume ()
		{
			//on resume and come back reconnect to manager
			base.OnResume ();
			if (beaconsEnabled)
				beaconManager.Connect (this);
		}
	}
}


