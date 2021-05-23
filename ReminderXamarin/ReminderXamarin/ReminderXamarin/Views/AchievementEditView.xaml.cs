using System;
using System.Linq;
using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class AchievementEditView : ContentPage
    {
        private AchievementEditViewModel ViewModel => BindingContext as AchievementEditViewModel;
        
        public AchievementEditView()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            if (ViewModel.IsEditMode)
            {
                ViewModel.IsEditMode = false;
                return true;
            }
            return base.OnBackButtonPressed();
        }

        protected override void OnDisappearing()
        {
            ViewModel.OnDisappearing();
            base.OnDisappearing();
        }

        private async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var viewModel = e.SelectedItem as AchievementStepViewModel;
            AchievementStepsListView.SelectedItem = null;
            if (viewModel != null)
            {
                await ViewModel.NavigateToAchievementStepEditViewCommand.ExecuteAsync(viewModel);
            }            
        }

        private void AddStepButtonOnClicked(object sender, EventArgs e)
        {
            AchievementStepsListView.ScrollTo(
                ViewModel.AchievementSteps.LastOrDefault(), 
                ScrollToPosition.End, 
                true);
        }
    }
}