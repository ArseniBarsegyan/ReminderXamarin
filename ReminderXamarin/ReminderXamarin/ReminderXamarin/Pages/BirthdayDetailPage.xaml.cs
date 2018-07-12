using System;
using System.IO;
using ReminderXamarin.Helpers;
using ReminderXamarin.Interfaces;
using ReminderXamarin.Interfaces.FilePickerService;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BirthdayDetailPage : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();
        private static readonly IFileSystem FileService = DependencyService.Get<IFileSystem>();
        private readonly BirthdayViewModel _viewModel;

        public BirthdayDetailPage(BirthdayViewModel viewModel)
        {
            InitializeComponent();

            DatePicker.Date = viewModel.BirthDayDate;
            NameEntry.Text = viewModel.Name;
            GiftDescriptionEditor.Text = viewModel.GiftDescription;

            _viewModel = viewModel;
            BindingContext = viewModel;
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
            FriendPhoto.Source = ImageSource.FromStream(() => new MemoryStream(fileContent));
            _viewModel.ImageContent = fileContent;
        }

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantHelper.Warning, ConstantHelper.BirthdaysDeleteMessage, ConstantHelper.Ok, ConstantHelper.Cancel);

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
    }
}