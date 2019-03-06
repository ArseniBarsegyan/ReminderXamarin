using System;
using ReminderXamarin.Helpers;
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
            ViewModel.UserName = UserNameEntry.Text;
            ViewModel.Password = PasswordEntry.Text;
            ViewModel.ConfirmPassword = ConfirmPasswordEntry.Text;

            ViewModel.RegisterCommand.Execute(null);
        }

        private void Login_OnTapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginView();
        }
    }
}