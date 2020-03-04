using System;
using System.IO;

using CoreGraphics;

using ReminderXamarin.Core.Interfaces;

using UIKit;

namespace ReminderXamarin.iOS.Services
{
    public class ImageService : IImageService
    {
        public void ResizeImage(string sourceFile, string targetFile, int requiredWidth, int requiredHeight)
        {
            if (File.Exists(sourceFile) && !File.Exists(targetFile))
            {
                using (var sourceImage = UIImage.FromFile(sourceFile))
                {
                    var sourceSize = sourceImage.Size;
                    var maxResizeFactor = Math.Min(requiredWidth / sourceSize.Width, requiredHeight / sourceSize.Height);

                    if (!Directory.Exists(Path.GetDirectoryName(targetFile)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
                    }

                    if (maxResizeFactor > 0.9)
                    {
                        File.Copy(sourceFile, targetFile);
                    }
                    else
                    {
                        var width = maxResizeFactor * sourceSize.Width;
                        var height = maxResizeFactor * sourceSize.Height;

                        UIGraphics.BeginImageContextWithOptions(new CGSize((float)width, (float)height), true, 1.0f);

                        sourceImage.Draw(new CGRect(0, 0, (float)width, (float)height));

                        var resultImage = UIGraphics.GetImageFromCurrentImageContext();
                        UIGraphics.EndImageContext();


                        if (targetFile.ToLower().EndsWith("png"))
                        {
                            resultImage.AsPNG().Save(targetFile, true);
                        }
                        else
                        {
                            resultImage.AsJPEG().Save(targetFile, true);
                        }
                    }
                }
            }
        }
    }
}