using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using PropertyChanged;
using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.Utilities;
using ReminderXamarin.ViewModels.Base;

using Rm.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class SettingsViewModel : BaseNavigableViewModel
    {
        private readonly IFileSystem _fileService;
        private readonly IMediaService _mediaService;
        private readonly IThemeService _themeService;
        private readonly ThemeSwitcher _themeSwitcher;
        private readonly IPlatformDocumentPicker _documentPicker;
        private ThemeTypes _savedTheme;
        private bool _isDarkTheme;
        private bool _usePinPageBackground;

        public SettingsViewModel(
            INavigationService navigationService,
            IFileSystem fileService,
            IMediaService mediaService,
            ThemeSwitcher themeSwitcher,
            IThemeService themeService,
            IPlatformDocumentPicker documentPicker,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _fileService = fileService;
            _mediaService = mediaService;
            _themeService = themeService;
            _themeSwitcher = themeSwitcher;
            _documentPicker = documentPicker;

            _savedTheme = (ThemeTypes)Enum.Parse(typeof(ThemeTypes), Settings.ThemeType);

            bool.TryParse(Settings.UsePin, out bool shouldUsePin);
            UsePin = shouldUsePin;

            if ((ThemeTypes)Enum.Parse(typeof(ThemeTypes), Settings.ThemeType) == ThemeTypes.Dark)
            {
                IsDarkTheme = true;
            }

            bool.TryParse(Settings.UsePinBackground, out bool shouldUsePinBackground);
            UsePinPageBackground = shouldUsePinBackground;
            
            if (UsePinPageBackground)
            {
                string[] bytesStr = Settings.PinBackground.Split('x');
                byte[] bytes = bytesStr.Select(x => (byte)int.Parse(x)).ToArray();
                PinBackgroundImageContent = bytes;
            }
            
            SaveSettingsCommand = commandResolver.Command(SaveSettings);
            OpenPinViewCommand = commandResolver.AsyncCommand(OpenPinViewAsync);
            ChangePinViewBackgroundCommand = commandResolver.AsyncCommand(ChangePinBackground);
            ResetPinBackgroundCommand = commandResolver.Command(ResetPinBackground);
        }

        public bool ModelChanged
        {
            get
            {
                bool.TryParse(Settings.UsePin, out bool shouldUsePin);
                bool.TryParse(Settings.UsePinBackground, out bool usePinBackground);
                return !(shouldUsePin == UsePin
                    && _savedTheme == _themeSwitcher.CurrentThemeType
                    && UsePinPageBackground == usePinBackground);
            }
        }

        public bool UsePin { get; set; }
        public bool UseSafeMode { get; set; }

        public bool UsePinPageBackground
        {
            get => _usePinPageBackground;
            set
            {
                _usePinPageBackground = value;
                
                if (!_usePinPageBackground)
                {
                    ResetPinBackground();
                }
                
                OnPropertyChanged();
            }
        }
        
        [AlsoNotifyFor(nameof(PinBackgroundImageSource))]
        public byte[] PinBackgroundImageContent { get; set; }
        public ImageSource PinBackgroundImageSource { get; private set; }

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

        public ICommand SaveSettingsCommand { get; }
        public ICommand OpenPinViewCommand { get; }
        public ICommand ChangePinViewBackgroundCommand { get; }
        public ICommand ResetPinBackgroundCommand { get; }

        public void OnDisappearing()
        {
            Settings.ThemeType = _savedTheme.ToString();
            _themeSwitcher.SwitchTheme(_savedTheme);
            _themeService.SetStatusBarColor((Color)Application.Current.Resources["StatusBar"]);
        }

        public override Task InitializeAsync(object navigationData)
        {
            UpdatePinPhoto();
            return base.InitializeAsync(navigationData);
        }

        private void UpdatePinPhoto()
        {
            if (PinBackgroundImageContent == null || PinBackgroundImageContent.Length == 0)
            {
                PinBackgroundImageSource = ImageSource.FromResource(
                    ConstantsHelper.NoPhotoImage);
            }
            else
            {
                PinBackgroundImageSource = ImageSource.FromStream(
                    () => new MemoryStream(PinBackgroundImageContent));
            }
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
            if (PinBackgroundImageContent == null || PinBackgroundImageContent.Length == 0)
            {
                UsePinPageBackground = false;
            }
            Settings.UsePinBackground = UsePinPageBackground.ToString();

            if (UsePinPageBackground)
            {
                string serialized = string.Join("x", PinBackgroundImageContent);
                Settings.PinBackground = serialized;
            }
            
            _savedTheme = (ThemeTypes)Enum.Parse(typeof(ThemeTypes), Settings.ThemeType);
            OnPropertyChanged();
        }
        
        private async Task ChangePinBackground()
        {
            if (!UsePinPageBackground)
            {
                return;
            }
            
            var document = await _documentPicker.DisplayImportAsync();
            if (document == null)
            {
                UsePinPageBackground = false;
                return;
            }

            // Ensure that user downloads .png or .jpg file as profile icon.
            if (document.Name.EndsWith(".png") 
                || document.Name.EndsWith(".jpg") 
                || document.Name.EndsWith(".jpeg"))
            {
                try
                {
                    UsePinPageBackground = true;
                    await Task.Run(() =>
                    {
                        var imageContent = _fileService.ReadAllBytes(document.Path);
                        var resizedImage = _mediaService.ResizeImage(
                            imageContent,
                            ConstantsHelper.ResizedImageWidth,
                            ConstantsHelper.ResizedImageHeight);

                        PinBackgroundImageContent = resizedImage;
                    }).ConfigureAwait(false);
                    
                    Device.BeginInvokeOnMainThread(UpdatePinPhoto);
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(ex.Message);
                }
            }
        }

        private void ResetPinBackground()
        {
            UsePinPageBackground = false;
            PinBackgroundImageContent = new byte[0];
            UpdatePinPhoto();
        }
    }
}