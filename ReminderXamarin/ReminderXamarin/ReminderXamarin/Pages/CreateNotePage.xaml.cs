﻿using System;
using System.Linq;
using ReminderXamarin.Elements;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using ReminderXamarin.Interfaces.FilePickerService;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNotePage : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();
        // Display message only when tap "back" and there is no changes.
        private bool _shouldDisplayMessage;
        private bool _saveClicked;

        public CreateNotePage()
        {
            InitializeComponent();
        }

        // Message should display only when user tap "back" and current data
        // has been modified (added photos or description)
        public bool ShouldDisplayMessage()
        {
            if (_saveClicked)
            {
                _shouldDisplayMessage = false;
            }
            else if (ViewModel.Photos.Any() || !string.IsNullOrWhiteSpace(DescriptionEditor.Text))
            {
                _shouldDisplayMessage = true;
            }
            else
            {
                _shouldDisplayMessage = false;
            }
            return _shouldDisplayMessage;
        }

        // Create note with photos and save them to SQLite DB.
        // There is no need to display warning message.
        private async void Save_OnClicked(object sender, EventArgs e)
        {
            _saveClicked = true;
            ViewModel.IsLoading = true;

            if (string.IsNullOrWhiteSpace(DescriptionEditor.Text))
            {
                ViewModel.IsLoading = false;
                await Navigation.PopAsync();
                return;
            }

            DateTime currentDateTime = DateTime.Now;

            ViewModel.CreationDate = currentDateTime;
            ViewModel.EditDate = currentDateTime;
            ViewModel.Description = DescriptionEditor.Text;

            ViewModel.CreateNoteCommand.Execute(null);
            await Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.PhotosCollectionChanged += ViewModel_OnPhotosCollectionChanged;
            MessagingCenter.Subscribe<ImageGallery, int>(this, ConstantHelper.ImageDeleted, (gallery, i) =>
            {
                ViewModel.DeletePhotoCommand.Execute(i);
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.PhotosCollectionChanged -= ViewModel_OnPhotosCollectionChanged;
            MessagingCenter.Unsubscribe<ImageGallery, int>(this, ConstantHelper.ImageDeleted);
        }

        private void ViewModel_OnPhotosCollectionChanged(object sender, EventArgs e)
        {
            ImageGallery.IsVisible = true;
            ImageGallery.Render();
        }

        private async void HorizontalImageGallery_OnItemTapped(object sender, EventArgs e)
        {
            if (sender is Image tappedImage)
            {
                FileImageSource fileImageSource = (FileImageSource)tappedImage.Source;
                string filePath = fileImageSource.File;
                ViewModel.SelectedPhoto = ViewModel.Photos.FirstOrDefault(x => x.ResizedPath == filePath);
                var images = ViewModel.Photos.ToImages();
                var currentImage = ViewModel.SelectedPhoto.ToImage();

                await Navigation.PushPopupAsync(new FullSizeImageGallery(images, currentImage));
            }
        }

        private async void PickPhoto_OnClicked(object sender, EventArgs e)
        {
            ViewModel.IsLoading = true;
            var document = await DocumentPicker.DisplayImportAsync(this);
            if (document == null)
            {
                ViewModel.IsLoading = false;
                return;
            }            
            ViewModel.PickPhotoCommand.Execute(document);
        }
    }
}