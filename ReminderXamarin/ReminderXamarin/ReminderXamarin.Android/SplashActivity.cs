using System.Threading.Tasks;
using Android.App;
using Android.Content;

namespace ReminderXamarin.Droid
{
    [Activity(Theme = "@style/Reminder.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(SimulateStartup);
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        private async void SimulateStartup()
        {
            await Task.Delay(500); // Simulate a bit of startup work.
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            OverridePendingTransition(Resource.Animation.slide_in_from_middle, Resource.Animation.slide_out_from_middle);
        }
    }
}