using Android.Content;
using ReminderXamarin.Droid.Renderers;
using ReminderXamarin.Elements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]
namespace ReminderXamarin.Droid.Renderers
{
    /// <summary>
    /// Renderer for <see cref="ExtendedEditor"/>
    /// </summary>
    public class ExtendedEditorRenderer : EditorRenderer
    {
        public ExtendedEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var extendedEditor = e.NewElement as ExtendedEditor;
                Control.Hint = extendedEditor?.Placeholder;
            }
        }
    }
}