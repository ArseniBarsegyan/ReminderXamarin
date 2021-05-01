using ReminderXamarin.Collections;

using System;
using System.Globalization;

using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class DaysWithToDoToHasToDoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RangeObservableCollection<DateTime> daysWithToDo)
            {
                DateTime currentDay = (DateTime)parameter;

                foreach(var time in daysWithToDo)
                {
                    if (time.ToString("D") == currentDay.ToString("D"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
