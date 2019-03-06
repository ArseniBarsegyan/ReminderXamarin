using System;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BirthdaysView : ContentPage
    {
        public BirthdaysView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        private async void FriendsList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is BirthdayViewModel viewModel)
            {
                await Navigation.PushAsync(new BirthdayDetailView(viewModel));
            }
            FriendsList.SelectedItem = null;
        }

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantsHelper.Warning, ConstantsHelper.FriendDeleteMessage, ConstantsHelper.Ok, ConstantsHelper.Cancel);

            if (result)
            {
                var menuItem = sender as MenuItem;
                var viewModel = menuItem?.CommandParameter as BirthdayViewModel;
                viewModel?.DeleteBirthdayCommand.Execute(null);
                ViewModel.OnAppearing();
            }
        }

        private async void AddFriendButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BirthdayCreateView());
        }
    }
}