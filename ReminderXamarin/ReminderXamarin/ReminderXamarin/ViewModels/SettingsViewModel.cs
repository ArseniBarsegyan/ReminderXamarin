using System;
using System.Windows.Input;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.Utilities;
using ReminderXamarin.ViewModels.Base;

using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IThemeService _themeService;
        private readonly ThemeSwitcher _themeSwitcher;
        private ThemeTypes _savedTheme;
        private bool _isDarkTheme;

        public SettingsViewModel(INavigationService navigationService,
            ThemeSwitcher themeSwitcher,
            IThemeService themeService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _themeService = themeService;
            _themeSwitcher = themeSwitcher;

            _savedTheme = (ThemeTypes)Enum.Parse(typeof(ThemeTypes), Settings.ThemeType);

            bool.TryParse(Settings.UsePin, out bool shouldUsePin);
            UsePin = shouldUsePin;
            Pin = Settings.UserPinCode;

            if ((ThemeTypes)Enum.Parse(typeof(ThemeTypes), Settings.ThemeType) == ThemeTypes.Dark)
            {
                IsDarkTheme = true;
            }
            SaveSettingsCommand = commandResolver.Command<string>(pin => SaveSettings(pin));
        }

        public bool ModelChanged
        {
            get
            {
                bool.TryParse(Settings.UsePin, out bool shouldUsePin);
                return !(shouldUsePin == UsePin
                    && Settings.UserPinCode == Pin
                    && _savedTheme == _themeSwitcher.CurrentThemeType);
            }
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
                    _themeService.SetStatusBarColor((Color)Application.Current.Resources["StatusBarColor"]);
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveSettingsCommand { get; set; }

        public void OnDisappearing()
        {
            Settings.ThemeType = _savedTheme.ToString();
            _themeSwitcher.SwitchTheme(_savedTheme);
            _themeService.SetStatusBarColor((Color)Application.Current.Resources["StatusBarColor"]);
        }

        private void SaveSettings(string pinCode)
        {
            Settings.UserPinCode = int.TryParse(pinCode, out var pin) ? pinCode : "1111";
            Pin = pinCode;
            Settings.UsePin = UsePin.ToString();
            Settings.ThemeType = _themeSwitcher.CurrentThemeType.ToString();
            _savedTheme = (ThemeTypes)Enum.Parse(typeof(ThemeTypes), Settings.ThemeType);
            OnPropertyChanged();
        }
    }
}