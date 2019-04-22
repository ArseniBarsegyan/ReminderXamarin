using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.LocalNotifications;
using Rm.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login());
            SwitchPasswordVisibilityCommand = new Command(SwitchPasswordVisibility);
            NavigateToRegisterPageCommand = new Command(async() => await NavigateToRegisterPage());
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool ShowPassword { get; set; }

        // Set this field to true to hide error message at LoginView.
        // When user will press "Login" button and get error change this property
        // will show error at LoginView.
        public bool IsValid { get; set; } = true;

        public ICommand LoginCommand { get; set; }
        public ICommand SwitchPasswordVisibilityCommand { get; set; }
        public ICommand NavigateToRegisterPageCommand { get; set; }

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

        private async Task NavigateToRegisterPage()
        {            
            await NavigationService.InitializeAsync<RegisterViewModel>();
        }

        private void SwitchPasswordVisibility()
        {
            ShowPassword = !ShowPassword;
        }
    }
}