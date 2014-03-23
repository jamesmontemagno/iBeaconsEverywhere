using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace iBeaconsEverywhereAndroid
{
	[Activity (Label = "iBeacon Details")]			
	public class DetailsActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Details);
			var uuid = Intent.GetStringExtra ("uuid");
			//Estimated distance between device and beacon
			var accuracy = Intent.GetStringExtra ("accuracy");
			var major = Intent.GetStringExtra ("major");
			var minor = Intent.GetStringExtra ("minor");
			//The measured signal strength of the Bluetooth packet that led do this iBeacon detection. (decibels)
			var rssi = Intent.GetStringExtra ("rssi");
			var proximity = EstimoteSdk.Utils.Proximity.ValueOf(Intent.GetStringExtra ("proximity"));

			FindViewById<TextView> (Resource.Id.uuid).Text += uuid;
			FindViewById<TextView> (Resource.Id.accuracy).Text += accuracy;
			FindViewById<TextView> (Resource.Id.major).Text += major;
			FindViewById<TextView> (Resource.Id.minor).Text += minor;
			FindViewById<TextView> (Resource.Id.rssi).Text += rssi;



			if(proximity == EstimoteSdk.Utils.Proximity.Immediate)
				FindViewById<ImageView> (Resource.Id.dot_immediate).Visibility = ViewStates.Visible;

			if(proximity == EstimoteSdk.Utils.Proximity.Near)
				FindViewById<ImageView> (Resource.Id.dot_near).Visibility = ViewStates.Visible;

			if(proximity == EstimoteSdk.Utils.Proximity.Far)
				FindViewById<ImageView> (Resource.Id.dot_far).Visibility = ViewStates.Visible;

			if(proximity == EstimoteSdk.Utils.Proximity.Unknown)
				FindViewById<ImageView> (Resource.Id.dot_unknown).Visibility = ViewStates.Visible;
				
			var beaconImage = FindViewById<ImageView> (Resource.Id.image_beacon);
			switch (minor) {
			case "1":
				beaconImage.SetImageResource (Resource.Drawable.beacon_blueberry);
				break;
			case "2":
				beaconImage.SetImageResource (Resource.Drawable.beacon_icy);
				break;
			case "3":
				beaconImage.SetImageResource (Resource.Drawable.beacon_gray);
				break;
			}
		}
	}
}

