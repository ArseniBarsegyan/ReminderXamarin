using System;
using System.Threading.Tasks;

using ReminderXamarin.Extensions;
using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Entry_OnUnfocused(UserNameEntry, new FocusEventArgs(UserNameEntry, false));
            Entry_OnUnfocused(PasswordEntry, new FocusEventArgs(PasswordEntry, false));
            Entry_OnUnfocused(ConfirmPasswordEntry, new FocusEventArgs(ConfirmPasswordEntry, false));
            await RegisterLayout.TranslateTo(0, 100, 300);
        }

        private void Entry_OnCompleted(object sender, EventArgs e)
        {
            if (sender == UserNameEntry)
            {
                PasswordEntry.Focus();
            }
        }

        private async void Entry_OnFocused(object sender, FocusEventArgs e)
        {
            var item = e.VisualElement;

            if (item == UserNameEntry && string.IsNullOrWhiteSpace(UserNameEntry.Text))
            {
                await TransitionToTitle(UserNameLabel, true);
            }
            else if (item == PasswordEntry && string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await TransitionToTitle(PasswordLabel, true);
            }
            else if (item == ConfirmPasswordEntry && string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
            {
                await TransitionToTitle(ConfirmPasswordLabel, true);
            }
        }

        private async void Entry_OnUnfocused(object sender, FocusEventArgs e)
        {
            var item = e.VisualElement;

            if (item == UserNameEntry && string.IsNullOrWhiteSpace(UserNameEntry.Text))
            {
                await TransitionToPlaceholder(UserNameLabel, true);
            }
            else if (item == PasswordEntry && string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await TransitionToPlaceholder(PasswordLabel, true);
            }
            else if (item == ConfirmPasswordEntry && string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
            {
                await TransitionToPlaceholder(ConfirmPasswordLabel, true);
            }
        }
        
        private async Task TransitionToTitle(Label target, bool animated)
        {
            if (animated)
            {
                var t1 = target.TranslateTo(0,0, 100);
                target.FontSize = Device.GetNamedSize(NamedSize.Small, target);
                await Task.WhenAll(t1);
            }
        }

        private async Task TransitionToPlaceholder(Label target, bool animated)
        {
            if (animated)
            {
                var t1 = target.TranslateTo(10,30, 100);
                target.FontSize = 16;
                await Task.WhenAll(t1);
            }
        }

        private async void ToggleRegisterOrLoginButton_OnClicked(object sender, EventArgs e)
        {
            if (BindingContext is LoginViewModel viewModel)
            {
                if (viewModel.IsRegister)
                {
                    await SignInButton.ColorTo(Color.FromHex("#3072B0"), 
                        Color.FromHex("#80B76D"), 
                        c => SignInButton.BackgroundColor = c, 
                        100);
                }
                else
                {
                    await SignInButton.ColorTo(Color.FromHex("#80B76D"), 
                        Color.FromHex("#3072B0"), 
                        c => SignInButton.BackgroundColor = c, 
                        100);
                }
            }
        }
    }
}