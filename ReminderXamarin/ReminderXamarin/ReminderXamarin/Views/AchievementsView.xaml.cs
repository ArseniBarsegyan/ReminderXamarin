using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class AchievementsView : ContentPage
    {
        private AchievementsViewModel ViewModel => BindingContext as AchievementsViewModel;
        
        public AchievementsView()
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

        private async void AchievementsList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var model = e.SelectedItem as AchievementViewModel;
            AchievementsListView.SelectedItem = null;
            if (model != null)
            {
                await ViewModel.NavigateToAchievementEditViewCommand.ExecuteAsync(model.Id);
            }
        }
    }
}