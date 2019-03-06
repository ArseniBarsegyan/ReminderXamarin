using System;
using System.Reactive.Linq;
using ReminderXamarin.Interfaces.FilePickerService;
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

            Observable.FromEventPattern(x => PickAchievementImageButton.Clicked += x,
                    x => PickAchievementImageButton.Clicked -= x)
                .Subscribe(async _ =>
                {
                    var document = await DocumentPicker.DisplayImportAsync(this);
                    if (document == null)
                    {
                        return;
                    }
                    ViewModel.SetImageCommand.Execute(document);
                });
        }
    }
}