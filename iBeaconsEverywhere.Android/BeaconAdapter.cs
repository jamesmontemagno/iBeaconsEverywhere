using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Views;
using Android.Widget;
using EstimoteSdk;

namespace iBeaconsEverywhereAndroid
{
	internal class BeaconAdapterWrapper : Java.Lang.Object
	{
		public TextView Title { get; set; }
		public TextView SubTitle { get; set; }
		public ImageView Image { get; set; }
	}

	class BeaconAdapter : BaseAdapter
	{
		private readonly Activity m_Context;
		public List<Beacon> Beacons {get;set;}

		public BeaconAdapter(Activity context)
		{
			m_Context = context;
			this.Beacons = new List<Beacon>();
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			if (position < 0)
				return null;

			var view = (convertView
				?? m_Context.LayoutInflater.Inflate(
					Resource.Layout.ItemBeacon, parent, false)
			);

			if (view == null)
				return null;

			var wrapper = view.Tag as BeaconAdapterWrapper;
			if (wrapper == null)
			{
				wrapper = new BeaconAdapterWrapper
				{
					Title = view.FindViewById<TextView>(Resource.Id.title),
					SubTitle = view.FindViewById<TextView>(Resource.Id.subtitle),
					Image = view.FindViewById<ImageView>(Resource.Id.image)
				};
				view.Tag = wrapper;
			}

			var beacon = this.Beacons[position];

			var message = string.Empty;
			var proximity = Utils.ComputeProximity (beacon);

			if (proximity == Utils.Proximity.Immediate) {
				message = "Immediate";
				wrapper.Image.SetImageResource (Resource.Drawable.ic_square_immediate);
			} else if (proximity == Utils.Proximity.Near) {

				message = "Near";
				wrapper.Image.SetImageResource (Resource.Drawable.ic_square_near);
			} else if (proximity == Utils.Proximity.Far) {

				message = "Far";
				wrapper.Image.SetImageResource (Resource.Drawable.ic_square_far);
			} else if (proximity == Utils.Proximity.Unknown) {

				message = "Unknown";
				wrapper.Image.SetImageResource (Resource.Drawable.ic_square_unknown);
			}


			wrapper.Title.Text = beacon.Name + " M: " + beacon.Major + "m: " + beacon.Minor;
			wrapper.SubTitle.Text = message + " " + beacon.ProximityUUID;
			return view;
		}

		public override int Count
		{
			get { return this.Beacons.Count; }
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return position;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override bool HasStableIds
		{
			get
			{
				return true;
			}
		}
	}
}

