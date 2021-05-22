using ReminderXamarin.ViewModels;

using Rm.Data.Data.Entities;

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

        private async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var model = e.SelectedItem as AchievementStep;
            AchievementStepsListView.SelectedItem = null;
            if (model != null)
            {
                await ViewModel.NavigateToAchievementStepEditViewCommand.ExecuteAsync(model);
            }            
        }
    }
}