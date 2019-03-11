using ReminderXamarin.Elements;
using Templates.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EditorWithBorder), typeof(EditorWithBorderRenderer))]
namespace Templates.iOS.Renderers
{
    /// <summary>
    /// Renderer for <see cref="Templates.Controls.EditorWithBorder"/>
    /// </summary>
    public class EditorWithBorderRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control != null)
            {
                var view = Element as EditorWithBorder;

                Control.Layer.BorderColor = view.BorderColor.ToCGColor();
                Control.Layer.BorderWidth = 1;
                Control.ClipsToBounds = true;
            }
        }
    }
}