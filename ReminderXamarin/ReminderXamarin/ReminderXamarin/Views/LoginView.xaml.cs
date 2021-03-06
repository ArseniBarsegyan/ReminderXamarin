﻿using System;
using System.Threading.Tasks;

using ReminderXamarin.Extensions;
using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            EntryOnUnfocused(UserNameEntry, new FocusEventArgs(UserNameEntry, false));
            EntryOnUnfocused(PasswordEntry, new FocusEventArgs(PasswordEntry, false));
            EntryOnUnfocused(ConfirmPasswordEntry, new FocusEventArgs(ConfirmPasswordEntry, false));
            await RegisterLayout.TranslateTo(0, 100, 300);
        }

        private void EntryOnCompleted(object sender, EventArgs e)
        {
            if (sender == UserNameEntry)
            {
                PasswordEntry.Focus();
            }
            if (sender == PasswordEntry)
            {
                if (ConfirmPasswordEntry.IsVisible)
                {
                    ConfirmPasswordEntry.Focus();
                }
            }
        }

        private async void EntryOnFocused(object sender, FocusEventArgs e)
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

        private async void EntryOnUnfocused(object sender, FocusEventArgs e)
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

        private async void ToggleRegisterOrLoginButtonOnClicked(object sender, EventArgs e)
        {
            if (BindingContext is LoginViewModel viewModel)
            {
                if (viewModel.IsRegister)
                {
                    await SignInButton.ColorTo(
                        (Color)Application.Current.Resources["LoginButtonBackground"],
                        (Color)Application.Current.Resources["RegisterButtonBackground"],
                        c => SignInButton.BackgroundColor = c,
                        100);
                }
                else
                {
                    await SignInButton.ColorTo(
                        (Color)Application.Current.Resources["RegisterButtonBackground"],
                        (Color)Application.Current.Resources["LoginButtonBackground"],
                        c => SignInButton.BackgroundColor = c,
                        100);
                }
            }
        }
    }
}