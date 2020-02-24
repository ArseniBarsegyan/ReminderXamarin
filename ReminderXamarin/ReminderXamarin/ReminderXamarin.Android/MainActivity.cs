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
using ReminderXamarin.Droid.Services.MediaPicker;
using Xamarin.Forms;
using Platform = ReminderXamarin.Droid.Services.FilePickerService.Platform;

namespace ReminderXamarin.Droid
{
    [Activity(Label = "Reminder", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public event Action<int, Result, Intent> ActivityResult;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init(this, bundle);
            //var cv = typeof(Xamarin.Forms.CarouselView);
            //var assembly = Assembly.Load(cv.FullName);
            CrossCurrentActivity.Current.Init(this, bundle);
            Platform.Init(this);
            CrossCurrentActivity.Current.Activity = this;
            ImageCircleRenderer.Init();
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, bundle);
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
    }
}

