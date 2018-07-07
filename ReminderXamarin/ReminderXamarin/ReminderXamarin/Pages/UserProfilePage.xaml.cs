﻿using System;
using ReminderXamarin.Interfaces.FilePickerService;
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

        public UserProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        private async void EditUserProfilePhoto_OnTapped(object sender, EventArgs e)
        {
            var document = await DocumentPicker.DisplayImportAsync(this);
            if (document == null)
            {
                return;
            }
            ViewModel.ChangeUserProfileCommand.Execute(document);
        }

        private async void UserProfileImage_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new FullSizeImageView(UserProfileImage.Source));
        }
    }
}