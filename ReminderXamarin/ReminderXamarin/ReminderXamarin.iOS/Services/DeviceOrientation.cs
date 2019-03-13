using ReminderXamarin.iOS.Services;
using ReminderXamarin.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceOrientation))]
namespace ReminderXamarin.iOS.Services
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of <see cref="IDeviceOrientation"/> for iOS.
    /// </summary>
    public class DeviceOrientation : IDeviceOrientation
    {
        public DeviceOrientations GetOrientation()
        {
            var currentOrientation = UIApplication.SharedApplication.StatusBarOrientation;
            bool isPortrait = currentOrientation == UIInterfaceOrientation.Portrait
                              || currentOrientation == UIInterfaceOrientation.PortraitUpsideDown;

            return isPortrait ? DeviceOrientations.Portrait : DeviceOrientations.Landscape;
        }
    }
}