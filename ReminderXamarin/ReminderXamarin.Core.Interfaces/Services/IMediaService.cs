namespace ReminderXamarin.Core.Interfaces
{
    public interface IMediaService
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
        byte[] GenerateThumbImage(string url, long second);
    }
}