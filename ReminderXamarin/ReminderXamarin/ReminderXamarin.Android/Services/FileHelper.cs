﻿using System;
using System.IO;

using Plugin.CurrentActivity;

using ReminderXamarin.Core.Interfaces;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Droid.Services
{
    public class FileHelper : IFileHelper
    {
        [Preserve]
        public FileHelper()
        {
        }
        
        public string GetLocalFilePath(string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
        }

        public string GetVideoSavingPath(string videoName)
        {
            var baseDirectory = CrossCurrentActivity.Current.Activity.GetExternalFilesDir(null);
            string videoPath = baseDirectory + @"/Movies/Videos/";
            return Path.Combine(videoPath, videoName);
        }
    }
}