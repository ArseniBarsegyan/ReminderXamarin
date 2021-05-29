using System;
using System.IO;
using System.Threading.Tasks;

using Android.Content;
using Android.Provider;
using ReminderXamarin.Droid.Services.MediaPicker;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.Services.MediaPicker;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Droid.Services.FilePickerService
{
    public class PlatformDocumentPicker : IPlatformDocumentPicker
    {
        private const string TemporalDirectoryName = "TmpMedia";
        
        [Preserve]
        public PlatformDocumentPicker()
        {
        }
        
        public async Task<PlatformDocument> DisplayImportAsync()
        {
            var intent = await ShowPickerDialog();
            if (intent != null)
            {
                return await StartActivity(intent);
            }
            return null;
        }

        private static Task<Intent> ShowPickerDialog()
        {
            var taskCompletionSource = new TaskCompletionSource<Intent>();

            var intentFromLibrary = new Intent()
                .SetAction(Intent.ActionGetContent)
                .SetType("image/*")
                .AddCategory(Intent.CategoryOpenable);

            taskCompletionSource.SetResult(intentFromLibrary);
            return taskCompletionSource.Task;
        }

        private Task<PlatformDocument> StartActivity(Intent requestIntent)
        {
            var taskCompletionSource = new TaskCompletionSource<PlatformDocument>();
            Platform.StartActivityForResult(requestIntent, (result, responseIntent) =>
            {
                try
                {
                    if (result != Android.App.Result.Canceled)
                    {
                        var url = responseIntent.Data;
                        if (url != null)
                        {
                            var name = string.Empty;
                            var contentResolver = Platform.MainActivity.ContentResolver;
                            using (var cursor = contentResolver?.Query(
                                url,
                                new[] { OpenableColumns.DisplayName }, null, null, null))
                            {
                                if (cursor?.MoveToFirst() ?? false)
                                {
                                    var columnIndex = cursor.GetColumnIndex(OpenableColumns.DisplayName);
                                    name = cursor.IsNull(columnIndex) ? string.Empty : cursor.GetString(columnIndex);
                                }
                            }
                            if (string.IsNullOrEmpty(name))
                            {
                                name = url.LastPathSegment;
                            }
                            var path = Path.GetTempFileName();
                            using (var dataStream = Platform.MainActivity.ContentResolver?.OpenInputStream(url))
                            using (var fileStream = File.OpenWrite(path))
                            {
                                dataStream?.CopyTo(fileStream);
                            }
                            
                            var fileName = Path.GetFileNameWithoutExtension(path);
                            var ext = Path.GetExtension(path);
                            
                            var fullPath = ReminderXamarin.Services.MediaPicker.FileHelper.GetOutputPath(
                                MediaFileType.Image,
                                TemporalDirectoryName,
                                $"{fileName}{ext}");

                            var fullImage = ImageHelpers.GetRotatedImage(path, 1);
                            File.WriteAllBytes(fullPath, fullImage);

                            taskCompletionSource.SetResult(new PlatformDocument(
                                name: name,
                                path: fullPath
                            ));
                            return;
                        }
                    }
                    taskCompletionSource.SetResult(null);
                }
                catch (Exception e)
                {
                    taskCompletionSource.SetException(e);
                }
            });
            return taskCompletionSource.Task;
        }
    }
}