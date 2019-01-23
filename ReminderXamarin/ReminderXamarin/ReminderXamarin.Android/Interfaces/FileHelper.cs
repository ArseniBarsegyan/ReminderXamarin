using System;
using System.IO;
using Plugin.CurrentActivity;
using ReminderXamarin.Droid.Interfaces;
using ReminderXamarin.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace ReminderXamarin.Droid.Interfaces
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
            var baseDirectory = CrossCurrentActivity.Current.Activity.GetExternalFilesDir(null);
            string videoPath = baseDirectory + @"/Movies/Videos/";
            return Path.Combine(videoPath, videoName);
        }
    }
}