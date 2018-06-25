using System;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementDetailPage : ContentPage
    {
        private readonly AchievementViewModel _viewModel;

        public AchievementDetailPage(AchievementViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void AchievementNotes_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            AchievementNotes.SelectedItem = null;
        }

        private async void AddNoteButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AchievementNoteCreatePage(_viewModel));
        }
    }
}