using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class NotesViewModel : BaseViewModel
    {
        private List<NoteViewModel> _allNotes;

        public NotesViewModel()
        {
            Notes = new ObservableCollection<NoteViewModel>();

            SearchText = string.Empty;
            DeleteNoteCommand = new Command<int>(async(id) => await DeleteNote(id));
            RefreshListCommand = new Command(async() => await Refresh());
            SelectNoteCommand = new Command<int>(async id => await SelectNoteCommandExecute(id));
            SearchCommand = new Command(SearchNotesByDescription);
        }

        public string SearchText { get; set; }
        public bool IsRefreshing { get; set; }
        public ObservableCollection<NoteViewModel> Notes { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public ICommand RefreshListCommand { get; set; }
        public ICommand SelectNoteCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public async Task OnAppearing()
        {
            await LoadNotesFromDatabase();
        }

        private async Task DeleteNote(int noteId)
        {
            var noteToDelete = App.NoteRepository.GetNoteAsync(noteId);
            App.NoteRepository.DeleteNote(noteToDelete);
            await OnAppearing();
        }

        private async Task Refresh()
        {
            IsRefreshing = true;
            await LoadNotesFromDatabase();
            IsRefreshing = false;
        }

        private void SearchNotesByDescription()
        {
            Notes = _allNotes
                .Where(x => x.FullDescription.Contains(SearchText))
                .ToObservableCollection();
        }

        private async Task LoadNotesFromDatabase()
        {
            // Fetch all note models from database.
            _allNotes = App.NoteRepository
                .GetAll()
                .Where(x => x.UserId == Settings.CurrentUserId)
                .ToNoteViewModels()
                .OrderByDescending(x => x.CreationDate)
                .ToList();
            // Show recently edited notes at the top of the list.
            Notes = _allNotes.ToObservableCollection();
            // Save filtering.
            SearchNotesByDescription();
        }

        private async Task<NoteViewModel> SelectNoteCommandExecute(int id)
        {
            var note = App.NoteRepository.GetNoteAsync(id);
            return note.ToNoteViewModel();
        }
    }
}