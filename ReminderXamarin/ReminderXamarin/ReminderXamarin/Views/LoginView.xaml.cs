using System;
using ReminderXamarin.Helpers;
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
            BackgroundImage.Source = ImageSource.FromResource(ConstantsHelper.BackgroundImageSource);
        }

        private void UserNameEntry_OnCompleted(object sender, EventArgs e)
        {
            PasswordEntry.Focus();
        }
    }
}