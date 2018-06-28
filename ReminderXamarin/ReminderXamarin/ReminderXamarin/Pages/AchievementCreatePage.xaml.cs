using System;
using System.IO;
using ReminderXamarin.Interfaces;
using ReminderXamarin.Interfaces.FilePickerService;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementCreatePage : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();
        private static readonly IFileSystem FileService = DependencyService.Get<IFileSystem>();

        public AchievementCreatePage()
        {
            InitializeComponent();
        }

        private async void Save_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleEditor.Text))
            {
                await Navigation.PopAsync();
                return;
            }
            ViewModel.Title = TitleEditor.Text;
            ViewModel.GeneralDescription = DescriptionEditor.Text;
            ViewModel.CreateAchievementCommand.Execute(null);
            await Navigation.PopAsync();
        }

        private async void PickImageButton_OnClicked(object sender, EventArgs e)
        {
            var document = await DocumentPicker.DisplayImportAsync(this);

            if (document == null)
            {
                return;
            }
            // Retrieve file content throught IFileService implementation.
            byte[] fileContent = FileService.ReadAllBytes(document.Path);

            FileNameLabel.IsVisible = true;
            FileNameLabel.Text = document.Name;
            
            PreviewImage.IsVisible = true;
            PreviewImage.Source = ImageSource.FromStream(() => new MemoryStream(fileContent));
            ViewModel.ImageContent = fileContent;
        }
    }
}