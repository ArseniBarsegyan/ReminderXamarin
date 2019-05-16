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
            if (BindingContext is AchievementsViewModel vm)
            {
                vm.OnAppearing();
            }
        }

        private void AchievementsList_OnItemSelected(object sender, EventArgs e)
        {
            var container = sender as AbsoluteLayout;
            var hiddenIdLabel = container?.Children.FirstOrDefault(x => x.GetType() == typeof(Label)) as Label;
            if (hiddenIdLabel == null)
            {
                return;
            }

            int.TryParse(hiddenIdLabel.Text, out int id);

            if (BindingContext is AchievementsViewModel viewModel)
            {
                viewModel.NavigateToAchievementEditViewCommand.Execute(id);
            }            
        }
    }
}