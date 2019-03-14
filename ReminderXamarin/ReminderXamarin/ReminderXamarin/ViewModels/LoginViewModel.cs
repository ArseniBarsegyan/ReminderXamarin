using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Helpers;
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
            if (await Authenticate(UserName, Password))
            {
                Settings.ApplicationUser = UserName;
                await Task.Delay(50);
                // Application.Current.MainPage = new NavigationPage(new MenuView(UserName));
                await NavigationService.InitializeAsync<MenuViewModel>();
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        private static async Task<bool> Authenticate(string userName, string password)
        {
            var user = (await UserRepository.Value.GetAsync(x => x.UserName == userName)).FirstOrDefault();
            // var user = UserRepository.Value.GetAllAsync().FirstOrDefault(x => x.UserName == userName);
            if (user == null)
            {
                return false;
            }
            var passwordBytes = Encoding.Unicode.GetBytes(password);
            var passwordHash = SHA256.Create().ComputeHash(passwordBytes);

            if (userName == user.UserName && passwordHash.SequenceEqual(user.Password))
            {
                return true;
            }
            return false;
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