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
            _lastSelectedDate = DateTime.Now;

            AllModels = new RangeObservableCollection<ToDoViewModel>();
            ActiveModels = new RangeObservableCollection<ToDoViewModel>();
            CompletedModels = new RangeObservableCollection<ToDoViewModel>();
            FailedModels = new RangeObservableCollection<ToDoViewModel>();
            DaysWithActiveToDoInCurrentMonth = new RangeObservableCollection<DateTime>();
            DaysWithCompletedToDoInCurrentMonth = new RangeObservableCollection<DateTime>();

            RefreshListCommand = commandResolver.Command(Refresh);
            SelectItemCommand = commandResolver.Command<int>(id => SelectItem(id));
            CreateToDoCommand = commandResolver.AsyncCommand(CreateToDo);
            DeleteToDoCommand = commandResolver.Command<ToDoViewModel>(DeleteToDo);
            SelectDayCommand = commandResolver.Command<DateTime>(selectedDate => SelectDay(selectedDate));
            ChangeToDoStatusCommand = commandResolver.Command<ToDoViewModel>(model => ChangeToDoStatus(model));
        }

        public bool IsLoading { get; set; }
        public bool IsRefreshing { get; set; }

        public RangeObservableCollection<ToDoViewModel> AllModels { get; private set; }
        public RangeObservableCollection<ToDoViewModel> ActiveModels { get; private set; }
        public RangeObservableCollection<ToDoViewModel> CompletedModels { get; private set; }
        public RangeObservableCollection<ToDoViewModel> FailedModels { get; private set; }
        public RangeObservableCollection<DateTime> DaysWithActiveToDoInCurrentMonth { get; private set; }
        public RangeObservableCollection<DateTime> DaysWithCompletedToDoInCurrentMonth { get; private set; }

        public ICommand RefreshListCommand { get; }
        public ICommand ShowDetailsCommand { get; }
        public ICommand SelectItemCommand { get; }
        public ICommand CreateToDoCommand { get; }
        public ICommand DeleteToDoCommand { get; }
        public ICommand SelectDayCommand { get; }
        public ICommand ChangeToDoStatusCommand { get; }

        public void OnAppearing()
        {
            MessagingCenter.Subscribe<NewToDoViewModel>(this,
                    ConstantsHelper.ToDoItemCreated, (vm) => SelectDay(_lastSelectedDate.Value));

            MessagingCenter.Subscribe<App>(this, ConstantsHelper.UpdateUI, app =>
            {
                SelectDay(_lastSelectedDate.Value);
            });

            SelectDay(_lastSelectedDate.Value);
        }

        public void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<NewToDoViewModel>(this,
                    ConstantsHelper.ToDoItemCreated);

            MessagingCenter.Unsubscribe<App>(this,
                ConstantsHelper.UpdateUI);
        }

        private void Refresh()
        {
            IsRefreshing = true;

            SelectDay(_lastSelectedDate.Value);

            IsRefreshing = false;
        }

        private ToDoViewModel SelectItem(int id)
        {
            return App.ToDoRepository.Value.GetToDoAsync(id)
                .ToToDoViewModel(NavigationService, _commandResolver);
        }

        private IEnumerable<ToDoViewModel> GetAllToDoFromDatabase(Func<ToDoModel, bool> predicate)
        {
            return App.ToDoRepository.Value
                    .GetAll()
                    .Where(x => x.UserId == Settings.CurrentUserId)
                    .Where(predicate)
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
            LoadActiveToDoForSelectedDate(_lastSelectedDate.Value);
            LoadCompletedToDoForSelectedDate(_lastSelectedDate.Value);
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
            var allToDos = GetAllToDoFromDatabase(x =>
                x.WhenHappens.Year == selectedDate.Year
                && x.WhenHappens.Month == selectedDate.Month
                && x.WhenHappens.Day == selectedDate.Day);

            AllModels.ReplaceRangeWithoutUpdating(allToDos);
            AllModels.RaiseCollectionChanged();

            DaysWithActiveToDoInCurrentMonth.ReplaceRangeWithoutUpdating(
                allToDos.Where(x => x.Status == ToDoStatus.Active)
                .Select(x => new DateTime(x.WhenHappens.Year, x.WhenHappens.Month, x.WhenHappens.Day)));
            DaysWithActiveToDoInCurrentMonth.RaiseCollectionChanged();

            DaysWithCompletedToDoInCurrentMonth.ReplaceRangeWithoutUpdating(
                allToDos.Where(x => x.Status == ToDoStatus.Completed)
                .Select(x => new DateTime(x.WhenHappens.Year, x.WhenHappens.Month, x.WhenHappens.Day)));
            DaysWithCompletedToDoInCurrentMonth.RaiseCollectionChanged();
        }

        private void LoadActiveToDoForSelectedDate(DateTime selectedDate)
        {
            ActiveModels.ReplaceRangeWithoutUpdating(
                GetModelsByCondition(x => x.Status == ToDoStatus.Active
                && x.WhenHappens.Year == selectedDate.Year
                && x.WhenHappens.Month == selectedDate.Month
                && x.WhenHappens.Day == selectedDate.Day));

            ActiveModels.RaiseCollectionChanged();
        }

        private void LoadCompletedToDoForSelectedDate(DateTime selectedDate)
        {
            CompletedModels.ReplaceRangeWithoutUpdating(
                GetModelsByCondition(x => x.Status == ToDoStatus.Completed
                & x.WhenHappens.Year == selectedDate.Year
                && x.WhenHappens.Month == selectedDate.Month
                && x.WhenHappens.Day == selectedDate.Day));

            CompletedModels.RaiseCollectionChanged();
        }

        private void ChangeToDoStatus(ToDoViewModel model)
        {
            if (model.Status == ToDoStatus.Active)
            {
                model.Status = ToDoStatus.Completed;
            }
            else if (model.Status == ToDoStatus.Completed)
            {
                model.Status = ToDoStatus.Active;
            }
            App.ToDoRepository.Value.Save(model.ToToDoModel());
            SelectDay(_lastSelectedDate.Value);
        }
    }
}