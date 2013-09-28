using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MapkitDirections
{
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        UIWindow window;
        UINavigationController navigationController;
        UIViewController viewController;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // create a new window instance based on the screen size
            window = new UIWindow(UIScreen.MainScreen.Bounds);

            viewController = new MyViewController();

            navigationController = new UINavigationController();
            navigationController.PushViewController(viewController, false);

            // If you have defined a view, add it here:
            window.AddSubview(navigationController.View);

            // make the window visible
            window.MakeKeyAndVisible();

            return true;
        }
    }
}

