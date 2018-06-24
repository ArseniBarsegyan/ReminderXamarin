using System;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementsPage : ContentPage
    {
        public AchievementsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        private async void Create_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AchievementCreatePage());
        }

        private async void AchievementsList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is AchievementViewModel viewModel)
            {
                await Navigation.PushAsync(new AchievementDetailPage(viewModel));
            }
            AchievementsList.SelectedItem = null;
        }

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantHelper.Warning, ConstantHelper.AchievementDeleteMessage, ConstantHelper.Ok, ConstantHelper.Cancel);
            if (result)
            {
                var menuItem = sender as MenuItem;
                var viewModel = menuItem?.CommandParameter as AchievementViewModel;
                viewModel?.DeleteAchievementCommand.Execute(viewModel);
                ViewModel.OnAppearing();
            }
        }
    }
}