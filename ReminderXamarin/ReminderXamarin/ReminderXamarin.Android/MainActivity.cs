using System;

using Acr.UserDialogs;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

using ImageCircle.Forms.Plugin.Droid;

using Plugin.CurrentActivity;
using Plugin.Permissions;

using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Droid.Services;
using ReminderXamarin.Droid.Services.FilePickerService;
using ReminderXamarin.Droid.Services.MediaPicker;
using ReminderXamarin.IoC;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.Services.MediaPicker;

using Xamarin.Forms;

using Platform = ReminderXamarin.Droid.Services.FilePickerService.Platform;

namespace ReminderXamarin.Droid
{
    [Activity(Label = "Reminder", Icon = "@mipmap/icon", Theme = "@style/MainTheme", 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public event Action<int, Result, Intent> ActivityResult;
        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            Instance = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            Forms.SetFlags("CollectionView_Experimental");
            Forms.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            Platform.Init(this);
            CrossCurrentActivity.Current.Activity = this;
            ImageCircleRenderer.Init();
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, bundle);

            Bootstrapper.Initialize();
            RegisterPlatformServices();
            LoadApplication(new App(MultiMediaPickerService.SharedInstance));
            
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);

            var intent = new Intent(ApplicationContext, typeof(NotificationService));
            StartService(intent);
        }

        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            ActivityResult?.Invoke(requestCode, resultCode, data);
            MultiMediaPickerService.SharedInstance.OnActivityResult(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {            
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void RegisterPlatformServices()
        {
            ComponentRegistry.Register<IPlatformDocumentPicker, PlatformDocumentPicker>();
            ComponentRegistry.Register<IMultiMediaPickerService, MultiMediaPickerService>();
            ComponentRegistry.Register<IAlertService, AlertService>();
            ComponentRegistry.Register<IDeviceOrientation, DeviceOrientation>();
            ComponentRegistry.Register<IFileHelper, Services.FileHelper>();
            ComponentRegistry.Register<IFileSystem, FileSystem>();
            ComponentRegistry.Register<IImageService, ImageService>();
            ComponentRegistry.Register<ILoadingService, LoadingService>();
            ComponentRegistry.Register<IMediaService, MediaService>();
            ComponentRegistry.Register<IPermissionService, PermissionService>();
            ComponentRegistry.Register<IVideoService, VideoService>();
            ComponentRegistry.Register<IThemeService, ThemeService>();
        }
    }
}

