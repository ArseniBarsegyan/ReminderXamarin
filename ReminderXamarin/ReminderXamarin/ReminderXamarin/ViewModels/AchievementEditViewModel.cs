using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Acr.UserDialogs;

using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.FilePickerService;
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

            ImageContent = new byte[0];
            AchievementStepViewModels = new ObservableCollection<AchievementStepViewModel>();

            ChangeImageCommand = commandResolver.AsyncCommand<PlatformDocument>(ChangeImage);
            SaveAchievementCommand = commandResolver.AsyncCommand(SaveAchievement);
            DeleteAchievementCommand = commandResolver.AsyncCommand(DeleteAchievement);
            AddStepCommand = commandResolver.AsyncCommand(AddStep);
            NavigateToAchievementStepEditViewCommand = commandResolver.AsyncCommand<KeyValuePair<int, int>>(NavigateToAchievementStepEditView);

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
        public byte[] ImageContent { get; set; }
        public bool ViewModelChanged { get; set; }
        public bool IsEditMode { get; set; }

        public ObservableCollection<AchievementStepViewModel> AchievementStepViewModels { get; set; }

        public IAsyncCommand<PlatformDocument> ChangeImageCommand { get; }
        public IAsyncCommand SaveAchievementCommand { get; }
        public IAsyncCommand DeleteAchievementCommand { get; }
        public IAsyncCommand AddStepCommand { get; }
        public IAsyncCommand<KeyValuePair<int, int>> NavigateToAchievementStepEditViewCommand { get; }

        private async Task ChangeImage(PlatformDocument document)
        {
            if (document.Name.EndsWith(".png") || document.Name.EndsWith(".jpg") || document.Name.EndsWith(".jpeg"))
            {
                try
                {
                    ViewModelChanged = true;
                    var imageContent = _fileService.ReadAllBytes(document.Path);
                    var resizedImage = _mediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth,
                        ConstantsHelper.ResizedImageHeight);

                    ImageContent = resizedImage;
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(ex.Message);
                }
            }
        }

        private async Task NavigateToAchievementStepEditView(KeyValuePair<int, int> pair)
        {
            await NavigationService.NavigateToAsync<AchievementStepViewModel>(pair);
        }

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
                Description = _achievement.GeneralDescription;
                ImageContent = _achievement.ImageContent;
                AchievementStepViewModels = _achievement.AchievementSteps
                    .ToViewModels(NavigationService,
                    _fileService, 
                    _mediaService, 
                    _commandResolver);
            }
            return base.InitializeAsync(navigationData);
        }

        private async Task SaveAchievement()
        {
            if (_achievementId == 0)
            {
                var achievement = new AchievementModel
                {
                    AchievementSteps = AchievementStepViewModels.ToModels(),
                    GeneralDescription = Description,
                    GeneralTimeSpent = 0,
                    ImageContent = ImageContent,
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
                achievement.GeneralDescription = Description;
                achievement.ImageContent = ImageContent;
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