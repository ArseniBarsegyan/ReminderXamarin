using System.Threading.Tasks;
using System.Windows.Input;
using Rm.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            SignInCommand = new Command(async() => await SignIn());
            SwitchPasswordVisibilityCommand = new Command(SwitchPasswordVisibility);
            ToggleRegisterOrLoginViewCommand = new Command(async() => await ToggleRegisterOrLoginView());
            SwitchPasswordConfirmVisibilityCommand = new Command(SwitchConfirmPasswordVisibilityCommandExecute);
        }

        public bool IsRegister { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool ShowPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public bool ShowConfirmedPassword { get; set; }

        // Set this field to true to hide error message at LoginView.
        // When user will press "Login" button and get error change this property
        // will show error at LoginView.
        public bool IsValid { get; set; } = true;

        public ICommand NavigateToLoginViewCommand { get; set; }
        public ICommand SwitchPasswordVisibilityCommand { get; set; }
        public ICommand SwitchPasswordConfirmVisibilityCommand { get; set; }
        public ICommand ToggleRegisterOrLoginViewCommand { get; set; }
        public ICommand SignInCommand { get; set; }

        private async Task SignIn()
        {
            if (IsRegister)
            {
                await Register();
            }
            else
            {
                await Login();
            }
        }

        private async Task Login()
        {
            if (await AuthenticationManager.Authenticate(UserName, Password))
            {
                Settings.ApplicationUser = UserName;
                // Application.Current.MainPage = new NavigationPage(new MenuView(UserName));
                await NavigationService.InitializeAsync<MenuViewModel>();
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        private async Task Register()
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

        private async Task ToggleRegisterOrLoginView()
        {
            IsRegister = !IsRegister;
        }

        private void SwitchPasswordVisibility()
        {
            ShowPassword = !ShowPassword;
        }

        private void SwitchConfirmPasswordVisibilityCommandExecute()
        {
            ShowConfirmedPassword = !ShowConfirmedPassword;
        }
    }
}