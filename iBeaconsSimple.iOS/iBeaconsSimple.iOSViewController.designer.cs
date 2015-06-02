// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iBeaconsSimple.iOS
{
	[Register ("iBeaconsSimple_iOSViewController")]
	partial class iBeaconsSimple_iOSViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LabelBeacon { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LabelDistance { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (LabelBeacon != null) {
				LabelBeacon.Dispose ();
				LabelBeacon = null;
			}
			if (LabelDistance != null) {
				LabelDistance.Dispose ();
				LabelDistance = null;
			}
		}
	}
}
