using System;
using System.Globalization;
using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class CalendarDayViewBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
            {
                return (Color)Application.Current.Resources["CalendarSelectedDate"];
            }
            
            return (Color)Application.Current.Resources["ViewBackground"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}