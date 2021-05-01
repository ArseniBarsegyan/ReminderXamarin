using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    public class CalendarDayItemView : ContentView
    {
        private DateTime _dateTime;
        private TapGestureRecognizer _tapGestureRecognizer;
        private Label _dayNumberLabel;
        private bool _selected;

        public CalendarDayItemView()
        {
            AddContent();
            SetDynamicResource(BackgroundColorProperty, "ViewBackground");
        }

        public DateTime Date
        {
            get => _dateTime;
            set
            {
                _dateTime = value;
                _dayNumberLabel.Text = _dateTime.Day.ToString();
                SetCurrentDayTextUnselectedColor();
            }
        }

        public static readonly BindableProperty HasActiveToDoProperty =
            BindableProperty.Create(
                propertyName: nameof(HasActiveToDo),
                returnType: typeof(bool),
                declaringType: typeof(CalendarDayItemView),
                defaultValue: true);

        public bool HasActiveToDo
        {
            get => (bool)GetValue(HasActiveToDoProperty);
            set => SetValue(HasActiveToDoProperty, value);
        }

        public static readonly BindableProperty HasCompletedToDoProperty =
            BindableProperty.Create(
                propertyName: nameof(HasCompletedToDo),
                returnType: typeof(bool),
                declaringType: typeof(CalendarDayItemView),
                defaultValue: true);

        public bool HasCompletedToDo
        {
            get => (bool)GetValue(HasCompletedToDoProperty);
            set => SetValue(HasCompletedToDoProperty, value);
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                propertyName: nameof(Command),
                returnType: typeof(ICommand),
                declaringType: typeof(CalendarDayItemView),
                defaultValue: null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(
                propertyName: nameof(CommandParameter),
                returnType: typeof(object),
                declaringType: typeof(CalendarDayItemView),
                defaultValue: null);

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public event EventHandler<DateTime> DaySelected;

        public void Deselect()
        {
            _selected = false;
            SetDynamicResource(BackgroundColorProperty, "ViewBackground");
            SetCurrentDayTextUnselectedColor();
        }

        public void Select()
        {
            if (_selected)
            {
                return;
            }

            _selected = true;

            SetDynamicResource(BackgroundColorProperty, "CalendarSelectedDate");
            SetCurrentDayTextSelectedColor();
            Command?.Execute(CommandParameter);
            DaySelected?.Invoke(this, Date);
        }

        public void Subscribe()
        {
            _tapGestureRecognizer.Tapped += TapGestureRecognizerOnTapped;
        }

        public void Unsubscribe()
        {
            _tapGestureRecognizer.Tapped -= TapGestureRecognizerOnTapped;
        }

        private void AddContent()
        {
            _tapGestureRecognizer = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1
            };
            GestureRecognizers.Add(_tapGestureRecognizer);

            _dayNumberLabel = new Label
            {
                Margin = new Thickness(5,0,0,0),
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };

            var rootLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            rootLayout.Children.Add(_dayNumberLabel);
            var nestedLayout = new StackLayout
            {
                Padding = new Thickness(0,5,0,0),
                Orientation = StackOrientation.Vertical,
                Spacing = 2
            };

            var activeBoxView = new BoxView 
            { 
                BackgroundColor = Color.DarkGreen,
                WidthRequest = 5,
                HeightRequest = 5,
                CornerRadius = 2.5 
            };
            activeBoxView.SetBinding(
                targetProperty: BoxView.IsVisibleProperty, 
                binding: new Binding(
                    path: nameof(HasActiveToDo),
                    BindingMode.OneWay,
                    source: this));

            var completedBoxView = new BoxView
            {
                BackgroundColor = Color.DarkBlue,
                WidthRequest = 5,
                HeightRequest = 5,
                CornerRadius = 2.5
            };
            completedBoxView.SetBinding(
                targetProperty: BoxView.IsVisibleProperty,
                binding: new Binding(
                    path: nameof(HasCompletedToDo),
                    BindingMode.OneWay,
                    source: this));

            nestedLayout.Children.Add(activeBoxView);
            nestedLayout.Children.Add(completedBoxView);

            rootLayout.Children.Add(nestedLayout);
            Content = rootLayout;
        }

        private void TapGestureRecognizerOnTapped(object sender, EventArgs e)
        {
            Select();
        }

        private void SetCurrentDayTextSelectedColor()
        {
            _dayNumberLabel.TextColor = (Color)Application.Current.Resources["CalendarSelectedDateText"];
        }

        private void SetCurrentDayTextUnselectedColor()
        {
            if (Date.DayOfWeek == DayOfWeek.Saturday)
            {
                _dayNumberLabel.TextColor = (Color)Application.Current.Resources["CalendarSaturdayText"];
            }
            else if (Date.DayOfWeek == DayOfWeek.Sunday)
            {
                _dayNumberLabel.TextColor = (Color)Application.Current.Resources["CalendarSundayText"];
            }
            else
            {
                _dayNumberLabel.TextColor = (Color)Application.Current.Resources["TextCommon"];
            }
        }
    }
}
