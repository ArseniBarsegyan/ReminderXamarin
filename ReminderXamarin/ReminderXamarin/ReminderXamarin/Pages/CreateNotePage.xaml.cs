using System;
using System.Linq;
using ReminderXamarin.Elements;
using ReminderXamarin.Extensions;
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

        public CreateNotePage()
        {
            InitializeComponent();
        }

        // Create note with photos and save them to SQLite DB
        private async void Save_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionEditor.Text))
            {
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

        private void ViewModel_OnPhotoAdded(object sender, EventArgs e)
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
            var document = await DocumentPicker.DisplayImportAsync(this);
            if (document == null)
            {
                return;
            }
            ViewModel.PickPhotoCommand.Execute(document);
        }
    }
}