using System;
using System.IO;

using Foundation;

using QuickLook;

using ReminderXamarin.Services;

using UIKit;

using Xamarin.Forms;

namespace ReminderXamarin.iOS.Services
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
        private readonly string _fileName;
        private readonly string _filePath;

        public QLPreviewItemFileSystem(string fileName, string filePath)
        {
            _fileName = fileName;
            _filePath = filePath;
        }

        public override string ItemTitle => _fileName;

        public override NSUrl ItemUrl => NSUrl.FromFilename(_filePath);
    }
}