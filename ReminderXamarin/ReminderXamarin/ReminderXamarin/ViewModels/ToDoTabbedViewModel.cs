using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using ReminderXamarin.Enums;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class ToDoTabbedViewModel : BaseViewModel
    {
        public ToDoTabbedViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            ActiveModels = new ObservableCollection<ToDoViewModel>();
            CompletedModels = new ObservableCollection<ToDoViewModel>();

            RefreshListCommand = new Command(RefreshCommandExecute);
            SelectItemCommand = new Command<int>(async id => await SelectItem(id));

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

        public ICommand RefreshListCommand { get; }
        public ICommand ShowDetailsCommand { get; }
        public ICommand SelectItemCommand { get; }

        private void RefreshCommandExecute()
        {
            IsRefreshing = true;
            LoadModelsFromDatabase();
            IsRefreshing = false;
        }

        private async Task<ToDoViewModel> SelectItem(int id)
        {
            return App.ToDoRepository.Value.GetToDoAsync(id).ToToDoViewModel(NavigationService);
        }

        private void LoadModelsFromDatabase()
        {
            var allModels = App.ToDoRepository.Value
                .GetAll()
                .Where(x => x.UserId == Settings.CurrentUserId)
                .ToToDoViewModels(NavigationService)
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