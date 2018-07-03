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

                if (imageAsBytes.Length == 0)
                {
                    retSource = "https://www.cabe-africa.org/wp-content/uploads/2012/01/1.png";
                }
                else
                {
                    retSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                }
            }
            return retSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}