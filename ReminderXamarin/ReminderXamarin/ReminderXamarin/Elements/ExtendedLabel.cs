using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    public class ExtendedLabel : Label
    {
        private const int DefaultLinesCount = 3;
        private const double DefaultLinesSpacing = 1.3;

        public static readonly BindableProperty LinesProperty =
            BindableProperty.Create(nameof(Lines), 
                typeof(int), 
                typeof(ExtendedLabel),
                DefaultLinesCount);

        public static readonly BindableProperty LineSpacingProperty =
            BindableProperty.Create(nameof(LineSpacing), 
                typeof(double), 
                typeof(ExtendedLabel),
                DefaultLinesSpacing);

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