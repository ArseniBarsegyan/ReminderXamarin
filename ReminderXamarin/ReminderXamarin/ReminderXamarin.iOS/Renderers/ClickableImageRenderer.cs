using ReminderXamarin.Elements;
using ReminderXamarin.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ClickableImage), typeof(ClickableImageRenderer))]
namespace ReminderXamarin.iOS.Renderers
{
    public class ClickableImageRenderer : ImageRenderer
    {
        private UIImageView _nativeElement;
        private ClickableImage _formsElement;

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                _formsElement = e.NewElement as ClickableImage;
                _nativeElement = Control as UIImageView;
                _nativeElement.UserInteractionEnabled = true;
                var clickGestureRecognizer = new UITapGestureRecognizer(ShowFullSizeImage);
                _nativeElement.AddGestureRecognizer(clickGestureRecognizer);
            }
        }

        private void ShowFullSizeImage()
        {
            _formsElement.ShowFullSizeImage();
        }
    }
}