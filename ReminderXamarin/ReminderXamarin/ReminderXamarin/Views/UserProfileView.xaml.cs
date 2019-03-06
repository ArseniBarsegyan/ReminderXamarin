using System;
using System.Reactive.Linq;
using ReminderXamarin.Helpers;
using ReminderXamarin.Interfaces.FilePickerService;
using ReminderXamarin.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfileView : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();
        private bool _isTranslated;

        public UserProfileView(UserProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            BackgroundImage.Source = ImageSource.FromResource(ConstantsHelper.BackgroundImageSource);

            Observable.FromEventPattern(x => PickUserPhotoImage.Clicked += x,
                    x => PickUserPhotoImage.Clicked -= x)
                .Subscribe(async _ =>
                {
                    var document = await DocumentPicker.DisplayImportAsync(this);
                    if (document == null)
                    {
                        return;
                    }
                    viewModel?.ChangeUserProfileCommand.Execute(document);
                });
        }

        private async void UserProfileImage_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new FullSizeImageView(UserProfileImage.Source));
        }

        private void BackgroundImage_OnTapped(object sender, EventArgs e)
        {
            if (_isTranslated)
            {
                BackgroundImage.LayoutTo(new Rectangle(0, 0, Width, 100), 250, Easing.SpringIn);
                UserProfileImage.TranslateTo(0, 0, 250, Easing.SpringIn);
                PickUserPhotoImage.TranslateTo(0, 0, 250, Easing.SpringIn);
                UserInfoLayout.TranslateTo(0, 0, 250, Easing.SpringIn);
            }
            else
            {
                BackgroundImage.LayoutTo(new Rectangle(0, 0, Width, 200), 250, Easing.SpringOut);
                UserProfileImage.TranslateTo(0, 100, 250, Easing.SpringOut);
                PickUserPhotoImage.TranslateTo(0, 100, 250, Easing.SpringOut);
                UserInfoLayout.TranslateTo(0, 100, 250, Easing.SpringOut);
            }
            _isTranslated = !_isTranslated;
        }
    }
}