using Android.Content;
using Android.Graphics;

using ReminderXamarin.Droid.Renderers;
using ReminderXamarin.Elements;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GrayscaleImage), typeof(GrayscaleImageRenderer))]
namespace ReminderXamarin.Droid.Renderers
{
    public class GrayscaleImageRenderer : ImageRenderer
    {
        public GrayscaleImageRenderer(Context context) 
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.SetSaturation(0);
                ColorMatrixColorFilter cf = new ColorMatrixColorFilter(matrix);
                Control.SetColorFilter(cf);
                Control.ImageAlpha = 128;
            }
        }
    }
}