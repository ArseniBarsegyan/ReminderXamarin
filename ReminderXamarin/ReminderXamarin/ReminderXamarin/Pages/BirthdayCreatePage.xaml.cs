using System;
using System.IO;
using ReminderXamarin.Interfaces;
using ReminderXamarin.Interfaces.FilePickerService;
using ReminderXamarin.Views;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BirthdayCreatePage : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();
        private static readonly IFileSystem FileService = DependencyService.Get<IFileSystem>();
        private bool _isPhotoSet;

        public BirthdayCreatePage()
        {
            InitializeComponent();
        }

        private async void PhotoPickerButton_OnClicked(object sender, EventArgs e)
        {
            var document = await DocumentPicker.DisplayImportAsync(this);

            if (document == null)
            {
                return;
            }
            // Retrieve file content throught IFileService implementation.
            byte[] fileContent = FileService.ReadAllBytes(document.Path);
            _isPhotoSet = true;

            FriendPhoto.Source = ImageSource.FromStream(() => new MemoryStream(fileContent));
            ViewModel.ImageContent = fileContent;
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