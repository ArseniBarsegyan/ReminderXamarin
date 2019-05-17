using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<NoteViewModel> _allNotes;

        public NotesViewModel()
        {
            Notes = new ObservableCollection<NoteViewModel>();

            SearchText = string.Empty;
            DeleteNoteCommand = new Command<int>(async id => await DeleteNote(id));
            RefreshListCommand = new Command(Refresh);
            SearchCommand = new Command(SearchNotesByDescription);
            NavigateToEditViewCommand = new Command<int>(async id => await NavigateToEditView(id));
        }

        public string SearchText { get; set; }
        public bool IsRefreshing { get; set; }
        public ObservableCollection<NoteViewModel> Notes { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public ICommand RefreshListCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand NavigateToEditViewCommand { get; set; }

        public void OnAppearing()
        {
            LoadNotesFromDatabase();
        }

        private async Task DeleteNote(int noteId)
        {
            bool result = await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(ConstantsHelper.NoteDeleteMessage,
                ConstantsHelper.Warning, ConstantsHelper.Ok, ConstantsHelper.No);

            if (result)
            {
                var noteToDelete = App.NoteRepository.Value.GetNoteAsync(noteId);
                App.NoteRepository.Value.DeleteNote(noteToDelete);
                OnAppearing();
            }
        }

        private void Refresh()
        {
            IsRefreshing = true;
            LoadNotesFromDatabase();
            IsRefreshing = false;
        }

        private void SearchNotesByDescription()
        {
            Notes = _allNotes
                .Where(x => x.FullDescription.Contains(SearchText))
                .ToObservableCollection();
        }

        private void LoadNotesFromDatabase()
        {
            // Fetch all note models from database.
            _allNotes = App.NoteRepository.Value
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

        private async Task NavigateToEditView(int id)
        {
            await NavigationService.NavigateToAsync<NoteEditViewModel>(id);
        }
    }
}