namespace ReminderXamarin.Core.Interfaces
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
        string GetVideoSavingPath(string videoName);
    }
}