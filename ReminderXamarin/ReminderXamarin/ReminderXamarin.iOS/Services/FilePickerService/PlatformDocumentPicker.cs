using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using MobileCoreServices;
using Photos;
using ReminderXamarin.Services.FilePickerService;
using UIKit;

namespace ReminderXamarin.iOS.Services.FilePickerService
{
    public class PlatformDocumentPicker : IPlatformDocumentPicker
    {
        private static readonly string[] textUTTypes =
            { UTType.Text, UTType.PlainText };
        
        private static readonly string[] imageUTTypes =
        { UTType.Image, UTType.PNG, UTType.TIFF, UTType.JPEG, UTType.JPEG2000 };

        [Preserve]
        public PlatformDocumentPicker()
        {
        }
        
        #region IPlatformDocumentPicker implementation

        public Task<PlatformDocument> DisplayImageImportAsync()
        {
            return DisplayImportAsync(imageUTTypes);
        }

        public Task<PlatformDocument> DisplayTextImportAsync()
        {
            return DisplayImportAsync(textUTTypes);
        }

        #endregion

        private Task<PlatformDocument> DisplayImportAsync(string[] utTypes)
        {
            var taskCompletionSource = new TaskCompletionSource<PlatformDocument>();
            var docPicker = new UIDocumentPickerViewController(utTypes, UIDocumentPickerMode.Import);
            docPicker.DidPickDocument += (sender, e) =>
            {
                CompleteTaskUsing(taskCompletionSource, e.Url);
            };
            docPicker.DidPickDocumentAtUrls += (sender, e) =>
            {
                CompleteTaskUsing(taskCompletionSource, e?.Urls[0]);
            };
            docPicker.WasCancelled += (sender, e) =>
            {
                taskCompletionSource.SetResult(null);
            };
            
            var window= UIApplication.SharedApplication.KeyWindow;
            var rootViewController = window.RootViewController;

            rootViewController?.PresentViewController(docPicker, true, null);
            var presentationPopover = docPicker.PopoverPresentationController;
            presentationPopover.SourceView = rootViewController.View;
            presentationPopover.PermittedArrowDirections = 0;
            presentationPopover.SourceRect = rootViewController.View.Frame;

            return taskCompletionSource.Task;
        }

        private void CompleteTaskUsing(
            TaskCompletionSource<PlatformDocument> taskCompletionSource,
            UIImagePickerMediaPickedEventArgs args)
        {
            if (args.MediaUrl != null)
            {
                CompleteTaskUsing(taskCompletionSource, args);
                return;
            }
            try
            {
                string imageName = null;
                {
                    var url = args.ReferenceUrl;
                    if (url != null)
                    {
                        var assets = PHAsset.FetchAssets(new NSUrl[] { url }, null);
                        if (assets.Count >= 1)
                        {
                            var asset = assets.firstObject as PHAsset;
                            var dateFormatter = new NSDateFormatter
                            {
                                DateFormat = "yyyy-MM-dd HH:mm:ss",
                            };
                            imageName = dateFormatter.ToString(asset.CreationDate);
                        }
                    }
                }
                imageName = imageName ?? DateTime.UtcNow.ToString();

                string path;
                using (var data = args.OriginalImage.AsJPEG())
                {
                    path = WriteToTemporaryFile(data);
                }
                taskCompletionSource.SetResult(new PlatformDocument(imageName + ".jpg", path));
            }
            catch (Exception e)
            {
                taskCompletionSource.SetException(e);
            }
        }

        private void CompleteTaskUsing(TaskCompletionSource<PlatformDocument> taskCompletionSource, NSUrl url)
        {
            try
            {
                string path;
                using (var data = NSData.FromFile(url.Path, NSDataReadingOptions.Mapped, out NSError error))
                {
                    if (error == null)
                    {
                        path = WriteToTemporaryFile(data);
                    }
                    else
                    {
                        taskCompletionSource.SetException(new IOException(error.LocalizedDescription));
                        return;
                    }
                }
                taskCompletionSource.SetResult(new PlatformDocument(url.LastPathComponent, path));
            }
            catch (Exception e)
            {
                taskCompletionSource.SetException(e);
            }
        }

        private string WriteToTemporaryFile(NSData data)
        {
            var path = Path.GetTempFileName();
            using (var stream = File.OpenWrite(path))
            {
                data.AsStream()
                    .CopyTo(stream);
            }
            return path;
        }
    }
}