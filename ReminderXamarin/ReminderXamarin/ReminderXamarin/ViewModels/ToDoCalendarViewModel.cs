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
using Rm.Data.Data.Repositories;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class ToDoCalendarViewModel : BaseNavigableViewModel
    {
        private readonly ICommandResolver _commandResolver;
        private ToDoRepository ToDoRepository => App.ToDoRepository.Value;
        private MonthViewModel _currentMonth;
        
        public ToDoCalendarViewModel(
            INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _commandResolver = commandResolver;

            AllModels = new RangeObservableCollection<ToDoViewModel>();
            ActiveModels = new RangeObservableCollection<ToDoViewModel>();
            CompletedModels = new RangeObservableCollection<ToDoViewModel>();
            FailedModels = new RangeObservableCollection<ToDoViewModel>();
            Months = new RangeObservableCollection<MonthViewModel>();

            RefreshListCommand = commandResolver.Command(Refresh);
            SelectItemCommand = commandResolver.Command<int>(id => SelectItem(id));
            CreateToDoCommand = commandResolver.AsyncCommand(CreateToDo);
            DeleteToDoCommand = commandResolver.Command<ToDoViewModel>(DeleteToDo);
            SelectDayCommand = commandResolver.Command<DateTime>(selectedDate => ToggleDayState(selectedDate, true));
            UnselectDayCommand = commandResolver.Command<DateTime>(unselectedDate => ToggleDayState(unselectedDate, false));
            ChangeToDoStatusCommand = commandResolver.Command<ToDoViewModel>(model => ChangeToDoStatus(model));
            SelectCurrentDayCommand = commandResolver.Command(SelectCurrentDay);
            
            InitializeMonths();
        }

        private void InitializeMonths()
        {
            var currentDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var previousMonthDateTime = currentDateTime.AddMonths(-1);
            var nextMonthDateTime = currentDateTime.AddMonths(1);
            
            Months.Add(new MonthViewModel(
                previousMonthDateTime,
                SelectDayCommand,
                UnselectDayCommand,
                GetDaysForToDoByDateAndStatus(previousMonthDateTime, ConstantsHelper.Active),
                GetDaysForToDoByDateAndStatus(previousMonthDateTime, ConstantsHelper.Completed)));

            _currentMonth = new MonthViewModel(
                currentDateTime,
                SelectDayCommand,
                UnselectDayCommand,
                GetDaysForToDoByDateAndStatus(currentDateTime, ConstantsHelper.Active),
                GetDaysForToDoByDateAndStatus(currentDateTime, ConstantsHelper.Completed));
            Months.Add(_currentMonth);
                
            Months.Add(new MonthViewModel(
                nextMonthDateTime,
                SelectDayCommand,
                UnselectDayCommand,
                GetDaysForToDoByDateAndStatus(nextMonthDateTime, ConstantsHelper.Active),
                GetDaysForToDoByDateAndStatus(nextMonthDateTime, ConstantsHelper.Completed)));
        }

        private List<int> GetDaysForToDoByDateAndStatus(DateTime dateTime, string status)
        {
            return GetAllToDoFromDatabase(
                    x => x.Status == status &&
                         x.WhenHappens.Year == dateTime.Year
                         && x.WhenHappens.Month == dateTime.Month)
                .Select(x => x.WhenHappens.Day)
                .ToList();
        }

        public bool IsRefreshing { get; set; }
        
        public DateTime? LastSelectedDate { get; private set; } = DateTime.Now;
        public DateTime CurrentDate => DateTime.Now;

        public int CurrentMonthIndex => Months.IndexOf(_currentMonth);

        public RangeObservableCollection<ToDoViewModel> AllModels { get; }
        public RangeObservableCollection<ToDoViewModel> ActiveModels { get; }
        public RangeObservableCollection<ToDoViewModel> CompletedModels { get; }
        public RangeObservableCollection<ToDoViewModel> FailedModels { get; }
        public RangeObservableCollection<MonthViewModel> Months { get; }

        public ICommand SelectCurrentDayCommand { get; }
        public ICommand RefreshListCommand { get; }
        public ICommand SelectItemCommand { get; }
        public ICommand CreateToDoCommand { get; }
        public ICommand DeleteToDoCommand { get; }
        public ICommand SelectDayCommand { get; }
        public ICommand UnselectDayCommand { get; }
        public ICommand ChangeToDoStatusCommand { get; }

        public void OnAppearing()
        {
            MessagingCenter.Subscribe<NewToDoViewModel, ToDoModel>(this,
                    ConstantsHelper.ToDoItemCreated, (vm, model) =>
                    {
                        ToggleDayState(LastSelectedDate.Value, true);
                        RefreshMonthDaysForToDo(model.ToToDoViewModel(_commandResolver));
                    });

            MessagingCenter.Subscribe<App>(this, ConstantsHelper.UpdateUI, app =>
            {
                ToggleDayState(LastSelectedDate.Value, true);
            });

            ToggleDayState(LastSelectedDate.Value, true);
        }

        public void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<NewToDoViewModel>(this,
                    ConstantsHelper.ToDoItemCreated);

            MessagingCenter.Unsubscribe<App>(this,
                ConstantsHelper.UpdateUI);
        }

        public void LoadDataIfNecessary(int appearedItemIndex, int previousItemIndex)
        {
            if (appearedItemIndex == 0)
            {
                var firstMonth = Months.First();
                var monthToInsertDateTime = firstMonth.CurrentDate.AddMonths(-1);
                var monthToInsert = new MonthViewModel(
                    monthToInsertDateTime,
                    SelectDayCommand,
                    UnselectDayCommand,
                    GetDaysForToDoByDateAndStatus(monthToInsertDateTime, ConstantsHelper.Active),
                    GetDaysForToDoByDateAndStatus(monthToInsertDateTime, ConstantsHelper.Completed));
                Months.Insert(0, monthToInsert);
            }

            if (appearedItemIndex == CurrentMonthIndex)
            {
                SelectCurrentDay();
                return;
            }

            if (appearedItemIndex == Months.Count - 1)
            {
                var lastMonth = Months.Last();
                var monthToAddDateTime = lastMonth.CurrentDate.AddMonths(1);
                var monthToAdd = new MonthViewModel(
                    monthToAddDateTime,
                    SelectDayCommand,
                    UnselectDayCommand,
                    GetDaysForToDoByDateAndStatus(monthToAddDateTime, ConstantsHelper.Active),
                    GetDaysForToDoByDateAndStatus(monthToAddDateTime, ConstantsHelper.Completed));
                Months.Add(monthToAdd);
            }

            var appearedMonth = Months.ElementAt(appearedItemIndex);
            ToggleDayState(appearedMonth.Days.First().CurrentDate, true);
        }

        private void Refresh()
        {
            IsRefreshing = true;

            ToggleDayState(LastSelectedDate.Value, true);

            IsRefreshing = false;
        }

        private ToDoViewModel SelectItem(int id)
        {
            return ToDoRepository.GetToDoAsync(id)
                .ToToDoViewModel(_commandResolver);
        }

        private IEnumerable<ToDoViewModel> GetAllToDoFromDatabase(Func<ToDoModel, bool> predicate)
        {
            return ToDoRepository
                    .GetAll()
                    .Where(x => x.UserId == Settings.CurrentUserId)
                    .Where(predicate)
                    .ToToDoViewModels(_commandResolver);
        }

        private IEnumerable<ToDoViewModel> GetModelsByCondition(Func<ToDoViewModel, bool> predicate) =>
            AllModels
            .Where(predicate)
            .OrderByDescending(x => x.WhenHappens);

        private async Task CreateToDo()
        {
            await NavigationService.NavigateToPopupAsync<NewToDoViewModel>(LastSelectedDate);
        }

        private void DeleteToDo(ToDoViewModel viewModel)
        {
            ToDoRepository.DeleteModel(viewModel.ToToDoModel());
            AllModels.Remove(viewModel);
            LoadActiveToDoForSelectedDate(LastSelectedDate.Value);
            LoadCompletedToDoForSelectedDate(LastSelectedDate.Value);
            RefreshMonthDaysForToDo(viewModel);
        }

        private void ToggleDayState(DateTime selectedDate, bool selected)
        {
            if (selected)
            {
                LastSelectedDate = selectedDate;
                LoadAllToDoForSelectedDate(selectedDate);
                LoadActiveToDoForSelectedDate(selectedDate);
                LoadCompletedToDoForSelectedDate(selectedDate);
                
                foreach (var monthViewModel in Months)
                {
                    monthViewModel.UnselectAllDaysExceptOne(selectedDate);
                }

                var day = Months
                    .SelectMany(x => x.Days)
                    .First(x => x.CurrentDate.Year == LastSelectedDate.Value.Year
                        && x.CurrentDate.Month == LastSelectedDate.Value.Month
                        && x.CurrentDate.Day == LastSelectedDate.Value.Day);
                
                day.Selected = true;
            }
            else
            {
                LastSelectedDate = null;
            }
        }

        private void LoadAllToDoForSelectedDate(DateTime selectedDate)
        {
            var allToDos = GetAllToDoFromDatabase(x =>
                x.WhenHappens.Year == selectedDate.Year
                && x.WhenHappens.Month == selectedDate.Month
                && x.WhenHappens.Day == selectedDate.Day);

            AllModels.ReplaceRangeWithoutUpdating(allToDos);
            AllModels.RaiseCollectionChanged();
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
            ToDoRepository.Save(model.ToToDoModel());
            ToggleDayState(LastSelectedDate.Value, true);
            RefreshMonthDaysForToDo(model);
        }

        private void RefreshMonthDaysForToDo(ToDoViewModel model)
        {
            DateTime date = model.WhenHappens;

            var monthToChange = Months.FirstOrDefault(m =>
                m.Days.Any(x => x.CurrentDate.Year == date.Year
                                && x.CurrentDate.Month == date.Month
                                && x.CurrentDate.Day == date.Day));

            monthToChange?.RefreshDaysForActiveAndCompletedToDo(
                GetDaysForToDoByDateAndStatus(monthToChange.CurrentDate, ConstantsHelper.Active), 
                GetDaysForToDoByDateAndStatus(monthToChange.CurrentDate, ConstantsHelper.Completed));
        }

        private void SelectCurrentDay()
        {
            ToggleDayState(DateTime.Now, true);
        }
    }
}