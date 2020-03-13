using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ReminderXamarin.Droid
{
    [Activity(Theme = "@style/Reminder.Splash", 
        MainLauncher = true, 
        NoHistory = true, 
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
        }
    }
}