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

using Xamarin.Forms;

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
            CreateNewAchievementCommand = commandResolver.Command(CreateNewAchievement);
        }

        public bool IsRefreshing { get; set; }
        public ObservableCollection<AchievementViewModel> Achievements { get; private set; }

        public ICommand RefreshListCommand { get; }
        public ICommand SelectAchievementCommand { get; }
        public IAsyncCommand<int> NavigateToAchievementEditViewCommand { get; }
        public ICommand CreateNewAchievementCommand { get; }

        public void OnAppearing()
        {
            MessagingCenter.Subscribe<NewAchievementViewModel>(this,
                    ConstantsHelper.AchievementCreated, (vm) => Refresh());
            Refresh();
        }

        public void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<NewAchievementViewModel>(this,
                    ConstantsHelper.AchievementCreated);
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

        private void CreateNewAchievement()
        {            
            NavigationService.NavigateToPopupAsync<NewAchievementViewModel>();
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