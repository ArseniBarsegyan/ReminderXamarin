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
        }

        public void Subscribe()
        {
            _tapGestureRecognizer.Tapped += TapGestureRecognizerOnTapped;
        }

        public void Unsubscribe()
        {
            _tapGestureRecognizer.Tapped -= TapGestureRecognizerOnTapped;
        }

        public DateTime Date
        {
            get => _dateTime;
            set
            {
                _dateTime = value;
                _dayNumberLabel.Text = _dateTime.Day.ToString();
            }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                propertyName: nameof(Command),
                returnType: typeof(ICommand),
                declaringType: typeof(ReminderCalendarView),
                defaultValue: null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(
                propertyName: nameof(CommandParameter),
                returnType: typeof(object),
                declaringType: typeof(CalendarDayItemView),
                defaultValue: null);

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public event EventHandler<DateTime> DaySelected;

        public void Deselect()
        {
            _selected = false;
            BackgroundColor = Color.White;
            _dayNumberLabel.TextColor = (Color)Application.Current.Resources["TextCommon"];
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
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = (Color)Application.Current.Resources["TextCommon"]
            };
            Content = _dayNumberLabel;
        }

        private void TapGestureRecognizerOnTapped(object sender, EventArgs e)
        {
            Select();
        }

        public void Select()
        {
            if (_selected)
            {
                return;
            }

            _selected = true;

            BackgroundColor = (Color)Application.Current.Resources["CalendarSelectedDate"];
            _dayNumberLabel.TextColor = (Color)Application.Current.Resources["CalendarSelectedDateText"];
            Command?.Execute(CommandParameter);
            DaySelected?.Invoke(this, Date);
        }
    }
}
