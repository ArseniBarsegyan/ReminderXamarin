using System;
using System.Text;
using ReminderXamarin.Helpers;
using ReminderXamarin.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinPage : ContentPage
    {
        private static int _currentCount;
        private readonly StringBuilder _pinBuilder;

        public PinPage()
        {
            InitializeComponent();
            _pinBuilder = new StringBuilder();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            _currentCount++;

            if (sender is Button button)
            {
                switch (_currentCount)
                {
                    case 1:
                        FirstNumber.Source = ConstantHelper.FilledDotImage;
                        break;
                    case 2:
                        FirstNumber.Source = ConstantHelper.FilledDotImage;
                        SecondNumber.Source = ConstantHelper.FilledDotImage;
                        break;
                    case 3:
                        FirstNumber.Source = ConstantHelper.FilledDotImage;
                        SecondNumber.Source = ConstantHelper.FilledDotImage;
                        ThirdNumber.Source = ConstantHelper.FilledDotImage;
                        break;
                    case 4:
                        FirstNumber.Source = ConstantHelper.FilledDotImage;
                        SecondNumber.Source = ConstantHelper.FilledDotImage;
                        ThirdNumber.Source = ConstantHelper.FilledDotImage;
                        FourthNumber.Source = ConstantHelper.FilledDotImage;
                        break;
                }

                if (button.Text == "x")
                {
                    RemoveNumber();
                }
                else
                {
                    int.TryParse(button.Text, out var number);
                    _pinBuilder.Append(number);

                    if (_currentCount == 4)
                    {
                        int.TryParse(_pinBuilder.ToString(), out int pin);
                        ViewModel.Pin = pin;
                        ResetImagesAndCount();
                        ViewModel.LoginCommand.Execute(null);
                    }
                }
            }
        }

        private void ResetImagesAndCount()
        {
            _pinBuilder.Length = 0;
            _currentCount = 0;

            FirstNumber.Source = ConstantHelper.EmptyDotImage;
            SecondNumber.Source = ConstantHelper.EmptyDotImage;
            ThirdNumber.Source = ConstantHelper.EmptyDotImage;
            FourthNumber.Source = ConstantHelper.EmptyDotImage;
        }

        private void RemoveNumber()
        {
            if (_pinBuilder.Length > 0)
            {
                _pinBuilder.Length--;
            }

            switch (_currentCount)
            {
                case 1:
                    FirstNumber.Source = ConstantHelper.EmptyDotImage;
                    break;
                case 2:
                    SecondNumber.Source = ConstantHelper.EmptyDotImage;
                    break;
                case 3:
                    ThirdNumber.Source = ConstantHelper.EmptyDotImage;
                    break;
                case 4:
                    FourthNumber.Source = ConstantHelper.EmptyDotImage;
                    break;
            }

            _currentCount--;
        }

        private void Exit_OnTapped(object sender, EventArgs e)
        {
            var applicationService = DependencyService.Get<IApplicationService>();
            applicationService.CloseApplication();
        }
    }
}