using System;
using Rm.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;

namespace ReminderXamarin.Views
{
    /// <inheritdoc />
    /// <summary>
    /// List of notes with search bar at the top.
    /// </summary>
    public partial class NotesView : ContentPage
    {
        public NotesView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is NotesViewModel viewModel)
            {
                await viewModel.OnAppearing();
            }            
        }

        private async void NotesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var viewModel = e.SelectedItem as NoteViewModel;
            NotesList.SelectedItem = null;
            if (viewModel != null)
            {
                await Navigation.PushAsync(new NoteDetailView(viewModel));
            }
        }

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantsHelper.Warning, ConstantsHelper.NoteDeleteMessage, ConstantsHelper.Ok, ConstantsHelper.Cancel);
            if (result)
            {
                var menuItem = sender as MenuItem;
                var noteViewModel = menuItem?.CommandParameter as NoteViewModel;

                if (BindingContext is NotesViewModel viewModel)
                {
                    viewModel.DeleteNoteCommand.Execute(noteViewModel?.Id);
                    await viewModel.OnAppearing();
                }
            }
        }

        private async void CreateNoteButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NoteCreateView());
        }
    }
}
