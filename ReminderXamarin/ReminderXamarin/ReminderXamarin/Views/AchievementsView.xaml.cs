using System.Linq;
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

        private async void AchievementsCollectionOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = e.CurrentSelection.FirstOrDefault() as AchievementViewModel;
            AchievementsCollection.SelectedItem = null;
            if (model != null)
            {
                await ViewModel.NavigateToAchievementEditViewCommand.ExecuteAsync(model.Id);
            }
        }
    }
}