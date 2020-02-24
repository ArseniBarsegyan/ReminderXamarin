using System.Windows.Input;
using Rm.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;
using ReminderXamarin.Utilities;
using ReminderXamarin.DependencyResolver;

namespace ReminderXamarin.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ThemeSwitcher _themeSwitcher;
        private bool _isDarkTheme;

        public SettingsViewModel()
        {
            bool.TryParse(Settings.UsePin, out bool shouldUsePin);
            UsePin = shouldUsePin;
            Pin = Settings.UserPinCode;
            _themeSwitcher = ComponentFactory.Resolve<ThemeSwitcher>();

            SaveSettingsCommand = new Command(SaveSettings);
        }

        /// <summary>
        /// Use pin instead of username/password
        /// </summary>
        public bool UsePin { get; set; }
        public string Pin { get; set; }

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (value != _isDarkTheme)
                {
                    _isDarkTheme = value;
                    _themeSwitcher.SwitchTheme(value ? ThemeTypes.Dark : ThemeTypes.Light);
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveSettingsCommand { get; set; }

        private void SaveSettings()
        {
            Settings.UserPinCode = int.TryParse(Pin, out var pin) ? Pin : "1111";
            Settings.UsePin = UsePin.ToString();
        }
    }
}