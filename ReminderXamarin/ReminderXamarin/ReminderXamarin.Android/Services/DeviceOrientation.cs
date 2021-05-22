using Android.Content;
using Android.Runtime;
using Android.Views;

using ReminderXamarin.Core.Interfaces;

namespace ReminderXamarin.Droid.Services
{
    public class DeviceOrientation : IDeviceOrientation
    {
        [Xamarin.Forms.Internals.Preserve]
        public DeviceOrientation()
        {
        }
        
        public DeviceOrientations GetOrientation()
        {
            IWindowManager windowManager = Android.App.Application
                .Context
                .GetSystemService(Context.WindowService)
                .JavaCast<IWindowManager>();

            var rotation = windowManager.DefaultDisplay.Rotation;

            bool isLandscape = rotation == SurfaceOrientation.Rotation90 
                || rotation == SurfaceOrientation.Rotation270;

            return isLandscape ? DeviceOrientations.Landscape 
                : DeviceOrientations.Portrait;
        }
    }
}