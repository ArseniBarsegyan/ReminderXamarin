using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using ReminderXamarin.Extensions;

using Rm.Data.Data.Entities;

using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class NullElementToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vmList = (IEnumerable<GalleryItemModel>) value;
            if (vmList.IsNullOrEmpty())
            {
                return string.Empty;
            }
            return vmList.Any() ? vmList.ElementAt(0).ImagePath : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}