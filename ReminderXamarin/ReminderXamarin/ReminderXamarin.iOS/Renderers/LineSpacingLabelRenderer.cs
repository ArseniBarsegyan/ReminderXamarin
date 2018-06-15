using System;
using Foundation;
using ReminderXamarin.Elements;
using ReminderXamarin.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LineSpacingLabel), typeof(LineSpacingLabelRenderer))]
namespace ReminderXamarin.iOS.Renderers
{
    public class LineSpacingLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                var lineSpacingLabel = Element as LineSpacingLabel;
                var paragraphStyle = new NSMutableParagraphStyle
                {
                    LineSpacing = (nfloat)lineSpacingLabel.LineSpacing
                };
                var str = new NSMutableAttributedString(lineSpacingLabel.Text);
                var style = UIStringAttributeKey.ParagraphStyle;
                var range = new NSRange(0, str.Length);

                str.AddAttribute(style, paragraphStyle, range);
                Control.Lines = lineSpacingLabel.Lines;
                Control.AttributedText = str;
            }
        }
    }
}