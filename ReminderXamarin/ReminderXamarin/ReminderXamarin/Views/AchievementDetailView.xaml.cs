using System;
using Rm.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementDetailView : ContentPage
    {
        private readonly AchievementViewModel _viewModel;

        public AchievementDetailView(AchievementViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            await _viewModel.OnAppearing();
            base.OnAppearing();
        }

        private async void DeleteAchievementNote_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantsHelper.Warning, ConstantsHelper.AchievementNoteDeleteMessage, ConstantsHelper.Ok, ConstantsHelper.Cancel);
            if (result)
            {
                var menuItem = sender as MenuItem;
                var achievementNoteViewModel = menuItem?.CommandParameter as AchievementNoteViewModel;
                _viewModel.DeleteAchievementNoteCommand.Execute(achievementNoteViewModel);
                await _viewModel.OnAppearing();
            }
        }

        private async void SaveNoteButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AchievementNoteCreateView(_viewModel));
        }

        private async void AchievementNotes_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is AchievementNoteViewModel achievementNoteViewModel)
            {
                await Navigation.PushAsync(new AchievementNoteEditView(_viewModel, achievementNoteViewModel));
            }
            AchievementNotes.SelectedItem = null;
        }

        private async void DeleteAchievementLink_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
            (ConstantsHelper.Warning, ConstantsHelper.AchievementDeleteMessage, ConstantsHelper.Ok,
                ConstantsHelper.Cancel);

            if (result)
            {
                _viewModel.DeleteAchievementCommand.Execute(null);
                await Navigation.PopAsync();
            }
        }
    }
}