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
            Children.Clear();
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

        private int _firstDayColumn;

        private int _lastDayRow;
        private int _lastDayColumn;
        
        private void AddCurrentMonth()
        {
            for (int i = 0; i < ViewModel.Days.Count; i++)
            {
                var dayViewModel = ViewModel.Days.ElementAt(i);
                
                if (i == 0)
                {
                    _firstDayColumn = dayViewModel.DayPosition.Column;
                }

                if (i == ViewModel.Days.Count - 1)
                {
                    _lastDayRow = dayViewModel.DayPosition.Row;
                    _lastDayColumn = dayViewModel.DayPosition.Column;
                }
                
                AddDay(dayViewModel);
            }

            //FillEmptyCells();
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

        private void FillEmptyCells()
        {
            if (_firstDayColumn != 0)
            {
                for (int i = 0; i < _firstDayColumn; i++)
                {
                    Children.Add(new ContentView(), i, 0);
                } 
            }

            if (_lastDayColumn != UIConstants.DaysInWeek - 1)
            {
                for (int i = _lastDayColumn; i < UIConstants.DaysInWeek; i++)
                {
                    Children.Add(new ContentView(), i, _lastDayRow);
                }
            }
        }
    }
}