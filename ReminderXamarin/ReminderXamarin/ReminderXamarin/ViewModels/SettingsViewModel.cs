﻿using System.Windows.Input;
using Rm.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            bool.TryParse(Settings.UsePin, out bool shouldUsePin);
            UsePin = shouldUsePin;
            Pin = Settings.UserPinCode;
            SaveSettingsCommand = new Command(SaveSettings);
        }

        /// <summary>
        /// Use pin instead of username/password
        /// </summary>
        public bool UsePin { get; set; }
        public string Pin { get; set; }

        public ICommand SaveSettingsCommand { get; set; }

        private void SaveSettings()
        {
            Settings.UserPinCode = int.TryParse(Pin, out var pin) ? Pin : "1111";
            Settings.UsePin = UsePin.ToString();
        }
    }
}