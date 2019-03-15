using System;
using System.Linq;
using ReminderXamarin.Elements;
using ReminderXamarin.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using Rg.Plugins.Popup.Extensions;
using RI.Data.Data.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteCreateView : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();
        // Display message only when tap "back" and there is no changes.
        private bool _shouldDisplayMessage;
        private bool _saveClicked;

        public NoteCreateView()
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
            MessagingCenter.Subscribe<ImageGallery, int>(this, ConstantsHelper.ImageDeleted, (gallery, i) =>
            {
                ViewModel.DeletePhotoCommand.Execute(i);
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.PhotosCollectionChanged -= ViewModel_OnPhotosCollectionChanged;
            MessagingCenter.Unsubscribe<ImageGallery, int>(this, ConstantsHelper.ImageDeleted);
        }

        private void ViewModel_OnPhotosCollectionChanged(object sender, EventArgs e)
        {
            ImageGallery.IsVisible = true;
            ImageGallery.Render();
        }

        private async void HorizontalImageGallery_OnItemTapped(object sender, EventArgs e)
        {
            //if (sender is Image tappedImage)
            //{
            //    FileImageSource fileImageSource = (FileImageSource)tappedImage.Source;
            //    string filePath = fileImageSource.File;
            //    ViewModel.SelectedPhoto = ViewModel.Photos.FirstOrDefault(x => x.ResizedPath == filePath);
            //    var images = ViewModel.Photos.ToImages();
            //    var currentImage = ViewModel.SelectedPhoto.ToImage();

            //    await Navigation.PushPopupAsync(new FullSizeImageGallery(images, currentImage));
            //}
            if (sender is Image tappedImage)
            {
                await Navigation.PushPopupAsync(new FullSizeImageView(tappedImage.Source));
            }
        }

        private void AddButton_OnClicked(object sender, EventArgs e)
        {
            AddItemsToNoteContentView.IsVisible = true;
        }

        private void AddItemsToNoteContentView_OnTakePhotoButtonClicked(object sender, EventArgs e)
        {
            ViewModel.TakePhotoCommand.Execute(null);
        }

        private async void AddItemsToNoteContentView_OnPickPhotoButtonClicked(object sender, EventArgs e)
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

        private void AddItemsToNoteContentView_OnTakeVideoButtonClicked(object sender, EventArgs e)
        {
            ViewModel.TakeVideoCommand.Execute(null);
        }

        private async void AddItemsToNoteContentView_OnPickVideoButtonClicked(object sender, EventArgs e)
        {
            ViewModel.IsLoading = true;
            var document = await DocumentPicker.DisplayImportAsync(this);
            if (document == null)
            {
                ViewModel.IsLoading = false;
                return;
            }
            ViewModel.PickVideoCommand.Execute(document);
        }

        private void VideoList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var viewModel = e.SelectedItem as VideoModel;
            VideoList.SelectedItem = null;
            if (viewModel != null)
            {
                var videoService = DependencyService.Get<IVideoService>();
                videoService.PlayVideo(viewModel.Path);
            }
        }
    }
}