using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class ToDoPageViewModel : BaseViewModel
    {
        public ToDoPageViewModel()
        {
            ToDoViewModels = new ObservableCollection<ToDoViewModel>();

            RefreshListCommand = new Command(RefreshCommandExecute);
            SelectItemCommand = new Command<int>(id => SelectItemCommandExecute(id));
        }

        public void OnAppearing()
        {
            LoadModelsFromDatabase();
        }

        public bool IsLoading { get; set; }
        public bool IsRefreshing { get; set; }
        public ObservableCollection<ToDoViewModel> ToDoViewModels { get; set; }
        public ICommand RefreshListCommand { get; set; }
        public ICommand ShowDetailsCommand { get; set; }
        public ICommand SelectItemCommand { get; set; }

        private void RefreshCommandExecute()
        {
            IsRefreshing = true;
            LoadModelsFromDatabase();
            IsRefreshing = false;
        }

        private ToDoViewModel SelectItemCommandExecute(int id)
        {
            return App.ToDoRepository.GetToDoAsync(id).ToToDoViewModel();
        }

        private void LoadModelsFromDatabase()
        {
            int.TryParse(Settings.CurrentUserId, out int userId);

            ToDoViewModels = App.ToDoRepository
                .GetAll()
                .Where(x => x.UserId == userId)
                .ToToDoViewModels()
                .OrderBy(x => x.Priority)
                .ThenByDescending(x => x.WhenHappens)
                .ToObservableCollection();
        }
    }
}