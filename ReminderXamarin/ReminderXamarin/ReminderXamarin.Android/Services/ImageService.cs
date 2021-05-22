using System.IO;

using Android.Graphics;

using ReminderXamarin.Core.Interfaces;

namespace ReminderXamarin.Droid.Services
{
    public class ImageService : IImageService
    {
        [Xamarin.Forms.Internals.Preserve]
        public ImageService()
        {
        }
        
        public void ResizeImage(string sourceFile, string targetFile, int requiredWidth, int requiredHeight)
        {
            if (!File.Exists(targetFile) && File.Exists(sourceFile))
            {
                using (Bitmap bitmap = DecodeSampledBitmapFromFile(sourceFile, requiredWidth, requiredHeight))
                using (FileStream stream = File.Create(targetFile))
                {
                    if (targetFile.ToLower().EndsWith("png"))
                    {
                        bitmap?.Compress(Bitmap.CompressFormat.Png, 100, stream);
                    }
                    else
                    {
                        bitmap?.Compress(Bitmap.CompressFormat.Jpeg, 95, stream);
                    }

                    bitmap?.Recycle();
                }
            }
        }

        private Bitmap DecodeSampledBitmapFromFile(string path, int requiredWidth, int requiredHeight)
        {
            var options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(path, options);

            options.InSampleSize = CalculateInSampleSize(options, requiredWidth, requiredHeight);
            options.InJustDecodeBounds = false;
            return BitmapFactory.DecodeFile(path, options);
        }

        private int CalculateInSampleSize(BitmapFactory.Options options, int requiredWidth, int requiredHeight)
        {
            var height = options.OutHeight;
            var width = options.OutWidth;
            var inSampleSize = 1;

            if (height > requiredHeight || width > requiredWidth)
            {
                var halfHeight = height / 2;
                var halfWidth = width / 2;

                while ((halfHeight / inSampleSize) > requiredHeight
                       && (halfWidth / inSampleSize) > requiredWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return inSampleSize;
        }
    }
}