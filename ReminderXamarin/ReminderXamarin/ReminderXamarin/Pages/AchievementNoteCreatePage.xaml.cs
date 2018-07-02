﻿using System;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementNoteCreatePage : ContentPage
    {
        private readonly AchievementViewModel _viewModel;

        public AchievementNoteCreatePage(AchievementViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }

        private async void SubmitButton_OnClicked(object sender, EventArgs e)
        {
            bool result = double.TryParse(TimeSpentEditor.Text, out var timeSpent);

            if (result)
            {
                var achievementNoteViewModel = new AchievementNoteViewModel
                {
                    AchievementId = _viewModel.Id,
                    Description = DescriptionEditor.Text,
                    Date = DatePicker.Date,
                    HoursSpent = timeSpent
                };
                _viewModel.UpdateAchievementCommand.Execute(achievementNoteViewModel);
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert(ConstantHelper.Warning, ConstantHelper.TimeParsingError, ConstantHelper.Ok);
            }
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}