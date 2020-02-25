using System.IO;

using ReminderXamarin.Services;

namespace ReminderXamarin.iOS.Services
{
    public class FileSystem : IFileSystem
    {
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}