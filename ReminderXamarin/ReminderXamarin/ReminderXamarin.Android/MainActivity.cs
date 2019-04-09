﻿using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Firebase.Iid;
using ImageCircle.Forms.Plugin.Droid;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using ReminderXamarin.Droid.Interfaces.FilePickerService;

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
            global::Xamarin.Forms.Forms.Init(this, bundle);
            //var cv = typeof(Xamarin.Forms.CarouselView);
            //var assembly = Assembly.Load(cv.FullName);
            CrossCurrentActivity.Current.Init(this, bundle);
            Platform.Init(this);
            CrossCurrentActivity.Current.Activity = this;
            ImageCircleRenderer.Init();
            UserDialogs.Init(this);
            LoadApplication(new App());

            // TODO deal with availability
            IsPlayServicesAvailable();

#if DEBUG
            // Force refresh of the token. If we redeploy the app, no new token will be sent but the old one will
            // be invalid.
            Task.Run(() =>
            {
                // This may not be executed on the main thread.
                FirebaseInstanceId.Instance.DeleteInstanceId();
                System.Diagnostics.Debug.WriteLine("Forced token: " + FirebaseInstanceId.Instance.Token);
            });
#endif

            var intent = new Intent(ApplicationContext, typeof(NotificationService));
            StartService(intent);
        }

        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    // In a real project you can give the user a chance to fix the issue.
                    System.Diagnostics.Debug.WriteLine($"Error: {GoogleApiAvailability.Instance.GetErrorString(resultCode)}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Error: Play services not supported!");
                    Finish();
                }
                return false;
            }
            System.Diagnostics.Debug.WriteLine("Play Services available.");
            return true;
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

