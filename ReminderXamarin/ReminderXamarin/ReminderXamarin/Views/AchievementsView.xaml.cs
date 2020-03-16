using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementsView : ContentPage
    {
        public AchievementsView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is AchievementsViewModel vm)
            {
                vm.OnAppearing();
            }
        }

        private async void AchievementsList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var model = e.SelectedItem as AchievementViewModel;
            AchievementsListView.SelectedItem = null;
            if (model != null)
            {
                if (BindingContext is AchievementsViewModel viewModel)
                {
                    await viewModel.NavigateToAchievementEditViewCommand.ExecuteAsync(model.Id);
                }
            }
        }
    }
}