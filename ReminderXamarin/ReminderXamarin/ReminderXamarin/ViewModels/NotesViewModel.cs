using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using Rm.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class NotesViewModel : BaseViewModel
    {
        private int _notesPerLoad = 10;
        private int _currentSkipCounter = 10;
        private List<NoteViewModel> _allNotes;
        private bool _isInitialized;
        private bool _isNavigatedToEditView;

        public NotesViewModel()
        {
            Notes = new ObservableCollection<NoteViewModel>();

            SearchText = string.Empty;
            DeleteNoteCommand = new Command<int>(async id => await DeleteNote(id));
            RefreshListCommand = new Command(Refresh);
            SearchCommand = new Command(SearchNotesByDescription);
            NavigateToEditViewCommand = new Command<int>(async id => await NavigateToEditView(id));
            LoadMoreNotesCommand = new Command(async () => await LoadMoreNotes());
        }

        public string SearchText { get; set; }
        public bool IsRefreshing { get; set; }
        public ObservableCollection<NoteViewModel> Notes { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public ICommand RefreshListCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand NavigateToEditViewCommand { get; set; }
        public ICommand LoadMoreNotesCommand { get; set; }

        public void OnAppearing()
        {
            if (!_isInitialized)
            {
                LoadNotesFromDatabase();

                MessagingCenter.Subscribe<NoteEditViewModel, int>(this, ConstantsHelper.NoteDeleted, (vm, id) =>
                {
                    RemoveDeletedNoteFromList(id);
                });
                MessagingCenter.Subscribe<NoteEditViewModel>(this, ConstantsHelper.NoteCreated, (vm) =>
                {
                    AddNewNoteToList();
                });
                MessagingCenter.Subscribe<NoteEditViewModel, int>(this, ConstantsHelper.NoteEdited, (vm, id) =>
                {
                    EditExistingViewModel(id);
                });
                MessagingCenter.Subscribe<NoteEditViewModel>(this, ConstantsHelper.NoteEditPageDissapeared, (vm) =>
                {
                    _isNavigatedToEditView = false;
                });
            }
            _isInitialized = true;
        }

        public void OnDissapearing()
        {
            if (!_isNavigatedToEditView)
            {
                MessagingCenter.Unsubscribe<NoteEditViewModel, int>(this, ConstantsHelper.NoteDeleted);
                MessagingCenter.Unsubscribe<NoteEditViewModel, int>(this, ConstantsHelper.NoteCreated);
                MessagingCenter.Unsubscribe<NoteEditViewModel, int>(this, ConstantsHelper.NoteEdited);
                MessagingCenter.Unsubscribe<NoteEditViewModel, int>(this, ConstantsHelper.NoteEditPageDissapeared);
            }
        }

        private async Task DeleteNote(int noteId)
        {
            bool result = await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(ConstantsHelper.NoteDeleteMessage,
                ConstantsHelper.Warning, ConstantsHelper.Ok, ConstantsHelper.No);

            if (result)
            {
                var noteToDelete = App.NoteRepository.Value.GetNoteAsync(noteId);
                App.NoteRepository.Value.DeleteNote(noteToDelete);
                RemoveDeletedNoteFromList(noteId);
            }
        }

        private void AddNewNoteToList()
        {
            var recentNote = App.NoteRepository.Value
                .GetAll()
                .OrderByDescending(x => x.CreationDate)
                .FirstOrDefault(x => x.UserId == Settings.CurrentUserId)
                .ToViewModel();
            _allNotes.Insert(0, recentNote);
            Notes.Insert(0, recentNote);
        }

        private void EditExistingViewModel(int id)
        {
            var newNote = App.NoteRepository.Value.GetNoteAsync(id).ToViewModel();

            var oldNote = _allNotes.FirstOrDefault(x => x.Id == id);
            var oldNoteIndex = _allNotes.IndexOf(oldNote);

            _allNotes.RemoveAt(oldNoteIndex);
            Notes.RemoveAt(oldNoteIndex);

            _allNotes.Insert(oldNoteIndex, newNote);
            Notes.Insert(oldNoteIndex, newNote);
        }

        private void RemoveDeletedNoteFromList(int id)
        {
            var viewModel = _allNotes.FirstOrDefault(x => x.Id == id);
            _allNotes.Remove(viewModel);
            Notes.Remove(viewModel);
        }

        private void Refresh()
        {
            IsRefreshing = true;
            LoadNotesFromDatabase();
            IsRefreshing = false;
        }

        private void SearchNotesByDescription()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadNotesFromDatabase();
            }
            else
            {
                Notes = _allNotes
                    .Where(x => x.FullDescription.Contains(SearchText))
                    .ToObservableCollection();
            }
        }

        private void LoadNotesFromDatabase()
        {
            _currentSkipCounter = 10;
            _allNotes = App.NoteRepository.Value
                .GetAll()
                .Where(x => x.UserId == Settings.CurrentUserId)
                .ToNoteViewModels()
                .OrderByDescending(x => x.CreationDate)
                .ToList();

            if (_allNotes.Count > _notesPerLoad)
            {
                Notes = _allNotes.Take(_notesPerLoad).ToObservableCollection();
            }
            else
            {
                Notes = _allNotes.ToObservableCollection();
            }

            if (SearchText != null)
            {
                Notes = Notes.Where(x => x.FullDescription.Contains(SearchText))
                    .ToObservableCollection();
            }
        }

        private async Task LoadMoreNotes()
        {
            try
            {
                List<NoteViewModel> notesToAdd;
                var remainingCount = _allNotes.Count - _allNotes.Skip(_currentSkipCounter).Count();
                if (remainingCount > _notesPerLoad)
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        notesToAdd = _allNotes
                            .Skip(_currentSkipCounter)
                            .Take(_notesPerLoad)
                            .ToList();
                    }
                    else
                    {
                        notesToAdd = _allNotes
                            .Skip(_currentSkipCounter)
                            .Take(_notesPerLoad)
                            .Where(x => x.FullDescription.Contains(SearchText))
                            .ToList();
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        notesToAdd = _allNotes
                            .Skip(_currentSkipCounter)
                            .Take(remainingCount)
                            .ToList();
                    }
                    else
                    {
                        notesToAdd = _allNotes
                            .Skip(_currentSkipCounter)
                            .Take(remainingCount)
                            .Where(x => x.FullDescription.Contains(SearchText))
                            .ToList();
                    }
                }
                foreach (var noteViewModel in notesToAdd)
                {
                    // Prevent duplicate adding models to view collection
                    if (!Notes.Contains(noteViewModel))
                    {
                        Notes.Add(noteViewModel);
                    }
                }
                _currentSkipCounter += 10;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Collection is too small: {ex.Message}");
            }
        }

        private async Task NavigateToEditView(int id)
        {
            _isNavigatedToEditView = true;
            await NavigationService.NavigateToAsync<NoteEditViewModel>(id);
        }
    }
}
