using ReminderXamarin.Elements;
using ReminderXamarin.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]
namespace ReminderXamarin.iOS.Renderers
{
    /// <summary>
    /// Renderer for <see cref="ExtendedEditor"/>
    /// </summary>
    public class ExtendedEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.AutocapitalizationType = UITextAutocapitalizationType.Sentences;
            }
        }
    }
}