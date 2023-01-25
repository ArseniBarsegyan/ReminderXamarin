using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
using ReminderXamarin.Core.Interfaces.Services;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;
using Rm.Data.Data.Entities;
using Rm.Data.Data.Repositories;
using Rm.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class NotesViewModel : BaseNavigableViewModel
    {
        private const int NotesPerLoad = 10;

        private readonly IUploadService _uploadService;
        private readonly INotesImportService _notesImportService;
        private readonly IPlatformDocumentPicker _documentPicker;
        private int _currentSkipCounter = 10;
        private List<Note> _allNotes;
        private bool _isInitialized;
        private bool _isNavigatedToEditView;

        private static NoteRepository NoteRepository => App.NoteRepository.Value;

        public NotesViewModel(
            INavigationService navigationService,
            INotesImportService notesImportService,
            IPlatformDocumentPicker documentPicker,
            IUploadService uploadService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _notesImportService = notesImportService;
            _documentPicker = documentPicker;
            _uploadService = uploadService;
            Notes = new ObservableCollection<Note>();

            SearchText = string.Empty;

            ImportNotesCommand = commandResolver.AsyncCommand(ImportAllNotes);
            UploadNotesCommand = commandResolver.AsyncCommand(UploadAllNotes);
            DeleteNoteCommand = commandResolver.AsyncCommand<Note>(DeleteNote);
            RefreshListCommand = commandResolver.Command(Refresh);
            SearchCommand = commandResolver.Command(SearchNotesByDescription);
            NavigateToEditViewCommand = commandResolver.AsyncCommand<int>(NavigateToEditView);
        }

        public string SearchText { get; set; }
        public bool IsRefreshing { get; set; }
        public ObservableCollection<Note> Notes { get; private set; }
        public IAsyncCommand ImportNotesCommand { get; }
        public IAsyncCommand UploadNotesCommand { get; }
        public IAsyncCommand<Note> DeleteNoteCommand { get; }
        public ICommand RefreshListCommand { get; }
        public ICommand SearchCommand { get; }
        public IAsyncCommand<int> NavigateToEditViewCommand { get; }

        public void OnAppearing()
        {
            if (!_isInitialized)
            {
                Refresh();

                MessagingCenter.Subscribe<NoteEditViewModel, int>(
                    this, ConstantsHelper.NoteDeleted, (vm, id) => { RemoveDeletedNoteFromList(id); });
                MessagingCenter.Subscribe<NoteEditViewModel>(
                    this, ConstantsHelper.NoteCreated, (vm) => { AddNewNoteToList(); });
                MessagingCenter.Subscribe<NoteEditViewModel, int>(
                    this, ConstantsHelper.NoteEdited, (vm, id) => { EditExistingViewModel(id); });
                MessagingCenter.Subscribe<NoteEditViewModel>(
                    this, ConstantsHelper.NoteEditPageDisappeared, (vm) => { _isNavigatedToEditView = false; });
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

        private async Task ImportAllNotes()
        {
            try
            {
                IsRefreshing = true;
                var document = await _documentPicker.DisplayTextImportAsync();
                if (document == null)
                {
                    IsRefreshing = false;
                    return;
                }
            
                var importNotes = _notesImportService.ImportNotes(document.Path);
            
                foreach (var note in importNotes)
                {
                    note.UserId = Settings.CurrentUserId;
                    NoteRepository.Save(note);
                }
            
                IsRefreshing = false;
                await UserDialogs.Instance.AlertAsync(
                    "Notes imported.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task UploadAllNotes()
        {
            var currentConnection = Connectivity.NetworkAccess;

            if (currentConnection == NetworkAccess.None || currentConnection == NetworkAccess.Unknown)
            {
                await UserDialogs.Instance.AlertAsync(
                    "Please, check your internet connection and try again.");
                
                return;
            }

            try
            {
                await _uploadService.SendEmailWithAttachments(
                    "Notes", "Reminder notes",
                    _allNotes);
            }
            catch (FeatureNotSupportedException)
            {
                await UserDialogs.Instance.AlertAsync("Feature not supported");
            }
            catch (Exception)
            {
                await UserDialogs.Instance.AlertAsync("Something went wrong during sending notes");
            }
        }

        private async Task DeleteNote(Note note)
        {
            bool result = await UserDialogs.Instance.ConfirmAsync(ConstantsHelper.NoteDeleteMessage,
                ConstantsHelper.Warning, ConstantsHelper.Ok, ConstantsHelper.No);

            if (result)
            {
                NoteRepository.DeleteNote(note);
                RemoveDeletedNoteFromList(note.Id);
            }
        }

        private void AddNewNoteToList()
        {
            var recentNote = NoteRepository
                .GetAll(x => x.UserId == Settings.CurrentUserId)
                .OrderByDescending(x => x.CreationDate)
                .FirstOrDefault();
            _allNotes.Insert(0, recentNote);
            Notes.Insert(0, recentNote);
        }

        private void EditExistingViewModel(int id)
        {
            var newNote = NoteRepository.GetNoteAsync(id);

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
            {
                return false;
            }

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

            _allNotes = NoteRepository
                .GetAll(x => x.UserId == Settings.CurrentUserId)
                .OrderByDescending(x => x.CreationDate)
                .ToList();

            if (_allNotes.Count > NotesPerLoad)
            {
                Notes = _allNotes
                    .Take(NotesPerLoad)
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
                if (remainingCount > NotesPerLoad)
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        notesToAdd = _allNotes
                            .Skip(_currentSkipCounter)
                            .Take(NotesPerLoad)
                            .ToList();
                    }
                    else
                    {
                        notesToAdd = _allNotes
                            .Skip(_currentSkipCounter)
                            .Take(NotesPerLoad)
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