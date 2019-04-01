using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel()
        {
            NavigateToLoginViewCommand = new Command(async () => await NavigateToLoginView());
            RegisterCommand = new Command(async () => await RegisterCommandExecute());
            SwitchPasswordVisibilityCommand = new Command(SwitchPasswordVisibilityCommandExecute);
            SwitchPasswordConfirmVisibilityCommand = new Command(SwitchConfirmPasswordVisibilityCommandExecute);
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool ShowPassword { get; set; }
        public bool ShowConfirmedPassword { get; set; }
        public bool IsValid { get; set; } = true;

        public ICommand NavigateToLoginViewCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand SwitchPasswordVisibilityCommand { get; set; }
        public ICommand SwitchPasswordConfirmVisibilityCommand { get; set; }

        public async Task RegisterCommandExecute()
        {
            if (UserName == null || Password == null || ConfirmPassword == null)
            {
                IsValid = false;
                return;
            }

            if (Password != ConfirmPassword)
            {   
                IsValid = false;
            }
            else
            {
                var authResult = await AuthenticationManager.Register(UserName, Password);
                if (authResult)
                {
                    if (await AuthenticationManager.Authenticate(UserName, Password))
                    {
                        Settings.ApplicationUser = UserName;
                        await NavigationService.InitializeAsync<MenuViewModel>();
                        IsValid = true;
                    }
                    else
                    {
                        IsValid = false;
                    }
                }
                else
                {
                    IsValid = false;
                }
            }
        }

        private async Task NavigateToLoginView()
        {
            await NavigationService.InitializeAsync<LoginViewModel>();
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