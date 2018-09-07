using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class AchievementsPageViewModel : BaseViewModel
    {
        public AchievementsPageViewModel()
        {
            Achievements = new ObservableCollection<AchievementViewModel>();

            RefreshListCommand = new Command(RefreshCommandExecute);
            SelectAchievementCommand = new Command<int>(async(id) => await SelectAchievementCommandExecute(id));
        }

        public bool IsRefreshing { get; set; }
        public ObservableCollection<AchievementViewModel> Achievements { get; set; }

        public ICommand RefreshListCommand { get; set; }
        public ICommand SelectAchievementCommand { get; set; }

        public void OnAppearing()
        {
            LoadAchievementsFromDatabase();
        }

        private void RefreshCommandExecute()
        {
            IsRefreshing = true;
            LoadAchievementsFromDatabase();
            IsRefreshing = false;
        }

        private async Task<AchievementViewModel> SelectAchievementCommandExecute(int id)
        {
            return (await App.AchievementRepository.GetByIdAsync(id)).ToAchievementViewModel();
        }

        private void LoadAchievementsFromDatabase()
        {
            int.TryParse(Settings.CurrentUserId, out int userId);

            Achievements = App.AchievementRepository
                .GetAll(userId)
                .ToAchievementViewModels()
                .OrderByDescending(x => x.GeneralTimeSpent)
                .ToObservableCollection();
        }
    }
}