// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace iBeaconsSimple.iOS
{
	[Register ("iBeaconsSimple_iOSViewController")]
	partial class iBeaconsSimple_iOSViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LabelBeacon { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (LabelBeacon != null) {
				LabelBeacon.Dispose ();
				LabelBeacon = null;
			}
		}
	}
}
