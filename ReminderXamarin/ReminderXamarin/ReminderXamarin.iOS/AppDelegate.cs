using System;
using System.Linq;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using Plugin.LocalNotifications;
using Rm.Helpers;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace ReminderXamarin.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
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
            Rg.Plugins.Popup.Popup.Init();

            global::Xamarin.Forms.Forms.Init();
            SQLitePCL.Batteries.Init();
            //var cv = typeof(Xamarin.Forms.CarouselView);
            //var assembly = Assembly.Load(cv.FullName);
            ImageCircleRenderer.Init();
            LoadApplication(new App());

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Ask the user for permission to get notifications on iOS 10.0+
                UNUserNotificationCenter.Current.RequestAuthorization(
                    UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                    (approved, error) => { });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // Ask the user for permission to get notifications on iOS 8.0+
                var settings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(5), delegate { CheckNotifications(); });
            return base.FinishedLaunching(app, options);
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(5), delegate { CheckNotifications(); });
        }

        public void CheckNotifications()
        {
            var currentDate = DateTime.Now;

            var allToDoModels = App.ToDoRepository.GetAll()
                .Where(x => x.WhenHappens.ToString("dd.MM.yyyy HH:mm") == currentDate.ToString("dd.MM.yyyy HH:mm"))
                .ToList();

            allToDoModels.ForEach(model =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CrossLocalNotifications.Current.Show(model.WhenHappens.ToString("D"), model.Description);
                });
                App.ToDoRepository.DeleteModel(model);
                MessagingCenter.Send((App)App.Current, ConstantsHelper.UpdateUI);
            });
        }
    }
}
