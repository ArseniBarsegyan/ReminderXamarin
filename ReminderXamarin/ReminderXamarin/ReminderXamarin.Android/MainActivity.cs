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
using ReminderXamarin.Droid.Services.FilePickerService;
using Xamarin.Forms;

namespace ReminderXamarin.Droid
{
    [Activity(Label = "Reminder", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        /// <summary>
        /// Publishes results from returning activities started for some result. Required for Platform class
        /// </summary>
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
            LoadApplication(new App());

            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);

            var intent = new Intent(ApplicationContext, typeof(NotificationService));
            StartService(intent);
        }

        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }

        /// <summary>
        /// Required for Platform.cs class.
        /// </summary>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            ActivityResult?.Invoke(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

