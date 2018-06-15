using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class NotesPageViewModel : BaseViewModel
    {
        private List<NoteViewModel> _allNotes;
        private string _currentSearchText = string.Empty;

        public NotesPageViewModel()
        {
            Notes = new ObservableCollection<NoteViewModel>();

            RefreshListCommand = new Command(RefreshCommandExecute);
            SelectNoteCommand = new Command<int>(id => SelectNoteCommandExecute(id));
            SearchCommand = new Command<string>(SearchNotesByDescription);
        }

        public bool IsLoading { get; set; }
        public bool IsRefreshing { get; set; }
        public ObservableCollection<NoteViewModel> Notes { get; set; }
        public ICommand RefreshListCommand { get; set; }
        public ICommand SelectNoteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand FilterNotesByDateCommand { get; set; }

        public void OnAppearing()
        {
            LoadNoteFromDatabase();
        }

        private void RefreshCommandExecute()
        {
            IsRefreshing = true;
            LoadNoteFromDatabase();
            IsRefreshing = false;
        }

        private void SearchNotesByDescription(string text)
        {
            _currentSearchText = text;
            Notes = _allNotes
                .Where(x => x.FullDescription.Contains(text))
                .ToObservableCollection();
        }

        private void LoadNoteFromDatabase()
        {
            // Fetch all note models from database.
            _allNotes = App.NoteRepository.GetAll().ToNoteViewModels().ToList();
            // Show recent notes at the top of the list.
            _allNotes.Reverse();
            Notes = _allNotes.ToObservableCollection();
            // Save filtering.
            SearchNotesByDescription(_currentSearchText);
        }

        private NoteViewModel SelectNoteCommandExecute(int id)
        {
            return App.NoteRepository.GetNoteAsync(id).ToNoteViewModel();
        }
    }
}