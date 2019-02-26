using System;
using ReminderXamarin.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundImage.Source = ImageSource.FromResource(ConstantsHelper.BackgroundImageSource);
        }

        private void UserNameEntry_OnCompleted(object sender, EventArgs e)
        {
            PasswordEntry.Focus();
        }

        private void RegisterLink_OnTapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegisterPage();
        }
    }
}