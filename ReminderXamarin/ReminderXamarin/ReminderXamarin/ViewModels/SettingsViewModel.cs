using System;
using System.Threading.Tasks;
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

            if ((ThemeTypes)Enum.Parse(typeof(ThemeTypes), Settings.ThemeType) == ThemeTypes.Dark)
            {
                IsDarkTheme = true;
            }
            SaveSettingsCommand = commandResolver.Command(SaveSettings);
            OpenPinViewCommand = commandResolver.AsyncCommand(OpenPinViewAsync);
        }

        public bool ModelChanged
        {
            get
            {
                bool.TryParse(Settings.UsePin, out bool shouldUsePin);
                return !(shouldUsePin == UsePin
                    && _savedTheme == _themeSwitcher.CurrentThemeType);
            }
        }

        public bool UsePin { get; set; }
        public bool UseSafeMode { get; set; }

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (value != _isDarkTheme)
                {
                    _isDarkTheme = value;
                    _themeSwitcher.SwitchTheme(value ? ThemeTypes.Dark : ThemeTypes.Light);
                    _themeService.SetStatusBarColor((Color)Application.Current.Resources["StatusBar"]);
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveSettingsCommand { get; private set; }
        public ICommand OpenPinViewCommand { get; private set; }

        public void OnDisappearing()
        {
            Settings.ThemeType = _savedTheme.ToString();
            _themeSwitcher.SwitchTheme(_savedTheme);
            _themeService.SetStatusBarColor((Color)Application.Current.Resources["StatusBar"]);
        }

        private async Task OpenPinViewAsync()
        {
            if (UsePin)
            {
                await NavigationService.NavigateToAsync<PinViewModel>(true);
            }            
        }

        private void SaveSettings()
        {
            Settings.UsePin = UsePin.ToString();
            Settings.ThemeType = _themeSwitcher.CurrentThemeType.ToString();
            Settings.UseSafeMode = UseSafeMode.ToString();
            _savedTheme = (ThemeTypes)Enum.Parse(typeof(ThemeTypes), Settings.ThemeType);
            OnPropertyChanged();
        }
    }
}