using System;
using System.Windows.Input;
using ReminderXamarin.Helpers;
using ReminderXamarin.Interfaces;
using ReminderXamarin.Pages;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class PinPageViewModel : BaseViewModel
    {
        public PinPageViewModel()
        {
            LoginCommand = new Command(LoginCommandExecute);
        }

        public int Pin { get; set; }

        public ICommand LoginCommand { get; set; }

        private void LoginCommandExecute()
        {
            var userPin = Settings.UserPinCode;
            if (Pin.ToString() == userPin)
            {
                var loadingService = DependencyService.Get<ILoadingService>();
                loadingService.ShowLoading();
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    Application.Current.MainPage = new NavigationPage(new MenuPage(Settings.ApplicationUser));
                    loadingService.HideLoading();
                    return false;
                });
            }
        }
    }
}