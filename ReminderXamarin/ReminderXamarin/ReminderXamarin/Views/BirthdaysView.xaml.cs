using System;
using Rm.Helpers;
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
            if (BindingContext is BirthdaysViewModel viewModel)
            {
                viewModel.OnAppearing();
            }
        }

        private void FriendsList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var birthdayViewModel = e.SelectedItem as BirthdayViewModel;
            FriendsList.SelectedItem = null;
            if (birthdayViewModel != null)
            {
                if (BindingContext is BirthdaysViewModel viewModel)
                {
                    viewModel.NavigateToEditBirthdayCommand.Execute(birthdayViewModel.Id);
                }
            }
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

                if (BindingContext is BirthdaysViewModel vm)
                {
                    vm.OnAppearing();
                }
            }
        }
    }
}