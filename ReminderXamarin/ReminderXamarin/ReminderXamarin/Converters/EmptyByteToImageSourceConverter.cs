using System;
using System.IO;

using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class EmptyByteToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource retSource = null;
            if (value != null)
            {
                byte[] imageAsBytes = (byte[])value;

                retSource = imageAsBytes.Length == 0 ?
                    "profile_icon.png"
                    : ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }
            return retSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}