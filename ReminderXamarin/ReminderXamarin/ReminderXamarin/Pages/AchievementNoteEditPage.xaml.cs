using System;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementNoteEditPage : ContentPage
    {
        private readonly AchievementNoteViewModel _achievementNoteViewModel;

        public AchievementNoteEditPage(AchievementNoteViewModel achievementNoteViewModel)
        {
            InitializeComponent();
            _achievementNoteViewModel = achievementNoteViewModel;
            BindingContext = achievementNoteViewModel;
        }

        private void SubmitButton_OnClicked(object sender, EventArgs e)
        {
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}