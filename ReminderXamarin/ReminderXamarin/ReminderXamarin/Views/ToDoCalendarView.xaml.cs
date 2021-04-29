﻿using Xamarin.Forms;

namespace ReminderXamarin.Views
{
    public partial class ToDoCalendarView : ContentPage
    {
        public ToDoCalendarView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ToDoCalendarView vm)
            {
                vm.OnAppearing();
            }
            ReminderCalendarView.Subscribe();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is ToDoCalendarView vm)
            {
                vm.OnDisappearing();
            }
            ReminderCalendarView.Unsubscribe();
        }
    }
}