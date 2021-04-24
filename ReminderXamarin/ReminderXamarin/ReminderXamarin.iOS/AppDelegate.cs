using System;
using System.Linq;

using Foundation;

using ImageCircle.Forms.Plugin.iOS;

using Plugin.LocalNotifications;

using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Enums;
using ReminderXamarin.IoC;
using ReminderXamarin.iOS.Services;
using ReminderXamarin.iOS.Services.FilePickerService;
using ReminderXamarin.iOS.Services.MediaPicker;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.Services.MediaPicker;

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

            Forms.Init();
            SQLitePCL.Batteries.Init();
            ImageCircleRenderer.Init();
            Bootstrapper.Initialize();
            RegisterPlatformServices();
            LoadApplication(new App());

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(
                    UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                    (approved, error) => { });
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

        private void RegisterPlatformServices()
        {
            ComponentRegistry.Register<IPlatformDocumentPicker, PlatformDocumentPicker>();
            ComponentRegistry.Register<IMultiMediaPickerService, MultiMediaPickerService>();
            ComponentRegistry.Register<IDeviceOrientation, DeviceOrientation>();
            ComponentRegistry.Register<IFileHelper, Services.FileHelper>();
            ComponentRegistry.Register<IFileSystem, FileSystem>();
            ComponentRegistry.Register<IImageService, ImageService>();
            ComponentRegistry.Register<IMediaService, MediaService>();
            ComponentRegistry.Register<IPermissionService, PermissionService>();
            ComponentRegistry.Register<IVideoService, VideoService>();
            ComponentRegistry.Register<IThemeService, ThemeService>();
        }
    }
}
