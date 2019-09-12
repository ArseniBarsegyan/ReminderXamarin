using System;
using System.Linq;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using Plugin.LocalNotifications;
using ReminderXamarin.Enums;
using ReminderXamarin.iOS.Services.MediaPicker;
using Rm.Helpers;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace ReminderXamarin.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();
            Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            SQLitePCL.Batteries.Init();
            //var cv = typeof(Xamarin.Forms.CarouselView);
            //var assembly = Assembly.Load(cv.FullName);
            ImageCircleRenderer.Init();
            LoadApplication(new App(new MultiMediaPickerService()));

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

            App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
            App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;

            return base.FinishedLaunching(app, options);
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(5), delegate { CheckNotifications(); });
        }

        public void CheckNotifications()
        {
            var currentDate = DateTime.Now;

            var allToDoModels = App.ToDoRepository.Value.GetAll()
                .Where(x => x.Status == ConstantsHelper.Active)
                .Where(x => x.WhenHappens.ToString("dd.MM.yyyy HH:mm") == currentDate.ToString("dd.MM.yyyy HH:mm"))
                .ToList();

            allToDoModels.ForEach(model =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CrossLocalNotifications.Current.Show(model.WhenHappens.ToString("D"), model.Description);
                });
                model.Status = ToDoStatus.Completed.ToString();
                App.ToDoRepository.Value.Save(model);
                MessagingCenter.Send((App)App.Current, ConstantsHelper.UpdateUI);
            });
        }
    }
}
