using System;
using System.IO;
using System.Reactive.Linq;
using ReminderXamarin.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BirthdayDetailView : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();
        private static readonly IFileSystem FileService = DependencyService.Get<IFileSystem>();
        private static readonly IMediaService MediaService = DependencyService.Get<IMediaService>();
        private readonly BirthdayViewModel _viewModel;

        public BirthdayDetailView(BirthdayViewModel viewModel)
        {
            InitializeComponent();

            DatePicker.Date = viewModel.BirthDayDate;
            NameEntry.Text = viewModel.Name;
            GiftDescriptionEditor.Text = viewModel.GiftDescription;

            _viewModel = viewModel;
            BindingContext = viewModel;

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
                    var resizedImage = MediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth, ConstantsHelper.ResizedImageHeight);
                    FriendPhoto.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
                    _viewModel.ImageContent = resizedImage;
                });
        }        

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantsHelper.Warning, ConstantsHelper.BirthdaysDeleteMessage, ConstantsHelper.Ok, ConstantsHelper.Cancel);

            if (result)
            {
                _viewModel.DeleteBirthdayCommand.Execute(null);
                await Navigation.PopAsync();
            }
        }

        private async void Confirm_OnClicked(object sender, EventArgs e)
        {
            if (GiftDescriptionEditor.Text == null || NameEntry.Text == null)
            {
                return;
            }
            _viewModel.BirthDayDate = DatePicker.Date;
            _viewModel.GiftDescription = GiftDescriptionEditor.Text;
            _viewModel.Name = NameEntry.Text;
            _viewModel.UpdateBirthdayCommand.Execute(null);
            await Navigation.PopAsync();
        }

        private async void FriendPhoto_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new FullSizeImageView(FriendPhoto.Source));
        }
    }
}