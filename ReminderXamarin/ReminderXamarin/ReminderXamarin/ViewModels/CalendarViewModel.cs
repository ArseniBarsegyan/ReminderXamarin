using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using ReminderXamarin.Collections;
using ReminderXamarin.Core.Interfaces.Commanding;
using Rm.Data.Data.Entities;
using Rm.Helpers;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        private const int CachedMonthCount = 7;
        private readonly ToDoCalendarViewModel _toDoCalendarViewModel;
        private MonthViewModel _currentMonth;

        public CalendarViewModel(
            ICommandResolver commandResolver,
            ToDoCalendarViewModel toDoCalendarViewModel)
        {
            _toDoCalendarViewModel = toDoCalendarViewModel;
            
            Months = new RangeObservableCollection<MonthViewModel>();
            SelectDayCommand = commandResolver.Command<DateTime>(SelectDay);
            SelectCurrentDayCommand = commandResolver.Command(SelectCurrentDay);
            RefreshListCommand = commandResolver.Command(Refresh);
            
            InitializeMonths(DateTime.Now);
        }
        
        public DateTime LastSelectedDate { get; set; } = DateTime.Now;
        public DateTime CurrentDate => DateTime.Now;
        public int CurrentMonthIndex => Months.IndexOf(_currentMonth);
        public RangeObservableCollection<MonthViewModel> Months { get; }
        public bool IsRefreshing { get; set; }
        public ICommand SelectCurrentDayCommand { get; }
        public ICommand SelectDayCommand { get; }
        public ICommand RefreshListCommand { get; }
        
        public void OnAppearing()
        {
            MessagingCenter.Subscribe<NewToDoViewModel, ToDoModel>(this,
                ConstantsHelper.ToDoItemCreated, (vm, model) =>
                {
                    SelectDay(LastSelectedDate);
                });

            MessagingCenter.Subscribe<App>(this, ConstantsHelper.UpdateUI, app =>
            {
                SelectDay(LastSelectedDate);
            });

            SelectDay(LastSelectedDate);
        }

        public void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<NewToDoViewModel>(this,
                ConstantsHelper.ToDoItemCreated);

            MessagingCenter.Unsubscribe<App>(this,
                ConstantsHelper.UpdateUI);
        }
        
        public MonthViewModel GetMonthByDate(DateTime dateTime) =>
            Months.FirstOrDefault(x => x.CurrentDate.Year == dateTime.Year
                                       && x.CurrentDate.Month == dateTime.Month);

        public void LoadDataIfNecessary(int appearedItemIndex, bool shouldSelectDay)
        {
            if (Months.Count <= 1)
                return;
            
            if (appearedItemIndex == 0)
            {
                CachePreviousMonths();
            }

            if (appearedItemIndex == CurrentMonthIndex 
                && Months.Contains(_currentMonth))
            {
                if (shouldSelectDay)
                {
                    if (DateTime.Now.Year == _currentMonth.CurrentDate.Year
                        && DateTime.Now.Month == _currentMonth.CurrentDate.Month)
                    {
                        SelectCurrentDay();
                        return;
                    }
                }
            }

            if (appearedItemIndex == Months.Count - 1)
            {
                CacheNextMonths();
            }

            if (shouldSelectDay)
            {
                var appearedMonth = Months.ElementAt(appearedItemIndex);
                SelectDay(appearedMonth.Days.First().CurrentDate);
            }
        }
        
        private void CachePreviousMonths()
        {
            var firstMonth = Months.First();

            for (int i = 1; i < CachedMonthCount; i++)
            {
                var monthToInsertDateTime = firstMonth.CurrentDate.AddMonths(-i);
                var monthToInsert = new MonthViewModel(
                    monthToInsertDateTime,
                    SelectDayCommand,
                    _toDoCalendarViewModel.GetDaysWithToDoByDateAndStatus(monthToInsertDateTime, ConstantsHelper.Active),
                    _toDoCalendarViewModel.GetDaysWithToDoByDateAndStatus(monthToInsertDateTime, ConstantsHelper.Completed));
                Months.Insert(0, monthToInsert);
                Months.RemoveAt(Months.Count - 1);
            }
        }

        private void CacheNextMonths()
        {
            var monthsToAdd = new List<MonthViewModel>();
            var lastMonth = Months.Last();
                
            for (int i = 1; i < CachedMonthCount; i++)
            {
                var monthToAddDateTime = lastMonth.CurrentDate.AddMonths(i);
                var monthToAdd = new MonthViewModel(
                    monthToAddDateTime,
                    SelectDayCommand,
                    _toDoCalendarViewModel.GetDaysWithToDoByDateAndStatus(monthToAddDateTime, ConstantsHelper.Active),
                    _toDoCalendarViewModel.GetDaysWithToDoByDateAndStatus(monthToAddDateTime, ConstantsHelper.Completed));
                    
                monthsToAdd.Add(monthToAdd);
                Months.RemoveAt(0);
            }
            Months.AddRange(monthsToAdd);
        }
        
        private void InitializeMonths(DateTime dateTime)
        {
            InitializeCurrentMonth(dateTime);
            InitializePreviousAndNextMonths(dateTime);
        }

        private void InitializeCurrentMonth(DateTime dateTime)
        {
            var currentDateTime = new DateTime(dateTime.Year, dateTime.Month, 1);
            
            _currentMonth = new MonthViewModel(
                currentDateTime,
                SelectDayCommand,
                _toDoCalendarViewModel.GetDaysWithToDoByDateAndStatus(currentDateTime, ConstantsHelper.Active),
                _toDoCalendarViewModel.GetDaysWithToDoByDateAndStatus(currentDateTime, ConstantsHelper.Completed));
            
            Months.Add(_currentMonth);
        }

        private async Task InitializePreviousAndNextMonths(DateTime dateTime)
        {
            await Task.Delay(50);
            
            var currentDateTime = new DateTime(dateTime.Year, dateTime.Month, 1);
            for (int i = 1; i < CachedMonthCount; i++)
            {
                var previousMonthDateTime = currentDateTime.AddMonths(-i);
                var nextMonthDateTime = currentDateTime.AddMonths(i);

                var previousMonth = new MonthViewModel(
                    previousMonthDateTime,
                    SelectDayCommand,
                    _toDoCalendarViewModel.GetDaysWithToDoByDateAndStatus(
                        previousMonthDateTime, 
                        ConstantsHelper.Active),
                    _toDoCalendarViewModel.GetDaysWithToDoByDateAndStatus(
                        previousMonthDateTime, 
                        ConstantsHelper.Completed));

                var nextMonth = new MonthViewModel(
                    nextMonthDateTime,
                    SelectDayCommand,
                    _toDoCalendarViewModel.GetDaysWithToDoByDateAndStatus(
                        nextMonthDateTime,
                        ConstantsHelper.Active),
                    _toDoCalendarViewModel.GetDaysWithToDoByDateAndStatus(
                        nextMonthDateTime,
                        ConstantsHelper.Completed));

                Device.BeginInvokeOnMainThread(() =>
                {
                    Months.Insert(0, previousMonth);
                    Months.Add(nextMonth);
                });
            }
        }
        
        private void Refresh()
        {
            IsRefreshing = true;
            SelectDay(LastSelectedDate);
            IsRefreshing = false;
        }
        
        private void SelectDay(DateTime selectedDate)
        {
            MoveToSelectedDateTimeIfNecessary(selectedDate);

            LastSelectedDate = selectedDate;
            
            _toDoCalendarViewModel.LoadAllToDoForSelectedDate(selectedDate);
            _toDoCalendarViewModel.LoadActiveToDoForSelectedDate(selectedDate);
            _toDoCalendarViewModel.LoadCompletedToDoForSelectedDate(selectedDate);

            foreach (var monthViewModel in Months)
            {
                monthViewModel.UnselectAllDaysExceptOne(selectedDate);
            }

            var day = Months
                .SelectMany(x => x.Days)
                .First(x => x.CurrentDate.Year == selectedDate.Year
                            && x.CurrentDate.Month == selectedDate.Month
                            && x.CurrentDate.Day == selectedDate.Day);

            day.Selected = true;
        }

        private void SelectCurrentDay()
        {
            ReloadMonthsIfNecessary();
            SelectDay(DateTime.Now);
        }
        
        private void ReloadMonthsIfNecessary()
        {
            if (!Months.Contains(_currentMonth))
            {
                Months.Clear();
                InitializeMonths(DateTime.Now);
            }
        }

        private void MoveToSelectedDateTimeIfNecessary(DateTime dateTime)
        {
            var day = Months.SelectMany(x => x.Days)
                .FirstOrDefault(x => x.CurrentDate.Year == dateTime.Year
                                     && x.CurrentDate.Month == dateTime.Month
                                     && x.CurrentDate.Day == dateTime.Day);

            if (day == null)
            {
                Months.Clear();
                InitializeMonths(dateTime);
            }
        }
    }
}