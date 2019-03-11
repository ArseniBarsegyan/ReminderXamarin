using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    /// <summary>
    /// Editor with colored border.
    /// </summary>
    public class EditorWithBorder : Editor
    {
        public static BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder),
            typeof(string), typeof(EditorWithBorder), string.Empty, BindingMode.TwoWay);

        public static BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor),
            typeof(Color), typeof(EditorWithBorder), Color.Gray, BindingMode.TwoWay);
        

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(EditorWithBorder), Color.DodgerBlue, BindingMode.Default);

        /// <summary>
        /// Border color.
        /// </summary>
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        /// <summary>
        /// Placeholder text
        /// </summary>
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        /// <summary>
        /// Color of placeholder in hex
        /// </summary>
        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }
    }
}
