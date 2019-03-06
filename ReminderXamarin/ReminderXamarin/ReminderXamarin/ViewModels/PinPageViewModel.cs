using System.Windows.Input;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels.Base;
using ReminderXamarin.Views;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class PinViewViewModel : BaseViewModel
    {
        public PinViewViewModel()
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
                Application.Current.MainPage = new NavigationPage(new MenuView(Settings.ApplicationUser));
            }
        }
    }
}