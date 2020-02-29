using Android.App;
using Android.Content;
using Android.OS;

using System.Threading.Tasks;

namespace ReminderXamarin.Droid
{
    [Activity(Theme = "@style/Reminder.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreenActivity : Activity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        private async void SimulateStartup()
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}