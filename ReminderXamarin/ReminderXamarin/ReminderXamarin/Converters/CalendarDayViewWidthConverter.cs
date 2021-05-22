using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

using ReminderXamarin.Extensions;

using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class CalendarDayViewWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width)
            {
                return width / 7;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}