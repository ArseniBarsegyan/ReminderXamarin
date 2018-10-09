using System;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using ImageCircle.Forms.Plugin.Droid;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using ReminderXamarin.Droid.Interfaces.FilePickerService;

namespace ReminderXamarin.Droid
{
    [Activity(Label = "Reminder", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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

            global::Xamarin.Forms.Forms.Init(this, bundle);
            var cv = typeof(Xamarin.Forms.CarouselView);
            var assembly = Assembly.Load(cv.FullName);
            CrossCurrentActivity.Current.Init(this, bundle);
            Platform.Init(this);
            CrossCurrentActivity.Current.Activity = this;
            ImageCircleRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);
            LoadApplication(new App());
        }

        /// <summary>
        /// Required for Platform.cs class.
        /// </summary>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            ActivityResult?.Invoke(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

