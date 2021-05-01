using ReminderXamarin.Collections;
using ReminderXamarin.Converters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    public partial class ReminderCalendarView : ContentView
    {
        private readonly List<CalendarDayItemView> _calendarViews = new List<CalendarDayItemView>();

        private readonly SwipeGestureRecognizer _leftSwipeGestureRecognizer;
        private readonly SwipeGestureRecognizer _rightSwipeGestureRecognizer;

        private DateTime _initialDate = DateTime.Now;
        private Label _dateLabel;
        private Button _prevMonthButton;
        private Button _nextMonthButton;
        private CalendarDayItemView _lastSelectedView;
        
        public ReminderCalendarView()
        {
            InitializeComponent();
            InitializeCalendar();

            _leftSwipeGestureRecognizer = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            _rightSwipeGestureRecognizer = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
            GestureRecognizers.Add(_leftSwipeGestureRecognizer);
            GestureRecognizers.Add(_rightSwipeGestureRecognizer);
        }        

        private void LeftSwipeGestureRecognizerOnSwiped(object sender, SwipedEventArgs e)
        {
            SwitchMonthForward();
        }

        private void RightSwipeGestureRecognizerOnSwiped(object sender, SwipedEventArgs e)
        {
            SwitchMonthBackward();
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                propertyName: nameof(Command),
                returnType: typeof(ICommand),
                declaringType: typeof(ReminderCalendarView),
                defaultValue: null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty DaysWithActiveToDoInCurrentMonthProperty =
            BindableProperty.Create(
                propertyName: nameof(DaysWithActiveToDoInCurrentMonth),
                returnType: typeof(RangeObservableCollection<DateTime>),
                declaringType: typeof(ReminderCalendarView),
                defaultValue: null);

        public RangeObservableCollection<DateTime> DaysWithActiveToDoInCurrentMonth
        {
            get => (RangeObservableCollection<DateTime>)GetValue(DaysWithActiveToDoInCurrentMonthProperty);
            set => SetValue(DaysWithActiveToDoInCurrentMonthProperty, value);
        }

        public static readonly BindableProperty DaysWithCompletedToDoInCurrentMonthProperty =
            BindableProperty.Create(
                propertyName: nameof(DaysWithCompletedToDoInCurrentMonth),
                returnType: typeof(RangeObservableCollection<DateTime>),
                declaringType: typeof(ReminderCalendarView),
                defaultValue: null);

        public RangeObservableCollection<DateTime> DaysWithCompletedToDoInCurrentMonth
        {
            get => (RangeObservableCollection<DateTime>)GetValue(DaysWithCompletedToDoInCurrentMonthProperty);
            set => SetValue(DaysWithCompletedToDoInCurrentMonthProperty, value);
        }

        public void Subscribe()
        {
            Unsubscribe();

            if (_prevMonthButton != null)
            {
                _prevMonthButton.Clicked += PreviousMonthButtonOnClicked;
            }

            if (_nextMonthButton != null)
            {
                _nextMonthButton.Clicked += NextMonthButtonOnClicked;
            }

            foreach (var view in _calendarViews)
            {
                view.DaySelected += DayViewOnDaySelected;
                view.Subscribe();
            }

            if (_leftSwipeGestureRecognizer != null)
                _leftSwipeGestureRecognizer.Swiped += LeftSwipeGestureRecognizerOnSwiped;

            if (_rightSwipeGestureRecognizer != null)
                _rightSwipeGestureRecognizer.Swiped += RightSwipeGestureRecognizerOnSwiped;
        }

        public void Unsubscribe()
        {
            if (_prevMonthButton != null)
            {
                _prevMonthButton.Clicked -= PreviousMonthButtonOnClicked;
            }

            if (_nextMonthButton != null)
            {
                _nextMonthButton.Clicked -= NextMonthButtonOnClicked;
            }

            foreach (var view in _calendarViews)
            {
                view.DaySelected -= DayViewOnDaySelected;
                view.Unsubscribe();
            }

            if (_leftSwipeGestureRecognizer != null)
                _leftSwipeGestureRecognizer.Swiped -= LeftSwipeGestureRecognizerOnSwiped;

            if (_rightSwipeGestureRecognizer != null)
                _rightSwipeGestureRecognizer.Swiped -= RightSwipeGestureRecognizerOnSwiped;
        }

        private void InitializeCalendar()
        {
            Unsubscribe();
            DateGrid.Children.Clear();            
            _calendarViews.Clear();

            AddDateLabel();
            AddPrevMonthButton();
            AddNextMonthButton();
            AddDaysNames();
            AddCurrentMonth();

            Subscribe();
        }

        private void AddDateLabel()
        {
            _dateLabel = new Label
            {
                Text = _initialDate.ToString("D"),
                HorizontalOptions = LayoutOptions.Center,
                Style = (Style)Resources["DateGridHeaderStyle"]
            };
            Grid.SetRow(_dateLabel, 0);
            Grid.SetColumn(_dateLabel, 1);
            Grid.SetColumnSpan(_dateLabel, 5);
            DateGrid.Children.Add(_dateLabel);
        }

        private void AddPrevMonthButton()
        {
            _prevMonthButton = new Button 
            { 
                Text = @"<",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            _prevMonthButton.SetDynamicResource(BackgroundColorProperty, "ViewBackground");

            Grid.SetRow(_prevMonthButton, 0);
            Grid.SetColumn(_prevMonthButton, 0);
            DateGrid.Children.Add(_prevMonthButton);
        }

        private void AddNextMonthButton()
        {
            _nextMonthButton = new Button 
            { 
                Text = @">",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            _nextMonthButton.SetDynamicResource(BackgroundColorProperty, "ViewBackground");

            Grid.SetRow(_nextMonthButton, 0);
            Grid.SetColumn(_nextMonthButton, 6);
            DateGrid.Children.Add(_nextMonthButton);
        }

        private void AddDaysNames()
        {
            var mondayLabel = new Label 
            { 
                Text = "Mo",
                Style = (Style)Resources["DateGridDayNameStyle"]
            };
            Grid.SetRow(mondayLabel, 1);
            Grid.SetColumn(mondayLabel, 0);
            DateGrid.Children.Add(mondayLabel);

            var tuesdayLabel = new Label
            {
                Text = "Tu",
                Style = (Style)Resources["DateGridDayNameStyle"]
            };
            Grid.SetRow(tuesdayLabel, 1);
            Grid.SetColumn(tuesdayLabel, 1);
            DateGrid.Children.Add(tuesdayLabel);

            var wednesdayLabel = new Label
            {
                Text = "We",
                Style = (Style)Resources["DateGridDayNameStyle"]
            };
            Grid.SetRow(wednesdayLabel, 1);
            Grid.SetColumn(wednesdayLabel, 2);
            DateGrid.Children.Add(wednesdayLabel);

            var thursdayLabel = new Label
            {
                Text = "Th",
                Style = (Style)Resources["DateGridDayNameStyle"]
            };
            Grid.SetRow(thursdayLabel, 1);
            Grid.SetColumn(thursdayLabel, 3);
            DateGrid.Children.Add(thursdayLabel);

            var fridayLabel = new Label
            {
                Text = "Fr",
                Style = (Style)Resources["DateGridDayNameStyle"]
            };
            Grid.SetRow(fridayLabel, 1);
            Grid.SetColumn(fridayLabel, 4);
            DateGrid.Children.Add(fridayLabel);

            var saturdayLabel = new Label
            {
                Text = "Sa",
                Style = (Style)Resources["DateGridSaturdayStyle"]
            };
            Grid.SetRow(saturdayLabel, 1);
            Grid.SetColumn(saturdayLabel, 5);
            DateGrid.Children.Add(saturdayLabel);

            var sundayLabel = new Label
            {
                Text = "Su",
                Style = (Style)Resources["DateGridSundayStyle"]
            };
            Grid.SetRow(sundayLabel, 1);
            Grid.SetColumn(sundayLabel, 6);
            DateGrid.Children.Add(sundayLabel);
        }

        private void AddCurrentMonth()
        {
            int firstDayPosition = FindFirstDayPosition();
            int daysInCurrentMonth = DateTime.DaysInMonth(_initialDate.Year, _initialDate.Month);

            var currentDay = 1;
            const int firstWeekRow = 2;

            for (int i = firstDayPosition; i < DateGrid.ColumnDefinitions.Count; i++)
            {
                AddDay(currentDay, i, firstWeekRow);
                currentDay++;
            }

            const int secondWeekRow = firstWeekRow + 1;
            for (int i = secondWeekRow; i < DateGrid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < DateGrid.ColumnDefinitions.Count; j++)
                {
                    if (currentDay > daysInCurrentMonth)
                    {
                        continue;
                    }

                    AddDay(currentDay, j, i);
                    currentDay++;
                }
            }
        }

        private int FindFirstDayPosition()
        {
            DayOfWeek currentMonthFirstDay = new DateTime(_initialDate.Year, _initialDate.Month, 1).DayOfWeek;

            var daysOfWeekEuOrdered = Enum.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .OrderBy(x => ((int)x + 6) % 7)
                .ToList();

            return daysOfWeekEuOrdered.IndexOf(currentMonthFirstDay);
        }

        private void AddDay(int dayNumber, int row, int column)
        {
            var dateTime = new DateTime(_initialDate.Year, _initialDate.Month, dayNumber);

            var dayView = new CalendarDayItemView
            {
                Date = dateTime,
                CommandParameter = dateTime
            };

            dayView.SetBinding(CalendarDayItemView.CommandProperty,
                new Binding(
                    nameof(Command),
                    source:this));

            dayView.SetBinding(CalendarDayItemView.HasActiveToDoProperty,
                new Binding(
                    nameof(DaysWithActiveToDoInCurrentMonth),
                    BindingMode.OneWay,
                    converter: new DaysWithToDoToHasToDoConverter(),
                    converterParameter: dateTime,
                    source: this));

            dayView.SetBinding(CalendarDayItemView.HasCompletedToDoProperty,
                new Binding(
                    nameof(DaysWithCompletedToDoInCurrentMonth),
                    converter: new DaysWithToDoToHasToDoConverter(),
                    converterParameter: dateTime,
                    source: this));

            if (_initialDate.Day == dayNumber)
            {
                dayView.Select();
                _lastSelectedView = dayView;
            }

            var frame = new Frame
            {
                Content = dayView,
                Padding = 0,
                Margin = 0,
                BorderColor = Color.Transparent,
                HasShadow = false,
                CornerRadius = 2
            };
            _calendarViews.Add(dayView);
            DateGrid.Children.Add(frame, row, column);
        }

        private void DayViewOnDaySelected(object sender, DateTime e)
        {
            if (sender is CalendarDayItemView view)
            {
                if (_lastSelectedView == view)
                {
                    return;
                }
                _lastSelectedView?.Deselect();
                _lastSelectedView = view;
            }
        }

        private void PreviousMonthButtonOnClicked(object sender, EventArgs e)
        {
            SwitchMonthBackward();
        }

        private void NextMonthButtonOnClicked(object sender, EventArgs e)
        {
            SwitchMonthForward();
        }

        private void SwitchMonthBackward()
        {
            var currentYear = _initialDate.Year;
            var currentMonth = _initialDate.Month;

            if (currentMonth == 1)
            {
                currentMonth = 12;
                currentYear--;
            }
            else
            {
                currentMonth--;
            }

            int day = GetCurrentDayInFollowingMonthAfterSwitching(currentYear, currentMonth);
            _initialDate = new DateTime(currentYear, currentMonth, day);
            InitializeCalendar();
        }

        private void SwitchMonthForward()
        {
            var currentYear = _initialDate.Year;
            var currentMonth = _initialDate.Month;

            if (currentMonth == 12)
            {
                currentMonth = 1;
                currentYear++;
            }
            else
            {
                currentMonth++;
            }

            int day = GetCurrentDayInFollowingMonthAfterSwitching(currentYear, currentMonth);
            _initialDate = new DateTime(currentYear, currentMonth, day);
            InitializeCalendar();
        }

        private int GetCurrentDayInFollowingMonthAfterSwitching(int year, int month)
        {
            var daysInFollowingMonth = DateTime.DaysInMonth(year, month);

            if (_initialDate.Day < daysInFollowingMonth)
            {
                return _initialDate.Day;
            }

            return daysInFollowingMonth;
        }
    }
}