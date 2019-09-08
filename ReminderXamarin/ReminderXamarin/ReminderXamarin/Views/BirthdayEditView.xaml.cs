using System;
using System.IO;
using Rm.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BirthdayEditView : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();
        private static readonly IFileSystem FileService = DependencyService.Get<IFileSystem>();
        private static readonly IMediaService MediaService = DependencyService.Get<IMediaService>();
        
        public BirthdayEditView()
        {
            InitializeComponent();
        }

        private void Confirm_OnClicked(object sender, EventArgs e)
        {
            if (GiftDescriptionEditor.Text == null || NameEntry.Text == null)
            {
                return;
            }
            if (BindingContext is BirthdayEditViewModel viewModel)
            {
                viewModel.BirthDayDate = DatePicker.Date;
                viewModel.GiftDescription = GiftDescriptionEditor.Text;
                viewModel.Name = NameEntry.Text;
                viewModel.SaveBirthdayCommand.Execute(null);
            }
        }

        private async void FriendPhoto_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new FullSizeImageView(FriendPhoto.Source));
        }

        private async void PhotoPickButton_OnClicked(object sender, EventArgs e)
        {
            var document = await DocumentPicker.DisplayImportAsync(this);

            if (document == null)
            {
                return;
            }
            var imageContent = FileService.ReadAllBytes(document.Path);
            var resizedImage = MediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth, ConstantsHelper.ResizedImageHeight);
            FriendPhoto.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
            if (BindingContext is BirthdayEditViewModel viewModel)
            {
                viewModel.ImageContent = resizedImage;
            }
        }
    }
}