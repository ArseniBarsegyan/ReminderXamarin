using System;
using ReminderXamarin.Services.FilePickerService;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementCreateView : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();

        public AchievementCreateView()
        {
            InitializeComponent();
        }

        private async void AchievementCreateButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            {
                await Navigation.PopAsync();
                return;
            }
            ViewModel.Title = TitleEntry.Text;
            ViewModel.GeneralDescription = DescriptionEditor.Text;
            ViewModel.CreateAchievementCommand.Execute(null);
            await Navigation.PopAsync();
        }

        private async void PickAchievementImageButton_OnClicked(object sender, EventArgs e)
        {
            var document = await DocumentPicker.DisplayImportAsync(this);
            if (document == null)
            {
                return;
            }
            ViewModel.SetImageCommand.Execute(document);
        }
    }
}