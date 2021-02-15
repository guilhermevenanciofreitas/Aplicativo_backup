using Xamarin.Forms;
using Foundation;
using UIKit;
using Aplicativo.Binding;

namespace Skclusive.Dashboard.Host.IPhone
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            Forms.Init();

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            // For iOS, wrap inside a navigation page, otherwise the header looks wrong
            var formsApp = new AppStartup();
            formsApp.MainPage = new NavigationPage(formsApp.MainPage);

            LoadApplication(formsApp);

            return base.FinishedLaunching(app, options);

        }
    }
}