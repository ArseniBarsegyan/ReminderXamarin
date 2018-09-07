using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
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
            SelectNoteCommand = new Command<int>(async (id) => await SelectNoteCommandExecute(id));
            SearchCommand = new Command<string>(SearchNotesByDescription);
        }

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
            int.TryParse(Settings.CurrentUserId, out int userId);

            // Fetch all note models from database.
            _allNotes = App.NoteRepository
                .GetAll(userId)
                .ToNoteViewModels()
                .OrderByDescending(x => x.EditDate)
                .ToList();
            // Show recently edited notes at the top of the list.
            Notes = _allNotes.ToObservableCollection();
            // Save filtering.
            SearchNotesByDescription(_currentSearchText);
        }

        private async Task<NoteViewModel> SelectNoteCommandExecute(int id)
        {
            return (await App.NoteRepository.GetByIdAsync(id)).ToNoteViewModel();
        }
    }
}