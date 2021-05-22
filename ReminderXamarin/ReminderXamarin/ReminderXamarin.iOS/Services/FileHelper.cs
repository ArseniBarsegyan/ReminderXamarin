using System;
using System.IO;
using Foundation;
using ReminderXamarin.Core.Interfaces;

namespace ReminderXamarin.iOS.Services
{
    public class FileHelper : IFileHelper
    {
        [Preserve]
        public FileHelper()
        {
        }
        
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