using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Firebase.Iid;
using Firebase.Messaging;
using ImageCircle.Forms.Plugin.Droid;
using Newtonsoft.Json.Linq;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using ReminderXamarin.Droid.Interfaces.FilePickerService;
using Xamarin.Forms;

namespace ReminderXamarin.Droid
{
    [Activity(Label = "Reminder", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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

            // TODO deal with availability
            IsPlayServicesAvailable();

#if DEBUG
            // Force refresh of the token. If we redeploy the app, no new token will be sent but the old one will
            // be invalid.
            Task.Run(() =>
            {
                // This may not be executed on the main thread.
                FirebaseInstanceId.Instance.DeleteInstanceId();
                Console.WriteLine("Forced token: " + FirebaseInstanceId.Instance.Token);
            });
#endif
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    // In a real project you can give the user a chance to fix the issue.
                    Console.WriteLine($"Error: {GoogleApiAvailability.Instance.GetErrorString(resultCode)}");
                }
                else
                {
                    Console.WriteLine("Error: Play services not supported!");
                    Finish();
                }
                return false;
            }
            else
            {
                Console.WriteLine("Play Services available.");
                return true;
            }
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

    // This service handles the device's registration with FCM.
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        public override async void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Console.WriteLine($"Token received: {refreshedToken}");
            await SendRegistrationToServerAsync(refreshedToken);
        }

        async Task SendRegistrationToServerAsync(string token)
        {
            //try
            //{
            //    // Formats: https://firebase.google.com/docs/cloud-messaging/concept-options
            //    // The "notification" format will automatically displayed in the notification center if the 
            //    // app is not in the foreground.
            //    const string templateBodyFCM =
            //        "{" +
            //        "\"notification\" : {" +
            //        "\"body\" : \"$(messageParam)\"," +
            //        "\"title\" : \"Xamarin University\"," +
            //        "\"icon\" : \"myicon\" }" +
            //        "}";

            //    var templates = new JObject();
            //    templates["genericMessage"] = new JObject
            //    {
            //        {"body", templateBodyFCM}
            //    };

            //    var client = new MobileServiceClient(App.MobileServiceUrl);
            //    var push = client.GetPush();

            //    await push.RegisterAsync(token, templates);

            //    // Push object contains installation ID afterwards.
            //    Console.WriteLine(push.InstallationId.ToString());
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    Debugger.Break();
            //}
        }
    }

    // This service is used if app is in the foreground and a message is received.
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            Console.WriteLine("Received: " + message);

            // Android supports different message payloads. To use the code below it must be something like this (you can paste this into Azure test send window):
            // {
            //   "notification" : {
            //      "body" : "The body",
            //                 "title" : "The title",
            //                 "icon" : "myicon
            //   }
            // }
            try
            {
                var msg = message.GetNotification().Body;
                MessagingCenter.Send<object, string>(this, App.NotificationReceivedKey, msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting message: " + ex);
            }
        }
    }
}

