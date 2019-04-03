using System.ComponentModel;
using CoreGraphics;
using Foundation;
using ReminderXamarin.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ReminderXamarin.Elements.FloatingActionButton), typeof(FloatingActionButtonRenderer))]
namespace ReminderXamarin.iOS.Renderers
{
    [Preserve]
    public class FloatingActionButtonRenderer : ButtonRenderer
    {
        public FloatingActionButtonRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            if (Control == null)
            {
                return;
            }

            Element.WidthRequest = 50;
            Element.HeightRequest = 50;
            Element.CornerRadius = 25;
            Element.BorderWidth = 0;
            Element.Text = null;
           
            Control.BackgroundColor = ((Elements.FloatingActionButton)Element).ButtonColor.ToUIColor();
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            
            Layer.ShadowRadius = 2.0f;
            Layer.ShadowColor = UIColor.Black.CGColor;
            Layer.ShadowOffset = new CGSize(1, 1);
            Layer.ShadowOpacity = 0.80f;
            Layer.ShadowPath = UIBezierPath.FromOval(Layer.Bounds).CGPath;
            Layer.MasksToBounds = false;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "ButtonColor")
            {
                Control.BackgroundColor = ((Elements.FloatingActionButton)Element).ButtonColor.ToUIColor();
            }
        }
    }
}