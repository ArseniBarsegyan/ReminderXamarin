using System;
using System.Reactive.Linq;
using ReminderXamarin.Interfaces.FilePickerService;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementCreatePage : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();

        public AchievementCreatePage()
        {
            InitializeComponent();

            Observable.FromEventPattern(x => AchievementCreateButton.Clicked += x,
                x => AchievementCreateButton.Clicked -= x)
                .Subscribe(async _ =>
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
                });
        }

        private async void PickImage_OnTapped(object sender, EventArgs e)
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