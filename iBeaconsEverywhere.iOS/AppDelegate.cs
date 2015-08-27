using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace iBeaconsEverywhere.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UINavigationController navigationController;
		UIWindow window;
		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			//Turn phone into beacon
			//var controller = new BeaconController ();
			//Detect Beacons
			var controller = new MasterViewController ();
			navigationController = new UINavigationController (controller);
			window.RootViewController = navigationController;

			// make the window visible
			window.MakeKeyAndVisible ();

			//Required in iOS 8 to show notifications
			UIApplication.SharedApplication.RegisterUserNotificationSettings (
				UIUserNotificationSettings.GetSettingsForTypes (UIUserNotificationType.Alert, null));

			
			return true;
		}
	}
}

