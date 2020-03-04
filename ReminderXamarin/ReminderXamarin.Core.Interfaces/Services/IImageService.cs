namespace ReminderXamarin.Core.Interfaces
{
    public interface IImageService
    {
        void ResizeImage(string sourceFile, string targetFile, int requiredWidth, int requiredHeight);
    }
}