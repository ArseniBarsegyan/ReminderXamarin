using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class NotesViewModel : BaseViewModel
    {
        private readonly IUploadService _uploadService;
        private readonly int _notesPerLoad = 10;
        private int _currentSkipCounter = 10;
        private List<Note> _allNotes;
        private bool _isInitialized;
        private bool _isNavigatedToEditView;        

        public NotesViewModel(INavigationService navigationService,
            IUploadService uploadService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _uploadService = uploadService;
            Notes = new ObservableCollection<Note>();

            SearchText = string.Empty;

            UploadNotesToApiCommand = commandResolver.AsyncCommand(UploadAllNotes);
            DeleteNoteCommand = commandResolver.AsyncCommand<int>(DeleteNote);
            RefreshListCommand = commandResolver.Command(Refresh);
            SearchCommand = commandResolver.Command(SearchNotesByDescription);
            NavigateToEditViewCommand = commandResolver.AsyncCommand<int>(NavigateToEditView);
        }

        public string SearchText { get; set; }
        public bool IsRefreshing { get; set; }
        public ObservableCollection<Note> Notes { get; private set; }
        
        public IAsyncCommand UploadNotesToApiCommand { get; }
        public IAsyncCommand<int> DeleteNoteCommand { get; }
        public ICommand RefreshListCommand { get; }
        public ICommand SearchCommand { get; }
        public IAsyncCommand<int> NavigateToEditViewCommand { get; }

        public void OnAppearing()
        {
            if (!_isInitialized)
            {
                Refresh();

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
                MessagingCenter.Subscribe<NoteEditViewModel>(this, ConstantsHelper.NoteEditPageDisappeared, (vm) =>
                {
                    _isNavigatedToEditView = false;
                });
            }
            _isInitialized = true;
        }

        public void OnDisappearing()
        {
            if (!_isNavigatedToEditView)
            {
                MessagingCenter.Unsubscribe<NoteEditViewModel, int>(this, ConstantsHelper.NoteDeleted);
                MessagingCenter.Unsubscribe<NoteEditViewModel, int>(this, ConstantsHelper.NoteCreated);
                MessagingCenter.Unsubscribe<NoteEditViewModel, int>(this, ConstantsHelper.NoteEdited);
                MessagingCenter.Unsubscribe<NoteEditViewModel, int>(this, ConstantsHelper.NoteEditPageDisappeared);
            }
        }

        private async Task UploadAllNotes()
        {
            var currentConnection = Connectivity.NetworkAccess;

            if (currentConnection == NetworkAccess.None || currentConnection == NetworkAccess.Unknown)
            {
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Please, check your internet connection and try again.");
                return;
            }

            try
            {
                var ctx = new CancellationToken();

                var result = await _uploadService.UploadAll(_allNotes, ctx).ConfigureAwait(false);

                if (result == HttpResult.Ok)
                {
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("All notes uploaded successfully");
                }
                else
                {
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Error while uploading notes");
                }
            }
            catch (HttpRequestException)
            {
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("It seems like server is offline. Please, try again later.");
            }
            catch (Exception)
            {
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Oops. It seems like server is down.");
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
                .GetAll(x => x.UserId == Settings.CurrentUserId)
                .OrderByDescending(x => x.CreationDate)
                .FirstOrDefault();
            _allNotes.Insert(0, recentNote);
            Notes.Insert(0, recentNote);
        }

        private void EditExistingViewModel(int id)
        {
            var newNote = App.NoteRepository.Value.GetNoteAsync(id);

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
                    .Where(CheckSearchText)
                    .ToObservableCollection();
            }
        }

        private bool CheckSearchText(Note note)
        {
            if (note.Description.IsNullOrEmpty())
                return false;

            var matchingText = SearchText.Split(' ');

            var matches = new bool[matchingText.Length];
            for (int i = 0; i < matchingText.Length; i++)
            {
                bool contains =
                   (note.Description.Contains(matchingText[i].ToLowerInvariant())
                        || note.CreationDate.ToString("dd.MM.yyyy, HH:mm").Contains(matchingText[i].ToLowerInvariant())
                        || note.EditDate.ToString("dd.MM.yyyy, HH:mm").Contains(matchingText[i].ToLowerInvariant()));
                matches[i] = contains;
            }

            if (matches.Contains(true))
            {
                return true;
            }
            return false;
        }

        private void LoadNotesFromDatabase()
        {
            _currentSkipCounter = 10;

            if (bool.TryParse(Settings.UseSafeMode, out bool result))
            {
                if (result)
                {
                    _allNotes = new List<Note>();
                    return;
                }
            }            

            _allNotes = App.NoteRepository.Value
                .GetAll(x => x.UserId == Settings.CurrentUserId)
                .OrderByDescending(x => x.CreationDate)
                .ToList();

            if (_allNotes.Count > _notesPerLoad)
            {
                Notes = _allNotes
                    .Take(_notesPerLoad)
                    .ToObservableCollection();
            }
            else
            {
                Notes = _allNotes.ToObservableCollection();
            }

            if (SearchText != null)
            {
                Notes = Notes
                    .Where(CheckSearchText)
                    .ToObservableCollection();
            }
        }

        public void LoadMoreNotes()
        {
            try
            {
                List<Note> notesToAdd;
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
                            .Where(CheckSearchText)
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
                            .Where(CheckSearchText)
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
