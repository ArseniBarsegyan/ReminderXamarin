using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Models;
using Rm.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class ToDoTabbedViewModel : BaseViewModel
    {
        public ToDoTabbedViewModel()
        {
            ActiveModels = new ObservableCollection<ToDoViewModel>();
            CompletedModels = new ObservableCollection<ToDoViewModel>();

            RefreshListCommand = new Command(RefreshCommandExecute);
            SelectItemCommand = new Command<int>(async id => await SelectItemCommandExecute(id));

            MessagingCenter.Subscribe<App>(this, ConstantsHelper.UpdateUI, _ =>
            {
                LoadModelsFromDatabase();
            });
        }

        public void OnAppearing()
        {
            LoadModelsFromDatabase();
        }

        public bool IsLoading { get; set; }
        public bool IsRefreshing { get; set; }

        public ObservableCollection<ToDoViewModel> ActiveModels { get; set; }
        public ObservableCollection<ToDoViewModel> CompletedModels { get; set; }

        public ICommand RefreshListCommand { get; set; }
        public ICommand ShowDetailsCommand { get; set; }
        public ICommand SelectItemCommand { get; set; }

        private void RefreshCommandExecute()
        {
            IsRefreshing = true;
            LoadModelsFromDatabase();
            IsRefreshing = false;
        }

        private async Task<ToDoViewModel> SelectItemCommandExecute(int id)
        {
            return App.ToDoRepository.GetToDoAsync(id).ToToDoViewModel();
        }

        private void LoadModelsFromDatabase()
        {
            var allModels = App.ToDoRepository
                .GetAll()
                .Where(x => x.UserId == Settings.CurrentUserId)
                .ToToDoViewModels()
                .ToList();

            ActiveModels = allModels.Where(x => x.Status == ToDoStatus.Active)
                .OrderByDescending(x => x.WhenHappens)
                .ToObservableCollection();

            CompletedModels = allModels.Where(x => x.Status == ToDoStatus.Completed)
                .OrderByDescending(x => x.WhenHappens)
                .ToObservableCollection();
        }
    }
}