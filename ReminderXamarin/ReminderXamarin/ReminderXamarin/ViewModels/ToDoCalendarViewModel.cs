using ReminderXamarin.Collections;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Enums;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class ToDoCalendarViewModel : BaseViewModel
    {
        private readonly ICommandResolver _commandResolver;

        private DateTime? _lastSelectedDate;

        public ToDoCalendarViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _commandResolver = commandResolver;

            AllModels = new RangeObservableCollection<ToDoViewModel>();
            ActiveModels = new RangeObservableCollection<ToDoViewModel>();
            CompletedModels = new RangeObservableCollection<ToDoViewModel>();
            FailedModels = new RangeObservableCollection<ToDoViewModel>();

            RefreshListCommand = commandResolver.Command(RefreshCommandExecute);
            SelectItemCommand = commandResolver.Command<int>(id => SelectItem(id));
            CreateToDoCommand = commandResolver.AsyncCommand(CreateToDo);
            DeleteToDoCommand = commandResolver.Command<ToDoViewModel>(DeleteToDo);
            SelectDayCommand = commandResolver.Command<DateTime>(selectedDate => SelectDay(selectedDate));

            MessagingCenter.Subscribe(this, ConstantsHelper.UpdateUI, (Action<App>)(_ =>
            {
                InitializeAllModels();
            }));
        }

        public bool IsLoading { get; set; }
        public bool IsRefreshing { get; set; }

        public RangeObservableCollection<ToDoViewModel> AllModels { get; private set; }
        public RangeObservableCollection<ToDoViewModel> ActiveModels { get; private set; }
        public RangeObservableCollection<ToDoViewModel> CompletedModels { get; private set; }
        public RangeObservableCollection<ToDoViewModel> FailedModels { get; private set; }

        public ICommand RefreshListCommand { get; }
        public ICommand ShowDetailsCommand { get; }
        public ICommand SelectItemCommand { get; }
        public ICommand CreateToDoCommand { get; }
        public ICommand DeleteToDoCommand { get; }
        public ICommand SelectDayCommand { get; }

        public void OnAppearing()
        {
            MessagingCenter.Subscribe<NewToDoViewModel>(this,
                    ConstantsHelper.ToDoItemCreated, (vm) => InitializeAllModels());

            InitializeAllCollections();
        }

        public void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<NewToDoViewModel>(this,
                    ConstantsHelper.ToDoItemCreated);
        }

        private void RefreshCommandExecute()
        {
            IsRefreshing = true;

            if (_lastSelectedDate != null)
            {
                SelectDay(_lastSelectedDate.Value);
            }
            else
            {
                InitializeAllModels();
            }
            
            IsRefreshing = false;
        }

        private ToDoViewModel SelectItem(int id)
        {
            return App.ToDoRepository.Value.GetToDoAsync(id)
                .ToToDoViewModel(NavigationService, _commandResolver);
        }

        private void InitializeAllCollections()
        {
            InitializeAllModels();
            InitializeActiveModels();
            InitializeCompletedModels();
        }

        private void InitializeAllModels()
        {
            AllModels.Clear();
            AllModels.AddRange(GetAllToDoFromDatabase());
        }   

        private void InitializeActiveModels()
        {
            ActiveModels.Clear();
            ActiveModels.AddRange(GetModelsByCondition(x => x.Status == ToDoStatus.Active));
        }

        private void InitializeCompletedModels()
        {
            CompletedModels.Clear();
            CompletedModels.AddRange(GetModelsByCondition(x => x.Status == ToDoStatus.Completed));
        }

        private IEnumerable<ToDoViewModel> GetAllToDoFromDatabase(Func<ToDoModel, bool> predicate = null)
        {
            if (predicate != null)
            {
                return App.ToDoRepository.Value
                    .GetAll()
                    .Where(x => x.UserId == Settings.CurrentUserId)
                    .Where(predicate)
                    .ToToDoViewModels(NavigationService, _commandResolver);
            }

            return App.ToDoRepository.Value
                    .GetAll()
                    .Where(x => x.UserId == Settings.CurrentUserId)
                    .ToToDoViewModels(NavigationService, _commandResolver);
        }

        private IEnumerable<ToDoViewModel> GetModelsByCondition(Func<ToDoViewModel, bool> predicate) =>
            AllModels
            .Where(predicate)
            .OrderByDescending(x => x.WhenHappens);

        private async Task CreateToDo()
        {
            await NavigationService.NavigateToPopupAsync<NewToDoViewModel>(_lastSelectedDate);
        }

        private void DeleteToDo(ToDoViewModel viewModel)
        {
            App.ToDoRepository.Value.DeleteModel(viewModel.ToToDoModel());
            AllModels.Remove(viewModel);
            InitializeActiveModels();
            InitializeCompletedModels();
        }

        private void SelectDay(DateTime selectedDate)
        {
            _lastSelectedDate = selectedDate;
            LoadAllToDoForSelectedDate(selectedDate);
            LoadActiveToDoForSelectedDate(selectedDate);
            LoadCompletedToDoForSelectedDate(selectedDate);
        }

        private void LoadAllToDoForSelectedDate(DateTime selectedDate)
        {
            AllModels.Clear();
            AllModels.AddRange(GetAllToDoFromDatabase(x =>
                x.WhenHappens.Year == selectedDate.Year
                && x.WhenHappens.Month == selectedDate.Month
                && x.WhenHappens.Day == selectedDate.Day));
        }

        private void LoadActiveToDoForSelectedDate(DateTime selectedDate)
        {
            ActiveModels.Clear();
            ActiveModels.AddRange(GetModelsByCondition(x => x.Status == ToDoStatus.Active
                && x.WhenHappens.Year == selectedDate.Year
                && x.WhenHappens.Month == selectedDate.Month
                && x.WhenHappens.Day == selectedDate.Day));
        }

        private void LoadCompletedToDoForSelectedDate(DateTime selectedDate)
        {
            CompletedModels.Clear();
            CompletedModels.AddRange(GetModelsByCondition(x => x.Status == ToDoStatus.Completed
                & x.WhenHappens.Year == selectedDate.Year
                && x.WhenHappens.Month == selectedDate.Month
                && x.WhenHappens.Day == selectedDate.Day));
        }
    }
}