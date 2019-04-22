using System;
using System.Globalization;
using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class InverseRegisterLoginTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return "Login";
            }
            return "Register";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Login")
            {
                return true;
            }

            return false;
        }
    }
}