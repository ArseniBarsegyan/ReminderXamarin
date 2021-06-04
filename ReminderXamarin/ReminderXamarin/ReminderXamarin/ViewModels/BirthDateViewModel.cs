using System.Collections.Generic;
using System.Linq;
using MvvmHelpers;
using ReminderXamarin.Collections;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class BirthDateViewModel : BaseViewModel
    {
        private int _daysInCurrentMonth;
        
        public int DaysInCurrentMonth
        {
            get => _daysInCurrentMonth;
            set
            {
                _daysInCurrentMonth = value;

                var days = new List<BirthdayDayViewModel>();
                for (int i = 1; i < _daysInCurrentMonth + 1; i++)
                {
                    days.Add(new BirthdayDayViewModel { Number = i });
                }
                
                Days.ReplaceRangeWithoutUpdating(days);
                Days.RaiseCollectionChanged();
                OnPropertyChanged();
            }
        }
        
        public int MonthNumber { get; set; }
        public string MonthName { get; set; }

        public BirthdayDayViewModel SelectedDay
        {
            get
            {
                var selectedDay = Days.FirstOrDefault(x => x.IsSelected);
                if (selectedDay != null)
                {
                    return selectedDay;
                }

                selectedDay = Days.ElementAt(0);
                selectedDay.IsSelected = true;
                return selectedDay;
            }
        }

        public RangeObservableCollection<BirthdayDayViewModel> Days { get; } = new RangeObservableCollection<BirthdayDayViewModel>();
        
        public void SelectDay(int number)
        {
            foreach (var day in Days)
            {
                day.IsSelected = day.Number == number;

                if (day.IsSelected)
                {
                    OnPropertyChanged(nameof(SelectedDay));
                }
            }
        }
    }
}