using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using System;
using Xamarin.Forms.Xaml;
using System.Linq;

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
            if (this.BindingContext is AchievementsViewModel vm)
            {
                vm.OnAppearing();
            }
        }

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantsHelper.Warning, ConstantsHelper.AchievementDeleteMessage, ConstantsHelper.Ok, ConstantsHelper.Cancel);
            if (result)
            {
                var menuItem = sender as MenuItem;
                var viewModel = menuItem?.CommandParameter as AchievementViewModel;
                viewModel?.DeleteAchievementCommand.Execute(viewModel);

                if (this.BindingContext is AchievementsViewModel vm)
                {
                    vm.OnAppearing();
                }                    
            }
        }

        private async void CreateAchievementButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AchievementCreateView());
        }

        private async void AchievementsList_OnItemSelected(object sender, EventArgs e)
        {
            var container = sender as AbsoluteLayout;
            var hiddenIdLabel = container?.Children.FirstOrDefault(x => x.GetType() == typeof(Label)) as Label;
            if (hiddenIdLabel == null)
            {
                return;
            }

            int.TryParse(hiddenIdLabel.Text, out int id);

            if (this.BindingContext is AchievementsViewModel viewModel)
            {
                var vm = viewModel.Achievements.FirstOrDefault(x => x.Id == id);
                if (vm != null)
                {
                    var achievementDetailPage = new AchievementDetailView(vm);
                    await Navigation.PushAsync(achievementDetailPage);
                }
            }            
        }
    }
}