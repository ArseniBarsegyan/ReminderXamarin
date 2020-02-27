using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    public class FloatingActionButton : Button
    {
        public static BindableProperty ButtonColorProperty = 
            BindableProperty.Create(nameof(ButtonColor),
                typeof(Color),
                typeof(FloatingActionButton),
                Color.Accent);

        public Color ButtonColor
        {
            get => (Color)GetValue(ButtonColorProperty);
            set => SetValue(ButtonColorProperty, value);
        }
    }
}