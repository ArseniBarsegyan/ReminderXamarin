﻿using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.Utilities;
using ReminderXamarin.ViewModels.Base;

using Rm.Helpers;

using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ThemeSwitcher _themeSwitcher;

        public LoginViewModel(INavigationService navigationService,
            ThemeSwitcher themeSwitcher,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _themeSwitcher = themeSwitcher;
            TogglePasswordImageSource = _themeSwitcher.CurrentThemeType == ThemeTypes.Dark
                        ? ConstantsHelper.TogglePasswordLightImage
                        : ConstantsHelper.TogglePasswordDarkImage;

            SignInCommand = commandResolver.AsyncCommand(SignIn);
            SwitchPasswordVisibilityCommand = commandResolver.Command(SwitchPasswordVisibility);
            ToggleRegisterOrLoginViewCommand = commandResolver.Command(ToggleRegisterOrLoginView);
            SwitchPasswordConfirmVisibilityCommand = commandResolver.Command(SwitchConfirmPasswordVisibility);
        }

        public ImageSource TogglePasswordImageSource { get; set; }
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

        public ICommand SwitchPasswordVisibilityCommand { get; }
        public ICommand SwitchPasswordConfirmVisibilityCommand { get; }
        public ICommand ToggleRegisterOrLoginViewCommand { get; }
        public IAsyncCommand SignInCommand { get; }

        private async Task SignIn()
        {
            if (IsRegister)
            {
                await Register().ConfigureAwait(false);
            }
            else
            {
                await Login().ConfigureAwait(false);
            }
        }

        private async Task Login()
        {
            if (await AuthenticationManager.Authenticate(UserName, Password).ConfigureAwait(false))
            {
                Settings.ApplicationUser = UserName;
                await NavigationService.InitializeAsync<MenuViewModel>().ConfigureAwait(false);
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
                        await NavigationService.InitializeAsync<MenuViewModel>().ConfigureAwait(false);
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

        private void ToggleRegisterOrLoginView()
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