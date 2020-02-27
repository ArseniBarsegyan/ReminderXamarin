using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    public class ExtendedLabel : Label
    {
        public static readonly BindableProperty LinesProperty =
            BindableProperty.Create(nameof(Lines), 
                typeof(int), 
                typeof(ExtendedLabel), 
                3);

        public static readonly BindableProperty LineSpacingProperty =
            BindableProperty.Create(nameof(LineSpacing), 
                typeof(double), 
                typeof(ExtendedLabel), 
                1.3);

        public static readonly BindableProperty IsUnderlinedProperty =
            BindableProperty.Create(nameof(IsUnderlined), 
                typeof(bool), 
                typeof(ExtendedLabel), 
                false, 
                BindingMode.Default);

        public int Lines
        {
            get => (int)GetValue(LinesProperty);
            set => SetValue(LinesProperty, value);
        }

        public double LineSpacing
        {
            get => (double)GetValue(LineSpacingProperty);
            set => SetValue(LineSpacingProperty, value);
        }

        public bool IsUnderlined
        {
            get => (bool)GetValue(IsUnderlinedProperty);
            set => SetValue(IsUnderlinedProperty, value);
        }
    }
}