using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Helpers;
using ReminderXamarin.Interfaces;
using ReminderXamarin.ViewModels.Base;
using ReminderXamarin.Views;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class PinViewModel : BaseViewModel
    {
        private static int _currentCount;
        private readonly StringBuilder _pinBuilder;

        public PinViewModel()
        {
            FirstNumberImageSource = new Image { Source = ConstantsHelper.EmptyDotImage };
            SecondNumberImageSource = new Image { Source = ConstantsHelper.EmptyDotImage };
            ThirdNumberImageSource = new Image { Source = ConstantsHelper.EmptyDotImage };
            FourthNumberImageSource = new Image { Source = ConstantsHelper.EmptyDotImage };

            DeleteNumberCommand = new Command(DeletePinNumber);
            PinCommand = new Command<string>(async pin => await CheckPin(pin));
            LoginCommand = new Command(async task => await Login());
            _pinBuilder = new StringBuilder();
        }

        public Image FirstNumberImageSource { get; }
        public Image SecondNumberImageSource { get; }
        public Image ThirdNumberImageSource { get; }
        public Image FourthNumberImageSource { get; }

        public int Pin { get; set; }

        public ICommand DeleteNumberCommand { get; set; }
        public ICommand PinCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        private async Task CheckPin(string text)
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
                int.TryParse(_pinBuilder.ToString(), out int pin);
                Pin = pin;
                await Task.Delay(250);
                ResetImagesAndCount();
                await Login();
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
                // Application.Current.MainPage = new NavigationPage(new MenuView(Settings.ApplicationUser));
                await NavigationService.InitializeAsync<MenuViewModel>();
            }
        }
    }
}