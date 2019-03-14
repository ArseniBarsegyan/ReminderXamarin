using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Data.Entities;
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
            if (Password != ConfirmPassword)
            {   
                IsValid = false;
            }
            else
            {
                var authResult = await Register(UserName, Password);
                if (authResult)
                {
                    if (await Authenticate(UserName, Password))
                    {
                        Settings.ApplicationUser = UserName;
                        await Task.Delay(50);
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

        private static async Task<bool> Register(string userName, string password)
        {
            var user = UserRepository.Value.GetAll().FirstOrDefault(x => x.UserName == userName);
            if (user != null)
            {
                return false;
            }
            var passwordBytes = Encoding.Unicode.GetBytes(password);
            var passwordHash = SHA256.Create().ComputeHash(passwordBytes);

            var userModel = new AppUser
            {
                UserName = userName,
                ImageContent = new byte[0],
                Password = passwordHash
            };
            await UserRepository.Value.CreateAsync(userModel);
            await UserRepository.Value.SaveAsync();
            return true;
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