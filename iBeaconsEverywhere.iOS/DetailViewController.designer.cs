// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace iBeaconsEverywhere.iOS
{
	[Register ("DetailViewController")]
	partial class DetailViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView ImageViewDot { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView ImageViewEstimote { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelAccuracy { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelMajor { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelMinor { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRSSI { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelUUID { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ImageViewDot != null) {
				ImageViewDot.Dispose ();
				ImageViewDot = null;
			}

			if (LabelUUID != null) {
				LabelUUID.Dispose ();
				LabelUUID = null;
			}

			if (LabelMajor != null) {
				LabelMajor.Dispose ();
				LabelMajor = null;
			}

			if (LabelMinor != null) {
				LabelMinor.Dispose ();
				LabelMinor = null;
			}

			if (LabelAccuracy != null) {
				LabelAccuracy.Dispose ();
				LabelAccuracy = null;
			}

			if (LabelRSSI != null) {
				LabelRSSI.Dispose ();
				LabelRSSI = null;
			}

			if (ImageViewEstimote != null) {
				ImageViewEstimote.Dispose ();
				ImageViewEstimote = null;
			}
		}
	}
}
