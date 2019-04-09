using System;
using System.Collections.Generic;
using System.Reflection;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using Plugin.LocalNotifications;
using ReminderXamarin.ViewModels;
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
        private NSTimer _timer;
        private readonly List<KeyValuePair<DateTime, string>> _toDoList = new List<KeyValuePair<DateTime, string>>();

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

            MessagingCenter.Subscribe<ToDoViewModel, KeyValuePair<DateTime, string>>(this, "ToDoCreated",
                (model, pair) =>
                {
                    _toDoList.Add(pair);
                });
            _timer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(5), delegate { SendNotification(); });
            return base.FinishedLaunching(app, options);
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            _timer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(5), delegate { SendNotification(); });
        }

        public void SendNotification()
        {
            var currentDate = DateTime.Now;
            for (int i = 0; i < _toDoList.Count; i++)
            {
                var pair = _toDoList[i];
                var dateString = currentDate.ToString("dd.MM.yyyy HH:mm");
                var pairDateString = pair.Key.ToString("dd.MM.yyyy HH:mm");
                if (dateString == pairDateString)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CrossLocalNotifications.Current.Show(pair.Key.ToString("D"), pair.Value);
                    });
                    _toDoList.Remove(pair);
                }
            }
        }
    }
}
