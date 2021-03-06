﻿using System.IO;

using Android.Graphics;
using Android.Media;

namespace ReminderXamarin.Droid.Services.MediaPicker
{
    public static class ImageHelpers
    {
        public static byte[] GetRotatedImage(string path, float scaleFactor, int quality = 100)
        {
            byte[] imageBytes;

            using (var originalImage = BitmapFactory.DecodeFile(path))
            {
                var rotation = GetRotation(path);
                var width = (originalImage.Width * scaleFactor);
                var height = (originalImage.Height * scaleFactor);

                using (var scaledImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, true))
                {
                    Bitmap rotatedImage = null;
                    
                    try
                    {
                        rotatedImage = scaledImage;

                        if (rotation != 0)
                        {
                            var matrix = new Matrix();
                            matrix.PostRotate(rotation);

                            rotatedImage = Bitmap.CreateBitmap(scaledImage,
                                0,
                                0,
                                scaledImage.Width,
                                scaledImage.Height,
                                matrix,
                                true);

                            scaledImage.Recycle();
                        }

                        using (var ms = new MemoryStream())
                        {
                            rotatedImage.Compress(Bitmap.CompressFormat.Jpeg, quality, ms);
                            imageBytes = ms.ToArray();
                        }
                    }
                    finally
                    {
                        rotatedImage.Dispose();
                    }
                }
            }

            return imageBytes;
        }

        private static int GetRotation(string filePath)
        {
            using (var ei = new ExifInterface(filePath))
            {
                var orientation = (Orientation)ei.GetAttributeInt(
                    ExifInterface.TagOrientation, 
                    (int)Orientation.Normal);

                switch (orientation)
                {
                    case Orientation.Rotate90:
                        return 90;
                    case Orientation.Rotate180:
                        return 180;
                    case Orientation.Rotate270:
                        return 270;
                    default:
                        return 0;
                }
            }
        }
    }
}