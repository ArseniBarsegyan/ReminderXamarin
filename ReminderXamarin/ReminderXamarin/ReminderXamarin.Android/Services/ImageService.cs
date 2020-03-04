using System.IO;

using Android.Graphics;

using ReminderXamarin.Core.Interfaces;

namespace ReminderXamarin.Droid.Services
{
    public class ImageService : IImageService
    {
        public void ResizeImage(string sourceFile, string targetFile, int requiredWidth, int requiredHeight)
        {
            if (!File.Exists(targetFile) && File.Exists(sourceFile))
            {
                var downImg = DecodeSampledBitmapFromFile(sourceFile, requiredWidth, requiredHeight);
                using (var outStream = File.Create(targetFile))
                {
                    if (targetFile.ToLower().EndsWith("png"))
                    {
                        downImg.Compress(Bitmap.CompressFormat.Png, 100, outStream);
                    }
                    else
                    {
                        downImg.Compress(Bitmap.CompressFormat.Jpeg, 95, outStream);
                    }
                }
                downImg.Recycle();
            }
        }

        private static Bitmap DecodeSampledBitmapFromFile(string path, int requiredWidth, int requiredHeight)
        {
            var options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(path, options);

            options.InSampleSize = CalculateInSampleSize(options, requiredWidth, requiredHeight);
            options.InJustDecodeBounds = false;
            return BitmapFactory.DecodeFile(path, options);
        }

        private static int CalculateInSampleSize(BitmapFactory.Options options, int requiredWidth, int requiredHeight)
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