using System.Threading.Tasks;
using System.Windows.Input;

using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            SignInCommand = new Command(async() => await SignIn());
            SwitchPasswordVisibilityCommand = new Command(SwitchPasswordVisibility);
            ToggleRegisterOrLoginViewCommand = new Command(async() => await ToggleRegisterOrLoginView());
            SwitchPasswordConfirmVisibilityCommand = new Command(SwitchConfirmPasswordVisibility);
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

        public ICommand NavigateToLoginViewCommand { get; }
        public ICommand SwitchPasswordVisibilityCommand { get; }
        public ICommand SwitchPasswordConfirmVisibilityCommand { get; }
        public ICommand ToggleRegisterOrLoginViewCommand { get; }
        public ICommand SignInCommand { get; }

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
            if (await AuthenticationManager.Authenticate(UserName, Password).ConfigureAwait(false))
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
                var authResult = await AuthenticationManager.Register(UserName, Password).ConfigureAwait(false);
                if (authResult)
                {
                    if (await AuthenticationManager.Authenticate(UserName, Password).ConfigureAwait(false))
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

        private void SwitchConfirmPasswordVisibility()
        {
            ShowConfirmedPassword = !ShowConfirmedPassword;
        }
    }
}