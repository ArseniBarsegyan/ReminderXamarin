﻿using System;
using ReminderXamarin.Enums;
using Rm.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDoDetailView : ContentPage
    {
        private readonly ToDoViewModel _viewModel;

        public ToDoDetailView(ToDoViewModel viewModel)
        {
            InitializeComponent();
            TimePicker.Time = viewModel.WhenHappens.TimeOfDay;
            BindingContext = viewModel;
            _viewModel = viewModel;

            Title = $"{viewModel.WhenHappens:d}";
            DescriptionEditor.Text = viewModel.Description;
        }

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantsHelper.Warning, ConstantsHelper.ToDoItemDeleteMessage, ConstantsHelper.Ok, ConstantsHelper.Cancel);
            if (result)
            {
                _viewModel.DeleteItemCommand.Execute(_viewModel);
                await Navigation.PopAsync();
            }
        }

        private async void Confirm_OnClicked(object sender, EventArgs e)
        {
            _viewModel.Description = DescriptionEditor.Text;

            var eventDate = DatePicker.Date;
            var eventTime = TimePicker.Time;

            var fullDate = eventDate.Add(eventTime);
            _viewModel.WhenHappens = fullDate;

            if (PriorityPicker.SelectedItem is ToDoStatus status)
            {
                _viewModel.Status = status;
            }
            _viewModel.UpdateItemCommand.Execute(_viewModel);

            await Navigation.PopAsync();
        }
    }
}