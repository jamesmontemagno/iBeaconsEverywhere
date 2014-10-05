using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.Wearable.Views;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using EstimoteSdk;

namespace iBeaconsEverywhere.Wear
{
	[Activity (Label = "iBeaconsEverywhere.Wear", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, BeaconManager.IServiceReadyCallback
	{

		private EstimoteSdk.Region beaconRegion;
		private BeaconManager beaconManager;



		const string beaconId ="com.refractored";
		const string uuid = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";
		private bool beaconsEnabled = true;
		View background;
		TextView count;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//create a new beacon manager to handle starting and stopping ranging
			beaconManager = new BeaconManager (this);


			//major and minor are optional, pass in null if you don't need them
			beaconRegion = new EstimoteSdk.Region (beaconId, uuid, null, null);

			var v = FindViewById<WatchViewStub> (Resource.Id.watch_view_stub);

			v.LayoutInflated += delegate {

				// Get our button from the layout resource,
				// and attach an event to it
				background = FindViewById<View> (Resource.Id.main);
				count = FindViewById<TextView>(Resource.Id.text);
			};

			beaconManager.Ranging += (object sender, BeaconManager.RangingEventArgs e) => RunOnUiThread (() => {


				background.SetBackgroundColor(Color.Black);
				count.Text = e.Beacons.Count.ToString();
				if(e.Beacons.Count == 0)
					return;

				var prox = Utils.ComputeProximity(e.Beacons[0]);
				if(prox == Utils.Proximity.Far)
					background.SetBackgroundColor(Color.Blue);
				else if(prox == Utils.Proximity.Near)
					background.SetBackgroundColor(Color.Yellow);
				else if(prox == Utils.Proximity.Immediate)
					background.SetBackgroundColor(Color.Green);
				else
					background.SetBackgroundColor(Color.Black);

			});
		}

		public void OnServiceReady()
		{
			// This method is called when BeaconManager is up and running.

			beaconManager.StartRanging (beaconRegion);

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
	}
}



