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
    public class ToDoCalendarViewModel : BaseNavigableViewModel
    {
        private readonly ICommandResolver _commandResolver;

        public ToDoCalendarViewModel(INavigationService navigationService,
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
            SelectDayCommand = commandResolver.Command<DateTime>(selectedDate => SelectDay(selectedDate, true));
            UnselectDayCommand = commandResolver.Command<DateTime>(unselectedDate => SelectDay(unselectedDate, false));
            ChangeToDoStatusCommand = commandResolver.Command<ToDoViewModel>(model => ChangeToDoStatus(model));
            
            InitializeMonths();
        }

        private void InitializeMonths()
        {
            DateTime currentMonthDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime previousMonthDateTime = DateTime.Now.AddMonths(-1);
            DateTime nextMonthDateTime = DateTime.Now.AddMonths(1);
            
            Months.Add(new MonthViewModel(
                previousMonthDateTime, 
                SelectDayCommand, 
                UnselectDayCommand,
                GetDaysForToDoByDateAndStatus(previousMonthDateTime, ConstantsHelper.Active),
                GetDaysForToDoByDateAndStatus(previousMonthDateTime, ConstantsHelper.Completed)));
            
            Months.Add(new MonthViewModel(
                currentMonthDateTime, 
                SelectDayCommand, 
                UnselectDayCommand,
                GetDaysForToDoByDateAndStatus(currentMonthDateTime, ConstantsHelper.Active),
                GetDaysForToDoByDateAndStatus(currentMonthDateTime, ConstantsHelper.Completed)));
            
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

        public RangeObservableCollection<ToDoViewModel> AllModels { get; }
        public RangeObservableCollection<ToDoViewModel> ActiveModels { get; }
        public RangeObservableCollection<ToDoViewModel> CompletedModels { get; }
        public RangeObservableCollection<ToDoViewModel> FailedModels { get; }
        public RangeObservableCollection<MonthViewModel> Months { get; }

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
                        SelectDay(LastSelectedDate.Value, true);
                        RefreshMonthDaysForToDo(model.ToToDoViewModel(NavigationService, _commandResolver));
                    });

            MessagingCenter.Subscribe<App>(this, ConstantsHelper.UpdateUI, app =>
            {
                SelectDay(LastSelectedDate.Value, true);
            });

            SelectDay(LastSelectedDate.Value, true);
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

            SelectDay(LastSelectedDate.Value, true);

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
            await NavigationService.NavigateToPopupAsync<NewToDoViewModel>(LastSelectedDate);
        }

        private void DeleteToDo(ToDoViewModel viewModel)
        {
            App.ToDoRepository.Value.DeleteModel(viewModel.ToToDoModel());
            AllModels.Remove(viewModel);
            LoadActiveToDoForSelectedDate(LastSelectedDate.Value);
            LoadCompletedToDoForSelectedDate(LastSelectedDate.Value);
            RefreshMonthDaysForToDo(viewModel);
        }

        private void SelectDay(DateTime selectedDate, bool selected)
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

                Months.ElementAt(1).SelectDay(LastSelectedDate.Value);
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
            App.ToDoRepository.Value.Save(model.ToToDoModel());
            SelectDay(LastSelectedDate.Value, true);
            RefreshMonthDaysForToDo(model);
        }

        private void RefreshMonthDaysForToDo(ToDoViewModel model)
        {
            DateTime date = model.WhenHappens;

            var monthToChange = Months.FirstOrDefault(m =>
                m.Days.Any(x => x.CurrentDate.Year == date.Year
                                && x.CurrentDate.Month == date.Month
                                && x.CurrentDate.Day == date.Day));

            if (monthToChange == null)
            {
                return;
            }

            monthToChange.RefreshDaysForActiveAndCompletedToDo(
                GetDaysForToDoByDateAndStatus(monthToChange.CurrentDate, ConstantsHelper.Active), 
                GetDaysForToDoByDateAndStatus(monthToChange.CurrentDate, ConstantsHelper.Completed));
        }
    }
}