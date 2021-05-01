using ReminderXamarin.Enums;

using System;
using System.Globalization;

using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class ToDoStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ToDoStatus status)
            {
                switch(status)
                {
                    case ToDoStatus.Active:
                        return (Color)Application.Current.Resources["ToDoActive"];
                    case ToDoStatus.Completed:
                        return (Color)Application.Current.Resources["ToDoCompleted"];
                    case ToDoStatus.Failed:
                        return (Color)Application.Current.Resources["ToDoFailed"];
                }
            }
            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
