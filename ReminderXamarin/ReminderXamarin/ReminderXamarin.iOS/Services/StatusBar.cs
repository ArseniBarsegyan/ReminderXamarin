using Foundation;
using ReminderXamarin.Services;
using UIKit;

namespace ReminderXamarin.iOS.Services
{
    public class StatusBar : IStatusBar
    {
        [Preserve]
        public StatusBar()
        {
        }
        
        public int Height => (int)UIApplication.SharedApplication.StatusBarFrame.Height;
    }
}