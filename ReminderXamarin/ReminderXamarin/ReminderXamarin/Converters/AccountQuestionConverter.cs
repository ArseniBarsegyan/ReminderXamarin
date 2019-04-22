using System;
using System.Globalization;
using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class AccountQuestionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return "Already have account?";
            }
            return "Don't have account?";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Already have account?")
            {
                return true;
            }
            return false;
        }
    }
}