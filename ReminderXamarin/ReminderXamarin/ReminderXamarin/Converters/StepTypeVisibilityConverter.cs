using System;
using System.Globalization;

using ReminderXamarin.ViewModels;

using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class StepTypeVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AchievementStepType achievementStepType)
            {
                if (achievementStepType == AchievementStepType.TimeSpending)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}