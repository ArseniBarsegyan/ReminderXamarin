﻿using System;
using System.Globalization;

using Xamarin.Forms;

namespace ReminderXamarin.Converters
{
    public class AchievementSpentTimePresentationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int hoursSpent = (int) value;
            if (hoursSpent < 1000)
            {
                return hoursSpent;
            }
            return $"{Math.Floor((double)hoursSpent/100)/10}k";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}