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
    }
}
