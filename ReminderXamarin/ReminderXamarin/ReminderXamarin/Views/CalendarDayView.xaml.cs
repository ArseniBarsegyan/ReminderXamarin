using System;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;

namespace ReminderXamarin.Views
{
    public partial class CalendarDayView : ContentView
    {
        private DayViewModel ViewModel => BindingContext as DayViewModel;

        public CalendarDayView()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizerOnTapped(object sender, EventArgs e)
        {
            if (ViewModel.Selected)
            {
                return;
            }
            
            ViewModel.Selected = true;
            ViewModel.DaySelectedCommand.Execute(ViewModel.CurrentDate);
        }
    }
}