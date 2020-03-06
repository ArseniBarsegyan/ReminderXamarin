using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Helpers;

namespace ReminderXamarin.ViewModels
{
    public class AchievementsViewModel : BaseViewModel
    {
        public AchievementsViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            Achievements = new ObservableCollection<AchievementViewModel>();

            RefreshListCommand = commandResolver.Command(Refresh);
            NavigateToAchievementEditViewCommand = commandResolver.AsyncCommand<int>(NavigateToAchievementEditView);
        }

        public bool IsRefreshing { get; set; }
        public ObservableCollection<AchievementViewModel> Achievements { get; set; }

        public ICommand RefreshListCommand { get; set; }
        public IAsyncCommand<int> NavigateToAchievementEditViewCommand { get; set; }

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
            Achievements = App.AchievementRepository.Value
                .GetAll(x => x.UserId == Settings.CurrentUserId)
                .ToAchievementViewModels(NavigationService)
                .OrderByDescending(x => x.GeneralTimeSpent)
                .ToObservableCollection();
        }
    }
}