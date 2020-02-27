using System;
using System.Globalization;

using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (string) value;
            return !string.IsNullOrWhiteSpace(val);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}