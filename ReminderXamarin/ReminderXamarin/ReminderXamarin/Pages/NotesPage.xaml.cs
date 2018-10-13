using System;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;

namespace ReminderXamarin.Pages
{
    /// <inheritdoc />
    /// <summary>
    /// List of notes with search bar at the top.
    /// </summary>
    public partial class NotesPage : ContentPage
    {
        public NotesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        private async void NotesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is NoteViewModel viewModel)
            {
                await Navigation.PushAsync(new NoteDetailPage(viewModel));
            }
            NotesList.SelectedItem = null;
        }        

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.SearchCommand.Execute(SearchBar.Text);
        }

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantsHelper.Warning, ConstantsHelper.NoteDeleteMessage, ConstantsHelper.Ok, ConstantsHelper.Cancel);
            if (result)
            {
                var menuItem = sender as MenuItem;
                var noteViewModel = menuItem?.CommandParameter as NoteViewModel;
                noteViewModel?.DeleteNoteCommand.Execute(null);
                ViewModel.OnAppearing();
            }
        }

        private async void CreateNoteButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NoteCreatePage());
        }
    }
}
