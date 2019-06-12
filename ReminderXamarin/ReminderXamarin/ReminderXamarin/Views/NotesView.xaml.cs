using System;
using System.Linq;
using System.Threading.Tasks;
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
            SearchBar.FadeTo(0, 0);
            NotesList.FadeTo(0, 0);
            CreateNoteButton.Scale = 0;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            if (BindingContext is NotesViewModel viewModel)
            {
                viewModel.OnAppearing();
            }
            await Task.Delay(200);
            await SearchBar.FadeTo(1, 500, Easing.CubicInOut);
            await NotesList.FadeTo(1, 500, Easing.CubicInOut);
            await CreateNoteButton.ScaleTo(1, 400);
        }

        private void NotesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var noteViewModel = e.SelectedItem as NoteViewModel;
            NotesList.SelectedItem = null;
            if (noteViewModel != null)
            {
                // await Navigation.PushAsync(new NoteDetailView(viewModel));
                if (BindingContext is NotesViewModel viewModel)
                {
                    viewModel.NavigateToEditViewCommand.Execute(noteViewModel.Id);
                }
            }
        }

        private async void NotesList_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            CreateNoteButton.Scale = 0;
            if (BindingContext is NotesViewModel viewModel)
            {
                if ((NoteViewModel)e.Item == viewModel.Notes.ElementAt(viewModel.Notes.Count - 1))
                {
                    viewModel.LoadMoreNotesCommand.Execute(null);
                }
            }
            await Task.Delay(300);
            await CreateNoteButton.ScaleTo(1, 300);
        }
    }
}
