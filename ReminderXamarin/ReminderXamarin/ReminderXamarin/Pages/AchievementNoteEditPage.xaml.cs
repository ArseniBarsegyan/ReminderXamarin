using System;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementNoteEditPage : ContentPage
    {
        private readonly AchievementViewModel _achievementViewModel;
        private readonly AchievementNoteViewModel _achievementNoteViewModel;

        public AchievementNoteEditPage(AchievementViewModel achievementViewModel, AchievementNoteViewModel achievementNoteViewModel)
        {
            InitializeComponent();
            _achievementViewModel = achievementViewModel;
            _achievementNoteViewModel = achievementNoteViewModel;
            FromTimePicker.Time = _achievementNoteViewModel.From.TimeOfDay;
            BindingContext = achievementNoteViewModel;
        }

        private async void SubmitButton_OnClicked(object sender, EventArgs e)
        {
            var fromFullDate = DatePicker.Date.Add(FromTimePicker.Time);
            var toFullDate = DatePicker.Date.Add(ToTimePicker.Time);

            _achievementNoteViewModel.From = fromFullDate;
            _achievementNoteViewModel.To = toFullDate;
            _achievementNoteViewModel.HoursSpent = (toFullDate - fromFullDate).Hours;
            _achievementNoteViewModel.Description = DescriptionEditor.Text;

            _achievementViewModel.UpdateAchievementCommand.Execute(_achievementNoteViewModel);
            await Navigation.PopAsync();
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}