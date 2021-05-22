using System.IO;
using Foundation;
using ReminderXamarin.Core.Interfaces;

namespace ReminderXamarin.iOS.Services
{
    public class FileSystem : IFileSystem
    {
        [Preserve]
        public FileSystem()
        {
        }
        
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}