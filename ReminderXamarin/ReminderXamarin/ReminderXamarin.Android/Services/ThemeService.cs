using ReminderXamarin.Services;

using Xamarin.Forms.Platform.Android;

namespace ReminderXamarin.Droid.Services
{
    public class ThemeService : IThemeService
    {
        [Xamarin.Forms.Internals.Preserve]
        public ThemeService()
        {
        }
        
        public void SetStatusBarColor(Xamarin.Forms.Color color)
        {
            MainActivity.Instance.SetStatusBarColor(color.ToAndroid());
        }
    }
}