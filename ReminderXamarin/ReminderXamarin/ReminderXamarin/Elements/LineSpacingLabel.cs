using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    /// <summary>
    /// Label with line spacing property (adjust spacing between lines).
    /// </summary>
    public class LineSpacingLabel : Label
    {
        public static readonly BindableProperty LinesProperty =
            BindableProperty.Create(nameof(Lines), typeof(int), typeof(LineSpacingLabel), 3);
        public static readonly BindableProperty LineSpacingProperty =
            BindableProperty.Create(nameof(LineSpacing), typeof(double), typeof(LineSpacingLabel), 1.3);

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
    }
}