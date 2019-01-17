using System;
using System.IO;
using Foundation;
using QuickLook;
using ReminderXamarin.iOS.Interfaces;
using ReminderXamarin.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(VideoService))]
namespace ReminderXamarin.iOS.Interfaces
{
    public class VideoService : IVideoService
    {
        public void PlayVideo(string path)
        {
            string name = Path.GetFileName(path);
            Device.BeginInvokeOnMainThread(() =>
            {
                QLPreviewItemFileSystem prevItem = new QLPreviewItemFileSystem(name, path);
                QLPreviewController previewController = new QLPreviewController();
                previewController.DataSource = new PreviewControllerDS(prevItem);
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(previewController, true, null);
            });
        }
    }

    public class PreviewControllerDS : QLPreviewControllerDataSource
    {
        private readonly QLPreviewItem _item;

        public PreviewControllerDS(QLPreviewItem item)
        {
            _item = item;
        }

        public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
        {
            return _item;
        }

        public override nint PreviewItemCount(QLPreviewController controller)
        {
            return 1;
        }
    }

    public class QLPreviewItemFileSystem : QLPreviewItem
    {
        string _fileName, _filePath;

        public QLPreviewItemFileSystem(string fileName, string filePath)
        {
            _fileName = fileName;
            _filePath = filePath;
        }

        public override string ItemTitle
        {
            get
            {
                return _fileName;
            }
        }
        public override NSUrl ItemUrl
        {
            get
            {
                return NSUrl.FromFilename(_filePath);
            }
        }

    }
}