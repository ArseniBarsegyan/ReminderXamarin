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
        private readonly CalendarViewModel _calendarViewModel;
        private ToDoRepository ToDoRepository => App.ToDoRepository.Value;
        
        public ToDoCalendarViewModel(
            INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _commandResolver = commandResolver;
            _calendarViewModel = new CalendarViewModel(commandResolver, this);
            Months = CalendarViewModel.Months;
            SelectDayCommand = CalendarViewModel.SelectDayCommand;
            SelectCurrentDayCommand = CalendarViewModel.SelectCurrentDayCommand;

            AllModels = new RangeObservableCollection<ToDoViewModel>();
            ActiveModels = new RangeObservableCollection<ToDoViewModel>();
            CompletedModels = new RangeObservableCollection<ToDoViewModel>();
            FailedModels = new RangeObservableCollection<ToDoViewModel>();

            CreateToDoCommand = commandResolver.AsyncCommand(CreateToDo);
            DeleteToDoCommand = commandResolver.Command<ToDoViewModel>(DeleteToDo);
            ChangeToDoStatusCommand = commandResolver.Command<ToDoViewModel>(ChangeToDoStatus);
        }

        public CalendarViewModel CalendarViewModel => _calendarViewModel;
        public int CurrentMonthIndex => _calendarViewModel.CurrentMonthIndex;
        public RangeObservableCollection<ToDoViewModel> AllModels { get; }
        public RangeObservableCollection<ToDoViewModel> ActiveModels { get; }
        public RangeObservableCollection<ToDoViewModel> CompletedModels { get; }
        public RangeObservableCollection<ToDoViewModel> FailedModels { get; }
        public RangeObservableCollection<MonthViewModel> Months { get; }

        public ICommand SelectCurrentDayCommand { get; }
        public ICommand CreateToDoCommand { get; }
        public ICommand DeleteToDoCommand { get; }
        public ICommand SelectDayCommand { get; }
        public ICommand ChangeToDoStatusCommand { get; }

        public void OnAppearing()
        {
            _calendarViewModel.OnAppearing();
            
            MessagingCenter.Subscribe<NewToDoViewModel, ToDoModel>(this,
                ConstantsHelper.ToDoItemCreated, (vm, model) =>
                {
                    RefreshMonthDaysForToDo(model.ToToDoViewModel(_commandResolver));
                });
        }

        public void OnDisappearing()
        {
            _calendarViewModel.OnDisappearing();
            
            MessagingCenter.Unsubscribe<NewToDoViewModel>(this,
                ConstantsHelper.ToDoItemCreated);
        }

        public MonthViewModel GetMonthByDate(DateTime dateTime)
            => _calendarViewModel.GetMonthByDate(dateTime);

        public void LoadAllToDoForSelectedDate(DateTime selectedDate)
        {
            var allToDos = GetAllToDoFromDatabase(x =>
                x.WhenHappens.Year == selectedDate.Year
                && x.WhenHappens.Month == selectedDate.Month
                && x.WhenHappens.Day == selectedDate.Day);

            AllModels.ReplaceRangeWithoutUpdating(allToDos);
            AllModels.RaiseCollectionChanged();
        }

        public void LoadActiveToDoForSelectedDate(DateTime selectedDate)
        {
            ActiveModels.ReplaceRangeWithoutUpdating(
                GetToDoModelsByCondition(x => x.Status == ToDoStatus.Active
                && x.WhenHappens.Year == selectedDate.Year
                && x.WhenHappens.Month == selectedDate.Month
                && x.WhenHappens.Day == selectedDate.Day));

            ActiveModels.RaiseCollectionChanged();
        }

        public void LoadCompletedToDoForSelectedDate(DateTime selectedDate)
        {
            CompletedModels.ReplaceRangeWithoutUpdating(
                GetToDoModelsByCondition(x => x.Status == ToDoStatus.Completed
                                              & x.WhenHappens.Year == selectedDate.Year
                                              && x.WhenHappens.Month == selectedDate.Month
                                              && x.WhenHappens.Day == selectedDate.Day));

            CompletedModels.RaiseCollectionChanged();
        }

        public List<int> GetDaysWithToDoByDateAndStatus(DateTime dateTime, string status)
        {
            return GetAllToDoFromDatabase(
                    x => x.Status == status &&
                         x.WhenHappens.Year == dateTime.Year
                         && x.WhenHappens.Month == dateTime.Month)
                .Select(x => x.WhenHappens.Day)
                .ToList();
        }
        
        private void ChangeToDoStatus(ToDoViewModel model)
        {
            switch (model.Status)
            {
                case ToDoStatus.Active:
                    model.Status = ToDoStatus.Completed;
                    break;
                case ToDoStatus.Completed:
                    model.Status = ToDoStatus.Active;
                    break;
            }
            ToDoRepository.Save(model.ToToDoModel());
            _calendarViewModel.SelectDayCommand.Execute(_calendarViewModel.LastSelectedDate);
            RefreshMonthDaysForToDo(model);
        }
        
        private IEnumerable<ToDoViewModel> GetAllToDoFromDatabase(Func<ToDoModel, bool> predicate)
        {
            return ToDoRepository
                .GetAll()
                .Where(x => x.UserId == Settings.CurrentUserId)
                .Where(predicate)
                .ToToDoViewModels(_commandResolver);
        }

        private IEnumerable<ToDoViewModel> GetToDoModelsByCondition(Func<ToDoViewModel, bool> predicate) =>
            AllModels
                .Where(predicate)
                .OrderByDescending(x => x.WhenHappens);

        private async Task CreateToDo()
        {
            await NavigationService.NavigateToPopupAsync<NewToDoViewModel>(CalendarViewModel.LastSelectedDate);
        }

        private void DeleteToDo(ToDoViewModel viewModel)
        {
            ToDoRepository.DeleteModel(viewModel.ToToDoModel());
            AllModels.Remove(viewModel);
            LoadActiveToDoForSelectedDate(CalendarViewModel.LastSelectedDate);
            LoadCompletedToDoForSelectedDate(CalendarViewModel.LastSelectedDate);
            RefreshMonthDaysForToDo(viewModel);
        }

        private void RefreshMonthDaysForToDo(ToDoViewModel model)
        {
            DateTime date = model.WhenHappens;

            var monthToChange = Months.FirstOrDefault(
                m => m.Days.Any(x => x.CurrentDate.Year == date.Year
                                     && x.CurrentDate.Month == date.Month
                                     && x.CurrentDate.Day == date.Day));

            monthToChange?.RefreshDaysForActiveAndCompletedToDo(
                GetDaysWithToDoByDateAndStatus(monthToChange.CurrentDate, ConstantsHelper.Active), 
                GetDaysWithToDoByDateAndStatus(monthToChange.CurrentDate, ConstantsHelper.Completed));
        }
    }
}