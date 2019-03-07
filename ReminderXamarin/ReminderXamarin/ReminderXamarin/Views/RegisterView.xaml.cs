using System;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterView : ContentPage
    {
        public RegisterView()
        {
            InitializeComponent();
            BackgroundImage.Source = ImageSource.FromResource(ConstantsHelper.BackgroundImageSource);
        }

        private void UserNameEntry_OnCompleted(object sender, EventArgs e)
        {
            PasswordEntry.Focus();
        }

        private void PasswordEntry_OnCompleted(object sender, EventArgs e)
        {
            ConfirmPasswordEntry.Focus();
        }

        private void SubmitButton_OnClicked(object sender, EventArgs e)
        {
            if (this.BindingContext is RegisterViewModel viewModel)
            {
                viewModel.UserName = UserNameEntry.Text;
                viewModel.Password = PasswordEntry.Text;
                viewModel.ConfirmPassword = ConfirmPasswordEntry.Text;
                
                viewModel.RegisterCommand.Execute(null);
            }
        }
    }
}