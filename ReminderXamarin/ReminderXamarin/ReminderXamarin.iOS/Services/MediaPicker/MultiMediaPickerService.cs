using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AVFoundation;

using Foundation;

using GMImagePicker;

using Photos;

using ReminderXamarin.Services.MediaPicker;

using UIKit;

namespace ReminderXamarin.iOS.Services.MediaPicker
{
    public class MultiMediaPickerService : IMultiMediaPickerService
    {
        private const string TemporalDirectoryName = "TmpMedia";

        private GMImagePickerController _currentPicker;
        private TaskCompletionSource<IList<MediaFile>> _mediaPickTcs;

        [Preserve]
        public MultiMediaPickerService()
        {
        }

        //Events
        public event EventHandler<MediaFile> OnMediaPicked;
        public event EventHandler<IList<MediaFile>> OnMediaPickedCompleted;

        public void Clean()
        {
            var documentsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), 
                TemporalDirectoryName);

            if (Directory.Exists(documentsDirectory))
            {
                Directory.Delete(documentsDirectory);
            }
        }

        public async Task<IList<MediaFile>> PickPhotosAsync()
        {
            return await PickMediaAsync("Select Images", PHAssetMediaType.Image);
        }

        public async Task<IList<MediaFile>> PickVideosAsync()
        {
            return await PickMediaAsync("Select Videos", PHAssetMediaType.Video);
        }

        private async Task<IList<MediaFile>> PickMediaAsync(string title, PHAssetMediaType type)
        {
            _mediaPickTcs = new TaskCompletionSource<IList<MediaFile>>();
            _currentPicker = new GMImagePickerController
            {
                Title = title,
                MediaTypes = new[] { type }
            };

            _currentPicker.FinishedPickingAssets += FinishedPickingAssets;

            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            await vc.PresentViewControllerAsync(_currentPicker, true);

            var results = await _mediaPickTcs.Task;

            _currentPicker.FinishedPickingAssets -= FinishedPickingAssets;
            OnMediaPickedCompleted?.Invoke(this, results);
            return results;
        }

        private async void FinishedPickingAssets(object sender, MultiAssetEventArgs args)
        {
            IList<MediaFile> results = new List<MediaFile>();
            TaskCompletionSource<IList<MediaFile>> tcs = new TaskCompletionSource<IList<MediaFile>>();

            var options = new PHImageRequestOptions
            {
                NetworkAccessAllowed = true
            };

            options.Synchronous = false;
            options.ResizeMode = PHImageRequestOptionsResizeMode.Fast;
            options.DeliveryMode = PHImageRequestOptionsDeliveryMode.HighQualityFormat;
            bool completed = false;
            for (var i = 0; i < args.Assets.Length; i++)
            {
                var asset = args.Assets[i];

                string fileName = string.Empty;
                if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                {
                    fileName = PHAssetResource.GetAssetResources(asset).FirstOrDefault().OriginalFilename;
                }

                switch (asset.MediaType)
                {
                    case PHAssetMediaType.Video:

                        PHImageManager.DefaultManager.RequestImageForAsset(asset, new SizeF(150.0f, 150.0f),
                            PHImageContentMode.AspectFill, options, async (img, info) =>
                            {
                                var startIndex = fileName.IndexOf(".", StringComparison.CurrentCulture);

                                string path = string.Empty;
                                if (startIndex != -1)
                                {
                                    path = ReminderXamarin.Services.MediaPicker.FileHelper.GetOutputPath(MediaFileType.Image, TemporalDirectoryName, 
                                        $"{fileName.Substring(0, startIndex)}-THUMBNAIL.JPG");
                                }
                                else
                                {
                                    path = ReminderXamarin.Services.MediaPicker.FileHelper.GetOutputPath(MediaFileType.Image, TemporalDirectoryName, 
                                        string.Empty);

                                }

                                if (!File.Exists(path))
                                {

                                    img.AsJPEG().Save(path, true);
                                }

                                TaskCompletionSource<string> tvcs = new TaskCompletionSource<string>();

                                var vOptions = new PHVideoRequestOptions();
                                vOptions.NetworkAccessAllowed = true;
                                vOptions.Version = PHVideoRequestOptionsVersion.Original;
                                vOptions.DeliveryMode = PHVideoRequestOptionsDeliveryMode.FastFormat;


                                PHImageManager.DefaultManager.RequestAvAsset(asset, vOptions, (avAsset, audioMix, vInfo) =>
                                {
                                    var vPath = ReminderXamarin.Services.MediaPicker.FileHelper.GetOutputPath(MediaFileType.Video, TemporalDirectoryName, fileName);

                                    if (!File.Exists(vPath))
                                    {
                                        AVAssetExportSession exportSession = new AVAssetExportSession(avAsset, AVAssetExportSession.PresetHighestQuality);

                                        exportSession.OutputUrl = NSUrl.FromFilename(vPath);
                                        exportSession.OutputFileType = AVFileType.QuickTimeMovie;


                                        exportSession.ExportAsynchronously(() =>
                                        {
                                            Console.WriteLine(exportSession.Status);

                                            tvcs.TrySetResult(vPath);
                                            //exportSession.Dispose();
                                        });

                                    }

                                });

                                var videoUrl = await tvcs.Task;
                                var meFile = new MediaFile()
                                {
                                    Type = MediaFileType.Video,
                                    Path = videoUrl,
                                    PreviewPath = path
                                };
                                results.Add(meFile);
                                OnMediaPicked?.Invoke(this, meFile);

                                if (args.Assets.Length == results.Count && !completed)
                                {
                                    completed = true;
                                    tcs.TrySetResult(results);
                                }

                            });
                        break;
                    default:
                        PHImageManager.DefaultManager.RequestImageData(asset, options, (data, dataUti, orientation, info) =>
                        {

                            string path = ReminderXamarin.Services.MediaPicker.FileHelper.GetOutputPath(MediaFileType.Image, TemporalDirectoryName, fileName);

                            if (!File.Exists(path))
                            {
                                Debug.WriteLine(dataUti);
                                var imageData = data;
                                //var image = UIImage.LoadFromData(imageData);

                                //if (imageScale < 1.0f)
                                //{
                                //    //begin resizing image
                                //    image = image.ResizeImageWithAspectRatio((float)imageScale);
                                //}

                                //if (imageQuality < 100)
                                //{
                                //    imageData = image.AsJPEG(Math.Min(imageQuality,100));
                                //}

                                imageData?.Save(path, true);
                            }

                            var meFile = new MediaFile
                            {
                                Type = MediaFileType.Image,
                                Path = path,
                                PreviewPath = path
                            };

                            results.Add(meFile);
                            OnMediaPicked?.Invoke(this, meFile);
                            if (args.Assets.Length == results.Count && !completed)
                            {
                                completed = true;
                                tcs.TrySetResult(results);
                            }
                        });
                        break;
                }
            }
            _mediaPickTcs?.TrySetResult(await tcs.Task);
        }
    }
}