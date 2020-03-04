namespace ReminderXamarin.Core.Interfaces
{
    public interface IFileSystem
    {
        byte[] ReadAllBytes(string path);
    }
}