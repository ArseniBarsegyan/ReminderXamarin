using System.Linq;
using ReminderXamarin.Constants;
using ReminderXamarin.ViewModels;
using ReminderXamarin.Views;
using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    public class CalendarMonthView : Grid
    {
        private MonthViewModel ViewModel => BindingContext as MonthViewModel;
        
        public CalendarMonthView()
        {
            Padding = new Thickness(10, 0, 0, 0);
            InitializeGrid();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            
            if (propertyName == nameof(BindingContext))
            {
                if (ViewModel == null)
                {
                    return;
                }
                
                Children.Clear();
                AddCurrentMonth();
            }
        }

        private void InitializeGrid()
        {
            for (int i = 0; i < UIConstants.DaysInWeek; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Star});
            }

            for (int i = 0; i < UIConstants.WeeksInMounthCount; i++)
            {
                RowDefinitions.Add(new RowDefinition {Height = UIConstants.CalendarDayViewHeight});
            }
        }
        
        private void AddCurrentMonth()
        {
            for (int i = 0; i < ViewModel.Days.Count; i++)
            {
                var dayViewModel = ViewModel.Days.ElementAt(i);
                AddDay(dayViewModel);
            }
        }

        private void AddDay(DayViewModel dayViewModel)
        {
            var view = new CalendarDayView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BindingContext = dayViewModel
            };

            Children.Add(view, dayViewModel.DayPosition.Column, dayViewModel.DayPosition.Row);
        }
    }
}