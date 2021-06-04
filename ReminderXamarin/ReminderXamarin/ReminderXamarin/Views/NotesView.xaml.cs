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
            NotesCollection.SelectedItem = null;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.OnDisappearing();
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

        private async Task HideCreateNoteButton()
        {
            _isAnimationInProgress = true;
            await Task.Run(() =>
            {
                CreateNoteButton.FadeTo(0, 200);
            });
            CreateNoteButton.IsVisible = false;
            _isAnimationInProgress = false;
        }

        private async Task ShowCreateNoteButton()
        {
            _isAnimationInProgress = true;
            await Task.Run(() =>
            {
                CreateNoteButton.FadeTo(1, 200);
            });

            CreateNoteButton.IsVisible = true;
            _isAnimationInProgress = false;
        }

        private async void NotesCollectionOnScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (e.LastVisibleItemIndex == ViewModel.Notes.Count - 1)
            {
                ViewModel.LoadMoreNotes();
            }
            
            if (_isAnimationInProgress)
            {
                return;
            }

            if (e.VerticalDelta > 0)
            {
                await HideCreateNoteButton();
            }
            else if (e.VerticalDelta < 0)
            {
                await ShowCreateNoteButton();
            }
        }

        private async void NotesCollectionOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NotesCollection.SelectedItem = null;
            if (e.CurrentSelection.FirstOrDefault() is Note model)
            {
                await ViewModel.NavigateToEditViewCommand.ExecuteAsync(model.Id);
            }
        }
    }
}
