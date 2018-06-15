using Android.Content;
using ReminderXamarin.Droid.Renderers;
using ReminderXamarin.Elements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ClickableImage), typeof(ClickableImageRenderer))]
namespace ReminderXamarin.Droid.Renderers
{
    public class ClickableImageRenderer : ImageRenderer
    {
        public ClickableImageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            var img = e.NewElement as ClickableImage;

            Control.Click += (sender, args) =>
            {
                img?.ShowFullSizeImage();
            };
        }
    }
}