using System;
using System.IO;
using ReminderXamarin.iOS.Services;
using ReminderXamarin.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace ReminderXamarin.iOS.Services
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }

        public string GetVideoSavingPath(string videoName)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, videoName);
        }
    }
}