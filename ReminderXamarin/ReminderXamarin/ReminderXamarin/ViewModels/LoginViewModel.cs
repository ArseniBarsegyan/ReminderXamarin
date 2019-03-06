﻿using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels.Base;
using ReminderXamarin.Views;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginCommandExecute());
            SwitchPasswordVisibilityCommand = new Command(SwitchPasswordVisibilityCommandExecute);
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

        private async Task LoginCommandExecute()
        {
            if (await AuthenticationManager.Authenticate(UserName, Password))
            {
                Settings.ApplicationUser = UserName;
                Application.Current.MainPage = new NavigationPage(new MenuView(UserName));
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        private void SwitchPasswordVisibilityCommandExecute()
        {
            ShowPassword = !ShowPassword;
        }
    }
}