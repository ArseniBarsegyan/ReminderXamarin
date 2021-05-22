using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Acr.UserDialogs;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class AchievementEditViewModel : BaseNavigableViewModel
    {
        private int _achievementId;
        private List<AchievementStep> _stepsToDelete;

        public AchievementEditViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _stepsToDelete = new List<AchievementStep>();
            AchievementSteps = new ObservableCollection<AchievementStep>();

            SaveAchievementCommand = commandResolver.AsyncCommand(SaveAchievement);
            NavigateToAchievementStepEditViewCommand = commandResolver.AsyncCommand<AchievementStep>(NavigateToAchievementStepEditView);
            DeleteAchievementCommand = commandResolver.AsyncCommand(DeleteAchievement);
            AddStepCommand = commandResolver.AsyncCommand(AddStep);
            DeleteStepCommand = commandResolver.AsyncCommand<AchievementStep>(DeleteStep);
            ChangeEditEnabledCommand = commandResolver.Command(() => IsEditMode = !IsEditMode);

            MessagingCenter.Subscribe<AchievementStepViewModel, AchievementStep>(this, 
                ConstantsHelper.AchievementStepEditComplete,
                (vm, args) => UpdateStepsList(args));
        }

        public void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<AchievementStepViewModel>(this, ConstantsHelper.AchievementStepEditComplete);
        }        

        public string Title { get; set; }
        public string Description { get; set; }
        public double AchievementProgress 
        {
            get => GeneralTimeSpent / 10000;
        }
        public double GeneralTimeSpent { get; set; }
        public bool IsEditMode { get; set; }

        public ObservableCollection<AchievementStep> AchievementSteps { get; private set; }

        public IAsyncCommand SaveAchievementCommand { get; }
        public IAsyncCommand<AchievementStep> NavigateToAchievementStepEditViewCommand { get; }
        public IAsyncCommand DeleteAchievementCommand { get; }
        public IAsyncCommand AddStepCommand { get; }
        public IAsyncCommand<AchievementStep> DeleteStepCommand { get; }
        public ICommand ChangeEditEnabledCommand { get; }
        
        public override Task InitializeAsync(object navigationData)
        {
            _achievementId = (int)navigationData;

            if (_achievementId == 0)
            {
                Title = Resmgr.Value.GetString(ConstantsHelper.CreateAchievement, CultureInfo.CurrentCulture);
            }
            else
            {
                var model = App.AchievementRepository.Value.GetAchievementAsync(_achievementId);
                GeneralTimeSpent = model.GeneralTimeSpent;
                Title = model.Title;
                Description = model.Description;
                AchievementSteps = model.AchievementSteps.ToObservableCollection();
            }
            return base.InitializeAsync(navigationData);
        }

        private void UpdateStepsList(AchievementStep model)
        {
            AchievementSteps.Add(model);
            GeneralTimeSpent = AchievementSteps.Sum(x => x.TimeSpent);
            IsEditMode = true;
        }

        private async Task SaveAchievement()
        {
            var model = App.AchievementRepository.Value.GetAchievementAsync(_achievementId);
            model.Description = Description;
            model.Title = Title;
            model.GeneralTimeSpent = GeneralTimeSpent;
            model.AchievementSteps = AchievementSteps.ToList();
            App.AchievementRepository.Value.Save(model);
            if (_stepsToDelete.Any())
            {
                foreach(var step in _stepsToDelete)
                {
                    App.AchievementStepRepository.Value.DeleteAchievementStep(step);
                }
            }
            if (GeneralTimeSpent >= 10000)
            {
                // TODO: make custom alert (at top of screen with animation)
                await UserDialogs.Instance.AlertAsync("Achieved");
                model.AchievedDate = DateTime.Now;
            }
            await NavigationService.NavigateBackAsync();
        }

        private async Task DeleteAchievement()
        {
            if (_achievementId != 0)
            {
                bool result = await UserDialogs.Instance.ConfirmAsync(
                    ConstantsHelper.AchievementDeleteMessage,
                    ConstantsHelper.Warning,
                    ConstantsHelper.Ok,
                    ConstantsHelper.Cancel);

                if (result)
                {
                    var achievementToDelete = App.AchievementRepository.Value
                        .GetAchievementAsync(_achievementId);

                    foreach (var step in AchievementSteps)
                    {
                        App.AchievementStepRepository.Value.DeleteAchievementStep(step);
                    }

                    App.AchievementRepository.Value.DeleteAchievement(achievementToDelete);
                    await NavigationService.NavigateBackAsync();
                }
            }
        }

        private Task AddStep()
        {
            return NavigationService.NavigateToPopupAsync<AchievementStepViewModel>(
                new AchievementStep
                {
                    AchievementId = _achievementId
                });
        }

        private async Task DeleteStep(AchievementStep model)
        {
            bool result = await UserDialogs.Instance.ConfirmAsync(
                    ConstantsHelper.AchievementStepDeleteMessage,
                    ConstantsHelper.Warning,
                    ConstantsHelper.Ok,
                    ConstantsHelper.Cancel);

            if (result)
            {
                AchievementSteps.Remove(model);
                GeneralTimeSpent = AchievementSteps.Sum(x => x.TimeSpent);
                if (!_stepsToDelete.Contains(model))
                {
                    _stepsToDelete.Add(model);
                }
                IsEditMode = true;
            }
        }

        private Task NavigateToAchievementStepEditView(AchievementStep model)
        {
            return NavigationService.NavigateToPopupAsync<AchievementStepViewModel>(model);
        }
    }
}