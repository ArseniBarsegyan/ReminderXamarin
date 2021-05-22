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
        private NotesViewModel ViewModel => BindingContext as NotesViewModel;

        public NotesView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
            NotesList.SelectedItem = null;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.OnDisappearing();
        }

        private async void NotesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            NotesList.SelectedItem = null;
            if (e.SelectedItem is Note model)
            {
                await ViewModel.NavigateToEditViewCommand.ExecuteAsync(model.Id);
            }
        }

        private void NotesList_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if ((Note)e.Item == ViewModel.Notes.ElementAt(ViewModel.Notes.Count - 1))
            {
                ViewModel.LoadMoreNotes();
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
