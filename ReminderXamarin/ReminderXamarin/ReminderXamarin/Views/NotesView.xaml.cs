using System;
using System.Linq;
using System.Threading.Tasks;

using ReminderXamarin.ViewModels;

using Rm.Data.Data.Entities;
using Rm.Helpers;

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
            if (BindingContext is NotesViewModel viewModel)
            {
                viewModel.OnAppearing();
            }
            NotesList.SelectedItem = null;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is NotesViewModel viewModel)
            {
                viewModel.OnDisappearing();
            }
        }

        private async void NotesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            NotesList.SelectedItem = null;
            if (e.SelectedItem is Note model)
            {
                if (BindingContext is NotesViewModel viewModel)
                {
                    await viewModel.NavigateToEditViewCommand.ExecuteAsync(model.Id);
                }
            }
        }

        private void NotesList_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (BindingContext is NotesViewModel viewModel)
            {
                if ((Note)e.Item == viewModel.Notes.ElementAt(viewModel.Notes.Count - 1))
                {
                    viewModel.LoadMoreNotes();
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
                SearchToolbarItem.IconImageSource = ConstantsHelper.SearchIcon;
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
                SearchToolbarItem.IconImageSource = ConstantsHelper.CancelIcon;
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

        private async void NotesList_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (_isAnimationInProgress)
            {
                return;
            }

            if (CreateNoteButton.IsVisible)
            {
                await HideCreateNoteButton();
                Device.StartTimer(TimeSpan.FromMilliseconds(800), () =>
                {
                    ShowCreateNoteButton();
                    return false;
                });
            }
        }

        private async Task HideCreateNoteButton()
        {
            _isAnimationInProgress = true;
            await Task.Run(() =>
            {
                CreateNoteButton.FadeTo(0, 200);
            });
            CreateNoteButton.IsVisible = false;
        }

        private async Task ShowCreateNoteButton()
        {
            await Task.Run(() =>
            {
                CreateNoteButton.FadeTo(1, 200);
            });

            CreateNoteButton.IsVisible = true;
            _isAnimationInProgress = false;
        }
    }
}
