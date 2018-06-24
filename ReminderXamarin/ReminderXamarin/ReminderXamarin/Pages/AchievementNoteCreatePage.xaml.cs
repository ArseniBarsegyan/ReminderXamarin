using System;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementNoteCreatePage : ContentPage
    {
        private readonly AchievementViewModel _viewModel;

        public AchievementNoteCreatePage(AchievementViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }

        private async void SubmitButton_OnClicked(object sender, EventArgs e)
        {
            var fromFullDate = DatePicker.Date.Add(FromTimePicker.Time);
            var toFullDate = DatePicker.Date.Add(ToTimePicker.Time);

            var achievementNoteViewModel = new AchievementNoteViewModel
            {
                AchievementId = _viewModel.Id,
                Description = DescriptionEditor.Text,
                From = fromFullDate,
                To = toFullDate,
                HoursSpent = (toFullDate - fromFullDate).Hours
            };
            _viewModel.AchievementNotes.Add(achievementNoteViewModel);
            _viewModel.UpdateAchievementCommand.Execute(null);
            await Navigation.PopModalAsync();
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}