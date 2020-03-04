using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
using ReminderXamarin.Enums;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Helpers;

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class ToDoTabbedViewModel : BaseViewModel
    {
        private readonly ICommandResolver _commandResolver;

        public ToDoTabbedViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _commandResolver = commandResolver;

            ActiveModels = new ObservableCollection<ToDoViewModel>();
            CompletedModels = new ObservableCollection<ToDoViewModel>();

            RefreshListCommand = commandResolver.Command(RefreshCommandExecute);
            SelectItemCommand = commandResolver.Command<int>(id => SelectItem(id));

            MessagingCenter.Subscribe<App>(this, ConstantsHelper.UpdateUI, _ =>
            {
                LoadModelsFromDatabase();
            });
        }

        public bool IsLoading { get; set; }
        public bool IsRefreshing { get; set; }

        public ObservableCollection<ToDoViewModel> ActiveModels { get; set; }
        public ObservableCollection<ToDoViewModel> CompletedModels { get; set; }

        public ICommand RefreshListCommand { get; }
        public ICommand ShowDetailsCommand { get; }
        public ICommand SelectItemCommand { get; }

        public void OnAppearing()
        {
            LoadModelsFromDatabase();
        }        

        private void RefreshCommandExecute()
        {
            IsRefreshing = true;
            LoadModelsFromDatabase();
            IsRefreshing = false;
        }

        private ToDoViewModel SelectItem(int id)
        {
            return App.ToDoRepository.Value.GetToDoAsync(id)
                .ToToDoViewModel(NavigationService, _commandResolver);
        }

        private void LoadModelsFromDatabase()
        {
            var allModels = App.ToDoRepository.Value
                .GetAll()
                .Where(x => x.UserId == Settings.CurrentUserId)
                .ToToDoViewModels(NavigationService, _commandResolver)
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