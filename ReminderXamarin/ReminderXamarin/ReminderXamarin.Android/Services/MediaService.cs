using System.IO;

using Android.Graphics;
using Android.Media;

using Java.IO;

using ReminderXamarin.Core.Interfaces;

namespace ReminderXamarin.Droid.Services
{
    public class MediaService : IMediaService
    {
        [Xamarin.Forms.Internals.Preserve]
        public MediaService()
        {
        }
        
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            // Load the bitmap 
            using (BitmapFactory.Options options = new BitmapFactory.Options { InPurgeable = true })
            using (Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options))
            {
                float newHeight = 0;
                float newWidth = 0;

                var originalHeight = originalImage.Height;
                var originalWidth = originalImage.Width;

                if (originalHeight > originalWidth)
                {
                    newHeight = height;
                    float ratio = originalHeight / height;
                    newWidth = originalWidth / ratio;
                }
                else
                {
                    newWidth = width;
                    float ratio = originalWidth / width;
                    newHeight = originalHeight / ratio;
                }

                using (Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, true))
                {
                    originalImage.Recycle();

                    using (var ms = new MemoryStream())
                    {
                        resizedImage.Compress(Bitmap.CompressFormat.Png, 100, ms);
                        resizedImage.Recycle();
                        return ms.ToArray();
                    }
                }
            }
        }

        public byte[] GenerateThumbImage(string url, long second)
        {
            using (MediaMetadataRetriever retriever = new MediaMetadataRetriever())
            using (FileInputStream inputStream = new FileInputStream(url))
            using (Bitmap bitmap = retriever.GetFrameAtTime(second))
            {
                retriever.SetDataSource(inputStream.FD);

                if (bitmap != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Png, 0, ms);
                        byte[] bitmapData = ms.ToArray();
                        return bitmapData;
                    }
                }
            }
            return null;
        }
    }
}