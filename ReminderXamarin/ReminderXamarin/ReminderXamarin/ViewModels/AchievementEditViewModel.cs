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

using Rm.Data.Data.Repositories;
using Rm.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class AchievementEditViewModel : BaseNavigableViewModel
    {
        private readonly List<AchievementStepViewModel> _stepsToDelete;
        private int _achievementId;

        private AchievementStepRepository AchievementStepRepository => App.AchievementStepRepository.Value;
        private AchievementRepository AchievementRepository => App.AchievementRepository.Value;

        public AchievementEditViewModel(
            INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _stepsToDelete = new List<AchievementStepViewModel>();
            AchievementSteps = new ObservableCollection<AchievementStepViewModel>();

            SaveAchievementCommand = commandResolver.AsyncCommand(SaveAchievement);
            NavigateToAchievementStepEditViewCommand = commandResolver.AsyncCommand<AchievementStepViewModel>(NavigateToAchievementStepEditView);
            DeleteAchievementCommand = commandResolver.AsyncCommand(DeleteAchievement);
            AddStepCommand = commandResolver.AsyncCommand(AddStep);
            DeleteStepCommand = commandResolver.AsyncCommand<AchievementStepViewModel>(DeleteStep);
            ChangeEditEnabledCommand = commandResolver.Command(() => IsEditMode = !IsEditMode);

            MessagingCenter.Subscribe<AchievementStepEditViewModel, AchievementStepViewModel>(this, 
                ConstantsHelper.AchievementStepEditComplete,
                (vm, args) => UpdateStepsList(args));
        }

        public void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<AchievementStepEditViewModel>(this, 
                ConstantsHelper.AchievementStepEditComplete);
        }        

        public string Title { get; set; }
        public string Description { get; set; }
        public double AchievementProgress => GeneralTimeSpent / 10000;
        public double GeneralTimeSpent { get; set; }
        public bool IsEditMode { get; set; }

        public ObservableCollection<AchievementStepViewModel> AchievementSteps { get; private set; }

        public IAsyncCommand SaveAchievementCommand { get; }
        public IAsyncCommand<AchievementStepViewModel> NavigateToAchievementStepEditViewCommand { get; }
        public IAsyncCommand DeleteAchievementCommand { get; }
        public IAsyncCommand AddStepCommand { get; }
        public IAsyncCommand<AchievementStepViewModel> DeleteStepCommand { get; }
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
                var model = AchievementRepository.GetAchievementAsync(_achievementId);
                GeneralTimeSpent = model.GeneralTimeSpent;
                Title = model.Title;
                Description = model.Description;
                AchievementSteps = model.AchievementSteps.ToAchievementStepViewModels();
            }
            return base.InitializeAsync(navigationData);
        }

        private void UpdateStepsList(AchievementStepViewModel viewModel)
        {
            if (!AchievementSteps.Contains(viewModel))
            {
                AchievementSteps.Add(viewModel);
            }

            GeneralTimeSpent = AchievementSteps.Sum(x => x.TimeSpent);
            IsEditMode = true;
        }

        private async Task SaveAchievement()
        {
            var model = AchievementRepository.GetAchievementAsync(_achievementId);
            model.Description = Description;
            model.Title = Title;
            model.GeneralTimeSpent = GeneralTimeSpent;
            model.AchievementSteps = AchievementSteps.ToAchievementStepViewModels();
            AchievementRepository.Save(model);
            if (_stepsToDelete.Any())
            {
                foreach(var stepViewModel in _stepsToDelete)
                {
                    var stepToDelete = AchievementStepRepository.GetAchievementStepAsync(stepViewModel.Id);
                    AchievementStepRepository.DeleteAchievementStep(stepToDelete);
                }
                _stepsToDelete.Clear();
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
                        var stepToDelete = AchievementStepRepository.GetAchievementStepAsync(step.Id);
                        AchievementStepRepository.DeleteAchievementStep(stepToDelete);
                    }

                    AchievementRepository.DeleteAchievement(achievementToDelete);
                    await NavigationService.NavigateBackAsync();
                }
            }
        }

        private Task AddStep()
        {
            return NavigationService.NavigateToPopupAsync<AchievementStepEditViewModel>(
                new AchievementStepViewModel
                {
                    AchievementId = _achievementId
                });
        }

        private async Task DeleteStep(AchievementStepViewModel viewModel)
        {
            bool result = await UserDialogs.Instance.ConfirmAsync(
                    ConstantsHelper.AchievementStepDeleteMessage,
                    ConstantsHelper.Warning,
                    ConstantsHelper.Ok,
                    ConstantsHelper.Cancel);

            if (result)
            {
                AchievementSteps.Remove(viewModel);
                GeneralTimeSpent = AchievementSteps.Sum(x => x.TimeSpent);
                if (!_stepsToDelete.Contains(viewModel))
                {
                    _stepsToDelete.Add(viewModel);
                }
                IsEditMode = true;
            }
        }

        private Task NavigateToAchievementStepEditView(AchievementStepViewModel model)
        {
            return NavigationService.NavigateToPopupAsync<AchievementStepEditViewModel>(model);
        }
    }
}