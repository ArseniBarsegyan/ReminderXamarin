namespace ReminderXamarin.Core.Interfaces
{
    public interface IDeviceOrientation
    {
        DeviceOrientations GetOrientation();
    }

    public enum DeviceOrientations
    {
        Undefined,
        Landscape,
        Portrait
    }
}