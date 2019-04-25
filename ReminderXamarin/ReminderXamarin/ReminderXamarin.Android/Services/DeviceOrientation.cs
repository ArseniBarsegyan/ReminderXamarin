using Android.Content;
using Android.Runtime;
using Android.Views;
using ReminderXamarin.Droid.Services;
using ReminderXamarin.Services;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceOrientation))]
namespace ReminderXamarin.Droid.Services
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of <see cref="IDeviceOrientation"/> for Android.
    /// </summary>
    public class DeviceOrientation : IDeviceOrientation
    {
        public DeviceOrientations GetOrientation()
        {
            IWindowManager windowManager = Android.App.Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            var rotation = windowManager.DefaultDisplay.Rotation;
            bool isLandscape = rotation == SurfaceOrientation.Rotation90 || rotation == SurfaceOrientation.Rotation270;
            return isLandscape ? DeviceOrientations.Landscape : DeviceOrientations.Portrait;
        }
    }
}