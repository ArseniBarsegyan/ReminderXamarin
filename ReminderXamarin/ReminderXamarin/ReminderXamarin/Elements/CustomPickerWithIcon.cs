using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    public class CustomPickerWithIcon : Picker
    {
        public static readonly BindableProperty ImageProperty = 
            BindableProperty.Create(nameof(Image),
                typeof(string),
                typeof(CustomPickerWithIcon),
                ConstantsHelper.ArrowForwardImage);

        public static BindableProperty PlaceholderColorProperty 
            = BindableProperty.Create(nameof(PlaceholderColor),
                typeof(Color),
                typeof(CustomPickerWithIcon),
                Color.DodgerBlue,
                BindingMode.TwoWay);

        public static readonly BindableProperty BorderColorProperty 
            = BindableProperty.Create(nameof(BorderColor),
                typeof(Color),
                typeof(CustomPickerWithIcon),
                Color.DodgerBlue,
                BindingMode.Default);

        public string Image
        {
            get => (string)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }
    }
}