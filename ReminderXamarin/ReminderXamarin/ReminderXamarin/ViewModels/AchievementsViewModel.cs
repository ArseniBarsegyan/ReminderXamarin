using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using Rm.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class AchievementsViewModel : BaseViewModel
    {
        public AchievementsViewModel()
        {
            Achievements = new ObservableCollection<AchievementViewModel>();

            RefreshListCommand = new Command(Refresh);
            NavigateToAchievementEditViewCommand = new Command<int>(async id => await NavigateToAchievementEditView(id));
        }

        public bool IsRefreshing { get; set; }
        public ObservableCollection<AchievementViewModel> Achievements { get; set; }

        public ICommand RefreshListCommand { get; set; }
        public ICommand NavigateToAchievementEditViewCommand { get; set; }

        public void OnAppearing()
        {
            LoadAchievementsFromDatabase();
        }

        private void Refresh()
        {
            IsRefreshing = true;
            LoadAchievementsFromDatabase();
            IsRefreshing = false;
        }

        private async Task NavigateToAchievementEditView(int id)
        {
            await NavigationService.NavigateToAsync<AchievementEditViewModel>(id);
        }

        private void LoadAchievementsFromDatabase()
        {
            Achievements = App.AchievementRepository
                .GetAll()
                .Where(x => x.UserId == Settings.CurrentUserId)
                .ToAchievementViewModels()
                .OrderByDescending(x => x.GeneralTimeSpent)
                .ToObservableCollection();
        }
    }
}