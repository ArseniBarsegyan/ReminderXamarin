using System;
using System.IO;
using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    /// <inheritdoc />
    /// <summary>
    /// Convert byte array to image source. Set image to empty user if byte array is empty.
    /// </summary>
    public class EmptyByteToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource retSource = null;
            if (value != null)
            {
                byte[] imageAsBytes = (byte[])value;

                retSource = imageAsBytes.Length == 0 ? 
                    "user_profile.png" 
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