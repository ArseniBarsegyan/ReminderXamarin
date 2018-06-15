using Android.Content;
using ReminderXamarin.Droid.Renderers;
using ReminderXamarin.Elements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ImageGallery), typeof(ImageGalleryRenderer))]
namespace ReminderXamarin.Droid.Renderers
{
    public class ImageGalleryRenderer : ScrollViewRenderer
    {
        public ImageGalleryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var element = e.NewElement as ImageGallery;
            element?.Render();
        }
    }
}