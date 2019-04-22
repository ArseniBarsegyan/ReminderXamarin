using System;
using System.Globalization;
using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class RegisterLoginTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
            {
                return "Register";
            }
            return "Login";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string) value == "Register")
            {
                return true;
            }
            return false;
        }
    }
}