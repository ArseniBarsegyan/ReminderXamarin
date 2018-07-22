﻿using System.Windows.Input;
using ReminderXamarin.Helpers;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        public SettingsPageViewModel()
        {
            bool.TryParse(Settings.UsePin, out bool shouldUsePin);
            UsePin = shouldUsePin;
            SaveSettingsCommand = new Command(SaveCommandExecute);
        }

        /// <summary>
        /// Use pin instead of username/password
        /// </summary>
        public bool UsePin { get; set; }

        public ICommand SaveSettingsCommand { get; set; }

        private void SaveCommandExecute()
        {
            Settings.UsePin = UsePin.ToString();
        }
    }
}