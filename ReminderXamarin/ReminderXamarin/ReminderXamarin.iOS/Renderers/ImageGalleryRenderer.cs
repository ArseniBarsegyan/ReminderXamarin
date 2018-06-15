using ReminderXamarin.Elements;
using ReminderXamarin.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ImageGallery), typeof(ImageGalleryRenderer))]
namespace ReminderXamarin.iOS.Renderers
{
    public class ImageGalleryRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var element = e.NewElement as ImageGallery;
            element?.Render();
        }
    }
}