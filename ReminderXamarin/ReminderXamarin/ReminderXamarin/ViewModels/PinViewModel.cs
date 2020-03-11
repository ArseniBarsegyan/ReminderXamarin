using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.Utilities;
using ReminderXamarin.ViewModels.Base;

using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class PinViewModel : BaseViewModel
    {
        private static int _currentCount;
        private readonly StringBuilder _pinBuilder;
        private readonly ThemeSwitcher _themeSwitcher;
        private bool _shouldConfirmPin;
        private int _pinToConfirm;

        public PinViewModel(INavigationService navigationService,
            ThemeSwitcher themeSwitcher,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _pinBuilder = new StringBuilder();
            _themeSwitcher = themeSwitcher;
            Message = Resmgr.Value.GetString(ConstantsHelper.EnterPin, CultureInfo.CurrentCulture);

            DeleteNumberCommand = commandResolver.Command(DeletePinNumber);
            PinCommand = commandResolver.AsyncCommand<string>(CheckPinAsync);

            InitializeImagesForButtons();
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData != null)
            {
                _shouldConfirmPin = (bool)navigationData;
            }
            return base.InitializeAsync(navigationData);            
        }

        public Image FirstNumberImageSource { get; private set; }
        public Image SecondNumberImageSource { get; private set; }
        public Image ThirdNumberImageSource { get; private set; }
        public Image FourthNumberImageSource { get; private set; }
        public ImageSource DeleteButtonImageSource { get; private set; }

        public int Pin { get; set; }
        public string Message { get; private set; }

        public ICommand DeleteNumberCommand { get; }
        public IAsyncCommand SetPin { get; }
        public IAsyncCommand<string> PinCommand { get; }

        private void InitializeImagesForButtons()
        {
            FirstNumberImageSource = new Image { Source = ConstantsHelper.EmptyDotImage };
            SecondNumberImageSource = new Image { Source = ConstantsHelper.EmptyDotImage };
            ThirdNumberImageSource = new Image { Source = ConstantsHelper.EmptyDotImage };
            FourthNumberImageSource = new Image { Source = ConstantsHelper.EmptyDotImage };

            var currentTheme = _themeSwitcher.GetThemeType();

            DeleteButtonImageSource = currentTheme == ThemeTypes.Dark ?
                ConstantsHelper.PinButtonDarkDeleteImageSource :
                ConstantsHelper.PinButtonLightDeleteImageSource;
        }

        private async Task CheckPinAsync(string text)
        {
            _currentCount++;

            switch (_currentCount)
            {
                case 1:
                    FirstNumberImageSource.Source = ConstantsHelper.FilledDotImage;
                    break;
                case 2:
                    SecondNumberImageSource.Source = ConstantsHelper.FilledDotImage;
                    break;
                case 3:
                    ThirdNumberImageSource.Source = ConstantsHelper.FilledDotImage;
                    break;
                case 4:
                    FourthNumberImageSource.Source = ConstantsHelper.FilledDotImage;
                    break;
            }

            int.TryParse(text, out var number);
            _pinBuilder.Append(number);

            if (_currentCount == 4)
            {
                if (_shouldConfirmPin)
                {
                    if (_pinToConfirm != 0)
                    {
                        int.TryParse(_pinBuilder.ToString(), out int confirmedPin);
                        if (confirmedPin == _pinToConfirm)
                        {
                            Settings.UserPinCode = confirmedPin.ToString();
                            await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Pin saved", "Success", "Ok");
                            await Task.Delay(200);
                            await NavigationService.NavigateBackAsync();
                        }
                        else
                        {
                            await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Pin doesn't match entered", "Error", "Ok");
                            await Task.Delay(200);
                            await NavigationService.NavigateBackAsync();
                        }
                    }
                    else
                    {
                        int.TryParse(_pinBuilder.ToString(), out _pinToConfirm);
                        Message = Resmgr.Value.GetString(ConstantsHelper.ConfirmPin, CultureInfo.CurrentCulture);
                        ResetImagesAndCount();
                    }                    
                }
                else
                {
                    int.TryParse(_pinBuilder.ToString(), out int pin);
                    Pin = pin;
                    await Task.Delay(25);
                    ResetImagesAndCount();
                    await Login().ConfigureAwait(false);
                }                
            }
        }

        private void DeletePinNumber()
        {
            if (_pinBuilder.Length > 0)
            {
                _pinBuilder.Length--;
            }

            switch (_currentCount)
            {
                case 1:
                    FirstNumberImageSource.Source = ConstantsHelper.EmptyDotImage;
                    break;
                case 2:
                    SecondNumberImageSource.Source = ConstantsHelper.EmptyDotImage;
                    break;
                case 3:
                    ThirdNumberImageSource.Source = ConstantsHelper.EmptyDotImage;
                    break;
                case 4:
                    FourthNumberImageSource.Source = ConstantsHelper.EmptyDotImage;
                    break;
            }

            if (_currentCount > 0)
            {
                _currentCount--;
            }
        }

        private void ResetImagesAndCount()
        {
            _pinBuilder.Length = 0;
            _currentCount = 0;

            FirstNumberImageSource.Source = ConstantsHelper.EmptyDotImage;
            SecondNumberImageSource.Source = ConstantsHelper.EmptyDotImage;
            ThirdNumberImageSource.Source = ConstantsHelper.EmptyDotImage;
            FourthNumberImageSource.Source = ConstantsHelper.EmptyDotImage;
        }

        private async Task Login()
        {
            var userPin = Settings.UserPinCode;
            if (Pin.ToString() == userPin)
            {
                await NavigationService.ToRootAsync<MenuViewModel>();
            }
        }
    }
}