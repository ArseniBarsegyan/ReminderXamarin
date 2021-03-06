﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;
using Rm.Data.Data.Repositories;
using Rm.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class AchievementsViewModel : BaseNavigableViewModel
    {
        public AchievementsViewModel(
            INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            Achievements = new ObservableCollection<AchievementViewModel>();

            RefreshListCommand = commandResolver.Command(Refresh);
            NavigateToAchievementEditViewCommand = commandResolver.AsyncCommand<int>(NavigateToAchievementEditView);
            CreateNewAchievementCommand = commandResolver.Command(CreateNewAchievement);
            DeleteAchievementCommand = commandResolver.AsyncCommand<AchievementViewModel>(DeleteAchievement);
        }

        public bool IsRefreshing { get; set; }
        public ObservableCollection<AchievementViewModel> Achievements { get; private set; }

        public ICommand RefreshListCommand { get; }
        public IAsyncCommand<int> NavigateToAchievementEditViewCommand { get; }
        public IAsyncCommand<AchievementViewModel> DeleteAchievementCommand { get; }
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
                .ToAchievementViewModels()
                .OrderByDescending(x => x.GeneralTimeSpent)
                .ToObservableCollection();
        }
        
        private async Task DeleteAchievement(AchievementViewModel viewModel)
        {
            bool result = await UserDialogs.Instance.ConfirmAsync(
                ConstantsHelper.AchievementDeleteMessage,
                ConstantsHelper.Warning,
                ConstantsHelper.Ok,
                ConstantsHelper.Cancel);

            if (result)
            {
                Achievements.Remove(viewModel);
                var achievementToDelete = App.AchievementRepository.Value
                    .GetAchievementAsync(viewModel.Id);

                var steps = achievementToDelete.AchievementSteps;
                foreach (var step in steps)
                {
                    App.AchievementStepRepository.Value.DeleteAchievementStep(step);
                }

                App.AchievementRepository.Value.DeleteAchievement(achievementToDelete);
            }
        }
    }
}