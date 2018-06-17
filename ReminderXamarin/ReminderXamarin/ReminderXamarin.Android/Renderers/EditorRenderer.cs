using Android.Content;
using Android.Text;
using Android.Views.InputMethods;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using EditorRenderer = ReminderXamarin.Droid.Renderers.EditorRenderer;

[assembly: ExportRenderer(typeof(Editor), typeof(EditorRenderer))]
namespace ReminderXamarin.Droid.Renderers
{
    public class EditorRenderer : Xamarin.Forms.Platform.Android.EditorRenderer
    {
        public EditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = null;
                Control.InputType = InputTypes.TextFlagCapSentences;
                Control.SetSingleLine(false);
            }
        }
    }
}