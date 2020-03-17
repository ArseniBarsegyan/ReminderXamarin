using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;

using ReminderXamarin.Core.Interfaces;
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
    public class AchievementEditViewModel : BaseViewModel
    {
        private readonly IFileSystem _fileService;
        private readonly IMediaService _mediaService;
        private readonly ICommandResolver _commandResolver;

        private int _achievementId;
        private AchievementModel _achievement;

        public AchievementEditViewModel(INavigationService navigationService,
            IFileSystem fileService,
            IMediaService mediaService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _fileService = fileService;
            _mediaService = mediaService;
            _commandResolver = commandResolver;

            AchievementStepViewModels = new ObservableCollection<AchievementStepViewModel>();

            SaveAchievementCommand = commandResolver.AsyncCommand(SaveAchievement);
            DeleteAchievementCommand = commandResolver.AsyncCommand(DeleteAchievement);
            AddStepCommand = commandResolver.AsyncCommand(AddStep);
            NavigateToAchievementStepEditViewCommand = commandResolver
                .AsyncCommand<KeyValuePair<int, int>>(NavigateToAchievementStepEditView);
            ChangeEditEnabledCommand = commandResolver.Command(() => IsEditEnabled = !IsEditEnabled);

            MessagingCenter.Subscribe<AchievementStepViewModel>(this, ConstantsHelper.AchievementStepEditComplete, AddViewModel);
        }

        public void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<AchievementStepViewModel>(this, ConstantsHelper.AchievementStepEditComplete);
        }

        private void AddViewModel(AchievementStepViewModel viewModel)
        {
            AchievementStepViewModels.Add(viewModel);
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GeneralTimeSpent { get; set; }
        public bool ViewModelChanged { get; set; }
        public bool IsEditMode { get; set; }
        public bool IsEditEnabled { get; set; }

        public ObservableCollection<AchievementStepViewModel> AchievementStepViewModels { get; private set; }

        public IAsyncCommand SaveAchievementCommand { get; }
        public IAsyncCommand DeleteAchievementCommand { get; }
        public IAsyncCommand AddStepCommand { get; }
        public ICommand ChangeEditEnabledCommand { get; }
        public IAsyncCommand<KeyValuePair<int, int>> NavigateToAchievementStepEditViewCommand { get; }
        
        public override Task InitializeAsync(object navigationData)
        {
            _achievementId = (int)navigationData;

            if (_achievementId == 0)
            {
                Title = Resmgr.Value.GetString(ConstantsHelper.CreateAchievement, CultureInfo.CurrentCulture);
            }
            else
            {
                IsEditMode = true;
                _achievement = App.AchievementRepository.Value.GetAchievementAsync(_achievementId);
                Id = _achievement.Id;
                Title = _achievement.Title;
                Description = _achievement.Description;
                AchievementStepViewModels = _achievement.AchievementSteps
                    .ToViewModels(NavigationService,
                    _fileService,
                    _mediaService,
                    _commandResolver);
            }
            return base.InitializeAsync(navigationData);
        }

        private async Task NavigateToAchievementStepEditView(KeyValuePair<int, int> pair)
        {
            await NavigationService.NavigateToAsync<AchievementStepViewModel>(pair);
        }

        private async Task SaveAchievement()
        {
            if (_achievementId == 0)
            {
                var achievement = new AchievementModel
                {
                    AchievementSteps = AchievementStepViewModels.ToModels(),
                    Description = Description,
                    GeneralTimeSpent = 0,
                    Title = Title,
                    UserId = Settings.CurrentUserId
                };
                App.AchievementRepository.Value.Save(achievement);
                await NavigationService.NavigateBackAsync();
            }
            else
            {
                if (AchievementStepViewModels.Any())
                {
                    GeneralTimeSpent = AchievementStepViewModels.Sum(x => x.TimeSpent);
                }

                var achievement = App.AchievementRepository.Value.GetAchievementAsync(_achievementId);
                achievement.Description = Description;
                achievement.Title = Title;
                achievement.GeneralTimeSpent = GeneralTimeSpent;
                achievement.AchievementSteps = AchievementStepViewModels.ToModels();
                App.AchievementRepository.Value.Save(achievement);
            }
        }

        private async Task DeleteAchievement()
        {
            if (_achievementId != 0)
            {
                bool result = await UserDialogs.Instance.ConfirmAsync(ConstantsHelper.AchievementDeleteMessage,
                    ConstantsHelper.Warning, ConstantsHelper.Ok, ConstantsHelper.Cancel);
                if (result)
                {
                    var achievementToDelete = App.AchievementRepository.Value.GetAchievementAsync(_achievementId);
                    App.AchievementRepository.Value.DeleteAchievement(achievementToDelete);
                    await NavigationService.NavigateBackAsync();
                }
            }
        }

        private async Task AddStep()
        {
            await NavigationService.NavigateToAsync<AchievementStepViewModel>();
        }
    }
}