using System;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BirthdaysPage : ContentPage
    {
        public BirthdaysPage()
        {
            InitializeComponent();
        }

        private async void FriendsList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is BirthdayViewModel viewModel)
            {
                await Navigation.PushAsync(new BirthdayDetailPage(viewModel));
            }
            FriendsList.SelectedItem = null;
        }

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantHelper.Warning, ConstantHelper.FriendDeleteMessage, ConstantHelper.Ok, ConstantHelper.Cancel);

            if (result)
            {
                var menuItem = sender as MenuItem;
                var noteViewModel = menuItem?.CommandParameter as NoteViewModel;
                noteViewModel?.DeleteNoteCommand.Execute(noteViewModel);
                ViewModel.OnAppearing();
            }
        }

        private async void AddFriendButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BirthdayCreatePage());
        }
    }
}