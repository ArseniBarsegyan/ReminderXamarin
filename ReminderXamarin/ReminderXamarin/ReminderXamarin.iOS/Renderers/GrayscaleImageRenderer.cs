using System;

using CoreGraphics;

using ReminderXamarin.Elements;
using ReminderXamarin.iOS.Renderers;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GrayscaleImage), typeof(GrayscaleImageRenderer))]
namespace ReminderXamarin.iOS.Renderers
{
    public class GrayscaleImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Image> e)
        {
            if (Control != null)
            {
                Control.Image = ConvertToGrayScale(Control.Image);
            }

            base.OnElementChanged(e);
        }

        private UIImage ConvertToGrayScale(UIImage image)
        {
            CGRect imageRect = new CGRect(0, 0, image.Size.Width, image.Size.Height);

            using (var colorSpace = CGColorSpace.CreateDeviceGray())
            using (var context = new CGBitmapContext(IntPtr.Zero, 
                (int)image.Size.Width, 
                (int)image.Size.Height, 8, 0, colorSpace, CGImageAlphaInfo.None))
            {
                context.DrawImage(imageRect, image.CGImage);

                using (var imageRef = context.ToImage())
                    return new UIImage(imageRef);
            }
        }        
    }
}