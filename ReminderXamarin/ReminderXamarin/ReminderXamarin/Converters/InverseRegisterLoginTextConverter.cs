using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using ReminderXamarin.Extensions;
using Rm.Helpers;
using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class InverseRegisterLoginTextConverter : IValueConverter
    {
        public static readonly Lazy<ResourceManager> Resmgr = new Lazy<ResourceManager>(
            () => new ResourceManager(ConstantsHelper.TranslationResourcePath, typeof(TranslateExtension).GetTypeInfo().Assembly));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Resmgr.Value.GetString(ConstantsHelper.Login, culture);
            }
            return Resmgr.Value.GetString(ConstantsHelper.Register, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == Resmgr.Value.GetString(ConstantsHelper.Login, culture))
            {
                return true;
            }

            return false;
        }
    }
}