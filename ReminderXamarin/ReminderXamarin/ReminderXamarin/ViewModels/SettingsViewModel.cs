using System;
using System.Windows.Input;

using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.Utilities;
using ReminderXamarin.ViewModels.Base;

using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ThemeSwitcher _themeSwitcher;
        private ThemeTypes _savedTheme;
        private bool _isDarkTheme;

        public SettingsViewModel(INavigationService navigationService,
            ThemeSwitcher themeSwitcher)
            : base(navigationService)
        {
            _themeSwitcher = themeSwitcher;

            bool.TryParse(Settings.UsePin, out bool shouldUsePin);
            UsePin = shouldUsePin;
            Pin = Settings.UserPinCode;

            if ((ThemeTypes)Enum.Parse(typeof(ThemeTypes), Settings.ThemeType) == ThemeTypes.Dark)
            {
                IsDarkTheme = true;
            }
            SaveSettingsCommand = new Command(SaveSettings);
        }

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

        public void OnDisappearing()
        {
            _savedTheme = (ThemeTypes)Enum.Parse(typeof(ThemeTypes), Settings.ThemeType);
            Settings.ThemeType = _savedTheme.ToString();
            _themeSwitcher.SwitchTheme(_savedTheme);
        }

        private void SaveSettings()
        {
            Settings.UserPinCode = int.TryParse(Pin, out var pin) ? Pin : "1111";
            Settings.UsePin = UsePin.ToString();

            Settings.ThemeType = _themeSwitcher.CurrentThemeType.ToString();
        }
    }
}