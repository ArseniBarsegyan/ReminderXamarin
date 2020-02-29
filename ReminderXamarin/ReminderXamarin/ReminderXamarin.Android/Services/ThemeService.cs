using ReminderXamarin.Services;

using Xamarin.Forms.Platform.Android;

namespace ReminderXamarin.Droid.Services
{
    public class ThemeService : IThemeService
    {
        public void SetStatusBarColor(Xamarin.Forms.Color color)
        {
            MainActivity.Instance.SetStatusBarColor(color.ToAndroid());
        }
    }
}