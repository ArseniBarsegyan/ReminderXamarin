using ReminderXamarin.ViewModels;

using Rm.Data.Data.Entities;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementEditView : ContentPage
    {
        public AchievementEditView()
        {
            InitializeComponent();
        }       

        private async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var model = e.SelectedItem as AchievementStep;
            AchievementStepsListView.SelectedItem = null;
            if (model != null)
            {
                if (BindingContext is AchievementEditViewModel viewModel)
                {
                    await viewModel.NavigateToAchievementStepEditViewCommand.ExecuteAsync(model);
                }
            }            
        }
    }
}