using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    /// <summary>
    /// If object exists returns true otherwise false.
    /// </summary>
    public class NullElementToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vmList = (IEnumerable<PhotoViewModel>)value;
            if (vmList.Any())
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}