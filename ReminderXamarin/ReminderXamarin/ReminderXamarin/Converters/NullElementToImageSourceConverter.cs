using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    /// <summary>
    /// Convert object to image source. If object is null return empty string.
    /// </summary>
    public class NullElementToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vmList = (IEnumerable<PhotoViewModel>) value;
            if (vmList.Any())
            {
                return vmList.ElementAt(0).ResizedPath;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}