using System;
using System.Linq;
using ReminderXamarin.Extensions;
using ReminderXamarin.Interfaces.FilePickerService;
using ReminderXamarin.ViewModels;
using ReminderXamarin.Views;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfilePage : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();

        public UserProfilePage(string username)
        {
            InitializeComponent();
            var appUser = App.UserRepository.GetAll().FirstOrDefault(x => x.UserName == username);
            if (appUser != null)
            {
                var viewModel = appUser.ToUserProfileViewModel();
                BindingContext = viewModel;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as UserProfileViewModel;
            viewModel?.OnAppearing();
        }

        private async void EditUserProfilePhoto_OnTapped(object sender, EventArgs e)
        {
            var document = await DocumentPicker.DisplayImportAsync(this);
            if (document == null)
            {
                return;
            }
            var viewModel = BindingContext as UserProfileViewModel;
            viewModel?.ChangeUserProfileCommand.Execute(document);
        }

        private async void UserProfileImage_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new FullSizeImageView(UserProfileImage.Source));
        }
    }
}