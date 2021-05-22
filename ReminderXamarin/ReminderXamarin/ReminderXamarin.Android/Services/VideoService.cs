using Android.Content;
using Android.Net;
using Android.Support.V4.Content;

using Java.IO;

using Plugin.CurrentActivity;

using ReminderXamarin.Core.Interfaces;

namespace ReminderXamarin.Droid.Services
{
    public class VideoService : IVideoService
    {
        [Xamarin.Forms.Internals.Preserve]
        public VideoService()
        {
        }
        
        public void PlayVideo(string path)
        {
            File videoFile = new File(path);
            Uri fileUri = FileProvider.GetUriForFile(CrossCurrentActivity.Current.AppContext,
                "com.arsbars.Reminder.fileprovider", videoFile);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(fileUri, "video/*");
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            if (CrossCurrentActivity.Current.Activity is MainActivity mainActivity)
            {
                mainActivity.StartActivity(intent);
            }
        }
    }
}