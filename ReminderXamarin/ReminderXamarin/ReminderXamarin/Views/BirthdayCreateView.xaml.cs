﻿using System;
using System.IO;
using System.Reactive.Linq;
using ReminderXamarin.Interfaces;
using ReminderXamarin.Interfaces.FilePickerService;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BirthdayCreateView : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();
        private static readonly IFileSystem FileService = DependencyService.Get<IFileSystem>();
        private static readonly IMediaService MediaService = DependencyService.Get<IMediaService>();
        private bool _isPhotoSet;

        public BirthdayCreateView()
        {
            InitializeComponent();

            Observable.FromEventPattern(x => PhotoPickButton.Clicked += x,
                    x => PhotoPickButton.Clicked -= x)
                .Subscribe(async _ =>
                {
                    var document = await DocumentPicker.DisplayImportAsync(this);

                    if (document == null)
                    {
                        return;
                    }
                    // Retrieve file content throught IFileService implementation.
                    var imageContent = FileService.ReadAllBytes(document.Path);
                    var resizedImage = MediaService.ResizeImage(imageContent, 1360, 768);
                    _isPhotoSet = true;

                    FriendPhoto.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
                    ViewModel.ImageContent = resizedImage;
                });
        }

        private async void Save_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(GiftDescriptionEditor.Text) ||
                !_isPhotoSet)
            {
                await Navigation.PopAsync();
                return;
            }

            ViewModel.Name = NameEntry.Text;
            ViewModel.BirthDayDate = DatePicker.Date;
            ViewModel.GiftDescription = GiftDescriptionEditor.Text;
            ViewModel.CreateBirthdayCommand.Execute(null);

            await Navigation.PopAsync();
        }

        private async void FriendPhoto_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new FullSizeImageView(FriendPhoto.Source));
        }
    }
}