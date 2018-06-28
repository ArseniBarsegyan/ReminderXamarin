using System;
using ReminderXamarin.Helpers;
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
            BindingContext = achievementNoteViewModel;
        }

        private async void SubmitButton_OnClicked(object sender, EventArgs e)
        {
            bool result = double.TryParse(TimeSpentEditor.Text, out var timeSpent);

            if (result)
            {
                _achievementNoteViewModel.Date = DatePicker.Date;
                _achievementNoteViewModel.HoursSpent = timeSpent;
                _achievementNoteViewModel.Description = DescriptionEditor.Text;

                _achievementViewModel.UpdateAchievementCommand.Execute(_achievementNoteViewModel);
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert(ConstantHelper.Warning, ConstantHelper.TimeParsingError, ConstantHelper.Ok);
            }
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}