using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    /// <summary>
    /// Editor with placeholder.
    /// </summary>
    public class ExtendedEditor : Editor
    {
        public static BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder),
            typeof(string), typeof(ExtendedEditor), string.Empty, BindingMode.TwoWay);
        
        /// <summary>
        /// Placeholder text
        /// </summary>
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
    }
}
