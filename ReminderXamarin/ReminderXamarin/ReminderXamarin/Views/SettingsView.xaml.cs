﻿using ReminderXamarin.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ConfirmButton.IsVisible = false;
        }

        private void ConfirmButton_OnClicked(object sender, EventArgs e)
        {
            if (BindingContext is SettingsViewModel viewModel)
            {
                viewModel.Pin = PinEntry.Text;
                viewModel.SaveSettingsCommand.Execute(null);
                ConfirmButton.IsVisible = false;
            }            
        }

        private void Switch_OnValueChanged(object sender, ToggledEventArgs e)
        {
            ConfirmButton.IsVisible = true;

            if (BindingContext is SettingsViewModel viewModel)
            {
            }
        }
    }
}