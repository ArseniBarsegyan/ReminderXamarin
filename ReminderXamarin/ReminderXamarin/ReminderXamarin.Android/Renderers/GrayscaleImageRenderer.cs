using Android.Content;
using Android.Graphics;
using Android.Widget;

using ReminderXamarin.Droid.Renderers;
using ReminderXamarin.Elements;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GrayscaleImage), typeof(GrayscaleImageRenderer))]
namespace ReminderXamarin.Droid.Renderers
{
    public class GrayscaleImageRenderer : ViewRenderer<GrayscaleImage, ImageView>
    {
        public GrayscaleImageRenderer(Context context) 
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<GrayscaleImage> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(new ImageView(Context));
            }

            if (Control != null)
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.SetSaturation(0);
                Control.SetColorFilter(new ColorMatrixColorFilter(matrix));
            }
        }
    }
}