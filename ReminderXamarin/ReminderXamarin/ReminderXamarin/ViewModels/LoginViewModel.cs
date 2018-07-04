using System.Windows.Input;
using ReminderXamarin.Helpers;
using ReminderXamarin.Pages;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            LoginCommand = new Command(LoginCommandExecute);
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsValid { get; set; }

        public ICommand LoginCommand { get; set; }

        private void LoginCommandExecute()
        {
            if (AuthenticationManager.Authenticate(UserName, Password))
            {
                Application.Current.MainPage = new NavigationPage(new MenuPage());
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }
    }
}