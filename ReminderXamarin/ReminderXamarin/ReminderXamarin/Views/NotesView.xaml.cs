using ReminderXamarin.ViewModels;

using Rm.Data.Data.Entities;

using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ReminderXamarin.Views
{
    public partial class NotesView : ContentPage
    {
        private bool _isAnimationInProgress;

        public NotesView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ToggleSearchBarVisibility(SearchToolbarItem, EventArgs.Empty);
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

        private async void ToggleSearchBarVisibility(object sender, EventArgs e)
        {
            if (_isAnimationInProgress)
            {
                return;
            }
            _isAnimationInProgress = true;

            if (SearchBar.IsVisible)
            {
                SearchToolbarItem.IconImageSource = "search_icon.png";
                var tasks = new Task[]
                {
                    SearchBar.TranslateTo(0, -120, 100),
                    SearchBar.FadeTo(0, 100)
                };
                await Task.WhenAll(tasks);
                SearchBar.IsVisible = false;
            }
            else
            {
                SearchToolbarItem.IconImageSource = "cancel.png";
                SearchBar.IsVisible = true;
                var tasks = new Task[]
                {
                    SearchBar.TranslateTo(0, 0, 100),
                    SearchBar.FadeTo(1, 100)
                };
                await Task.WhenAll(tasks);
            }

            _isAnimationInProgress = false;
        }
    }
}
