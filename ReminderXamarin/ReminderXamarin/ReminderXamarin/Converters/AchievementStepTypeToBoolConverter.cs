using System;
using System.Globalization;

using ReminderXamarin.ViewModels;

using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class AchievementStepTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var achievementType = (AchievementStepType)value;
            return achievementType == AchievementStepType.TimeSpending;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
