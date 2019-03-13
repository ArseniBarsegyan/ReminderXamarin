using System.IO;
using ReminderXamarin.iOS.Services;
using ReminderXamarin.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileSystem))]
namespace ReminderXamarin.iOS.Services
{
    /// <summary>
    /// Implementation of <see cref="IFileSystem"/> for iOS.
    /// </summary>
    public class FileSystem : IFileSystem
    {
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}