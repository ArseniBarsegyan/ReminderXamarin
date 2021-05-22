using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReminderXamarin.Collections;
using ReminderXamarin.Constants;
using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class MonthViewModel : BaseViewModel
    {
        private readonly ICommand _daySelectedCommand;
        private readonly ICommand _dayUnselectedCommand;
        
        private List<int> DaysWithActiveToDo { get; set; }
        private List<int> DaysWithCompletedToDo { get; set; }

        public MonthViewModel(
            DateTime dateTime,
            ICommand daySelectedCommand,
            ICommand dayUnselectedCommand,
            List<int> daysWithActiveToDo,
            List<int> daysWithCompletedToDo)
        {
            CurrentDate = dateTime;
            Days = new RangeObservableCollection<DayViewModel>();
            _daySelectedCommand = daySelectedCommand;
            _dayUnselectedCommand = dayUnselectedCommand;
            DaysWithActiveToDo = daysWithActiveToDo;
            DaysWithCompletedToDo = daysWithCompletedToDo;
            InitializeDays();
        }

        public RangeObservableCollection<DayViewModel> Days { get; }
        public DateTime CurrentDate { get; }

        public void UnselectAllDaysExceptOne(DateTime selectedDay)
        {
            foreach (var dayViewModel in Days)
            {
                if (dayViewModel.CurrentDate != selectedDay
                    && dayViewModel.Selected)
                {
                    dayViewModel.Selected = false;
                }
            }
        }

        public void SelectDay(DateTime dateTime)
        {
            foreach (var dayViewModel in Days)
            {
                if (dayViewModel.CurrentDate.Year == dateTime.Year
                    && dayViewModel.CurrentDate.Month == dateTime.Month
                    && dayViewModel.CurrentDate.Day == dateTime.Day)
                {
                    dayViewModel.Selected = true;
                }
            }
        }

        private void InitializeDays()
        {
            const int firstWeekRow = 0;
            int currentDayNumber = 1;

            for (int i = firstWeekRow; i < UIConstants.WeeksInMounthCount; i++)
            {
                if (i == firstWeekRow)
                {
                    for (int j = FindFirstDayPosition(); j < UIConstants.DaysInWeek; j++)
                    {
                        AddDay(currentDayNumber, j, firstWeekRow);
                        currentDayNumber++;
                    }
                }
                else
                {
                    for (int k = 0; k < UIConstants.DaysInWeek; k++)
                    {
                        int daysInCurrentMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);
                        
                        if (currentDayNumber > daysInCurrentMonth)
                        {
                            break;
                        }

                        AddDay(currentDayNumber, k, i);
                        currentDayNumber++;
                    }
                }
            }
        }
        
        private int FindFirstDayPosition()
        {
            DayOfWeek currentMonthFirstDay = new DateTime(CurrentDate.Year, CurrentDate.Month, 1).DayOfWeek;

            var daysOfWeekEuOrdered = Enum.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .OrderBy(x => ((int)x + 6) % 7)
                .ToList();

            return daysOfWeekEuOrdered.IndexOf(currentMonthFirstDay);
        }
        
        private void AddDay(int dayNumber, int column, int row)
        {
            bool hasActiveToDo = DaysWithActiveToDo.Contains(dayNumber);
            bool hasCompletedToDo = DaysWithCompletedToDo.Contains(dayNumber);

            Days.Add(new DayViewModel
            {
                CurrentDate = new DateTime(CurrentDate.Year, CurrentDate.Month, dayNumber),
                DayPosition = new DayPosition {Column = column, Row = row},
                DaySelectedCommand = _daySelectedCommand,
                DayUnselectedCommand = _dayUnselectedCommand,
                HasActiveToDo = hasActiveToDo,
                HasCompletedToDo = hasCompletedToDo
            });
        }

        public void RefreshDaysForActiveAndCompletedToDo(List<int> daysWithActiveToDo,
            List<int> daysWithCompletedToDo)
        {
            DaysWithActiveToDo = daysWithActiveToDo;
            DaysWithCompletedToDo = daysWithCompletedToDo;
            
            foreach (var dayViewModel in Days)
            {
                dayViewModel.HasActiveToDo = DaysWithActiveToDo.Contains(dayViewModel.CurrentDate.Day);
                dayViewModel.HasCompletedToDo = DaysWithCompletedToDo.Contains(dayViewModel.CurrentDate.Day);
            }
        }
    }
}
