using System.Windows.Input;
using ReminderXamarin.Helpers;
using ReminderXamarin.Pages;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel()
        {
            RegisterCommand = new Command(RegisterCommandExecute);
            SwitchPasswordVisibilityCommand = new Command(SwitchPasswordVisibilityCommandExecute);
            SwitchPasswordConfirmVisibilityCommand = new Command(SwitchConfirmPasswordVisibilityCommandExecute);
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool ShowPassword { get; set; }
        public bool ShowConfirmedPassword { get; set; }
        public bool IsValid { get; set; } = true;

        public ICommand RegisterCommand { get; set; }
        public ICommand SwitchPasswordVisibilityCommand { get; set; }
        public ICommand SwitchPasswordConfirmVisibilityCommand { get; set; }

        public void RegisterCommandExecute()
        {
            if (Password != ConfirmPassword)
            {
                // AlertService.DisplayAlert("");
                IsValid = false;
            }
            else
            {
                var authResult = AuthenticationManager.Register(UserName, Password);
                if (authResult)
                {
                    if (AuthenticationManager.Authenticate(UserName, Password))
                    {
                        Settings.ApplicationUser = UserName;
                        Application.Current.MainPage = new NavigationPage(new MenuPage(UserName));
                        IsValid = true;
                    }
                    else
                    {
                        IsValid = false;
                    }
                }
                else
                {
                    // AlertService.DisplayAlert("");
                    IsValid = false;
                }
            }
        }

        private void SwitchPasswordVisibilityCommandExecute()
        {
            ShowPassword = !ShowPassword;
        }

        private void SwitchConfirmPasswordVisibilityCommandExecute()
        {
            ShowConfirmedPassword = !ShowConfirmedPassword;
        }
    }
}