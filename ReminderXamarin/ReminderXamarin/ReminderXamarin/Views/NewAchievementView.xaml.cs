﻿using Rg.Plugins.Popup.Pages;

namespace ReminderXamarin.Views
{
    public partial class NewAchievementView : PopupPage
    {
        public NewAchievementView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            TitleEntry.Focus();
        }
    }
}