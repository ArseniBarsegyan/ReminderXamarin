using System;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class BirthdayEditView : ContentPage
    {
        private BirthdayEditViewModel ViewModel => BindingContext as BirthdayEditViewModel;
        private bool _isTranslated;
        
        public BirthdayEditView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.OnDisappearing();
        }

        private void BackgroundImageOnTapped(object sender, EventArgs e)
        {
            if (_isTranslated)
            {
                AnimateOut();
            }
            else
            {
                AnimateIn();
            }
            _isTranslated = !_isTranslated;
        }

        private void AnimateIn()
        {
            BackgroundImage.LayoutTo(new Rectangle(0, 0, Width, 200), 250, Easing.SpringOut);
            PersonImage.TranslateTo(0, 100, 250, Easing.SpringOut);
            PickPersonPhotoImage.TranslateTo(0, 100, 250, Easing.SpringOut);
            PersonInfoLayout.TranslateTo(0, 100, 250, Easing.SpringOut);
        }

        private void AnimateOut()
        {
            BackgroundImage.LayoutTo(new Rectangle(0, 0, Width, 100), 250, Easing.SpringIn);
            PersonImage.TranslateTo(0, 0, 250, Easing.SpringIn);
            PickPersonPhotoImage.TranslateTo(0, 0, 250, Easing.SpringIn);
            PersonInfoLayout.TranslateTo(0, 0, 250, Easing.SpringIn);
        }

        private void MonthPickerOnSelectedIndexChanged(object sender, EventArgs e)
        {
            ViewModel.SelectMonthCommand.Execute(MonthPicker.SelectedItem);
        }

        private void DayPickerOnSelectedIndexChanged(object sender, EventArgs e)
        {
            ViewModel.SelectDayCommand.Execute(DayPicker.SelectedItem);
        }
    }
}