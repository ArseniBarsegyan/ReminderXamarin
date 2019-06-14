using System.Linq;
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
                viewModel.OnDissapearing();
            }
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

        private void NotesList_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (BindingContext is NotesViewModel viewModel)
            {
                if ((NoteViewModel)e.Item == viewModel.Notes.ElementAt(viewModel.Notes.Count - 1))
                {
                    viewModel.LoadMoreNotesCommand.Execute(null);
                }
            }
        }
    }
}
