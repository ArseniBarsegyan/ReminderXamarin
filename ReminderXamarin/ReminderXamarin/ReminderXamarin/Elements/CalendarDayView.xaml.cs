using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    public partial class CalendarDayView : ContentView
    {
        private DateTime _dateTime;

        public CalendarDayView()
        {
            InitializeComponent();
        }

        public DateTime Date
        {
            get => _dateTime;
            set
            {
                _dateTime = value;
                DayNumberLabel.Text = _dateTime.Day.ToString();
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
                declaringType: typeof(CalendarDayView),
                defaultValue: null);

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public event EventHandler<DateTime> DaySelected;

        public void Deselect()
        {
            BackgroundColor = Color.White;
        }

        private void TapGestureRecognizerOnTapped(object sender, EventArgs e)
        {
            BackgroundColor = (Color)Application.Current.Resources["CalendarSelectedDate"];
            Command?.Execute(CommandParameter);
            DaySelected?.Invoke(this, Date);
        }
    }
}