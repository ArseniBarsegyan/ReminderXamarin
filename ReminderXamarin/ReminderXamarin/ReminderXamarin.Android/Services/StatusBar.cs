using Android.App;
using Plugin.CurrentActivity;
using ReminderXamarin.Services;

namespace ReminderXamarin.Droid.Services
{
    public class StatusBar : IStatusBar
    {
        [Xamarin.Forms.Internals.Preserve]
        public StatusBar()
        {
        }
        
        private Activity Activity => CrossCurrentActivity.Current.Activity;
        
        public int Height
        {
            get
            {
                int statusBarHeight = -1;
                int resourceId = Activity.Resources.GetIdentifier("status_bar_height", "dimen", "android");
                if (resourceId > 0)
                {
                    statusBarHeight = Activity.Resources.GetDimensionPixelSize(resourceId);
                }
                return statusBarHeight;
            }
        }
    }
}