using System.Linq;
using ReminderXamarin.ViewModels;
using Rm.Data.Data.Entities;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (BindingContext is NotesViewModel viewModel)
            {
                viewModel.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is NotesViewModel viewModel)
            {
                viewModel.OnDisappearing();
            }
        }

        private void NotesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var noteModel = e.SelectedItem as Note;
            NotesList.SelectedItem = null;
            if (noteModel != null)
            {
                if (BindingContext is NotesViewModel viewModel)
                {
                    viewModel.NavigateToEditViewCommand.Execute(noteModel.Id);
                }
            }
        }

        private void NotesList_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (BindingContext is NotesViewModel viewModel)
            {
                if ((Note)e.Item == viewModel.Notes.ElementAt(viewModel.Notes.Count - 1))
                {
                    viewModel.LoadMoreNotesCommand.Execute(null);
                }
            }
        }
    }
}
