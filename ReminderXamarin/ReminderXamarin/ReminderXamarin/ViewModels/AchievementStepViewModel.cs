using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Acr.UserDialogs;

using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class AchievementStepViewModel : BaseViewModel
    {
        private readonly IFileSystem _fileService;
        private readonly IMediaService _mediaService;

        private int _achievementStepId;
        private AchievementStep _achievementStep;        

        public AchievementStepViewModel(
            INavigationService navigationService,
            IFileSystem fileService,
            IMediaService mediaService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _fileService = fileService;
            _mediaService = mediaService;

            ImageContent = new byte[0];
            SaveAchievementStepCommand = commandResolver.AsyncCommand(Save);
            ChangeImageCommand = commandResolver.AsyncCommand<PlatformDocument>(ChangeImage);
            ChangeProgressCommand = commandResolver.AsyncCommand<string>(ChangeProgress);
        }

        public IAsyncCommand SaveAchievementStepCommand { get; }
        public IAsyncCommand<PlatformDocument> ChangeImageCommand { get; }
        public IAsyncCommand<string> ChangeProgressCommand { get; }

        public IList<string> AvailableStepTypes =>
            Enum.GetNames(typeof(AchievementStepType)).Select(x => x.ToString()).ToList();

        public AchievementStepType StepType { get; set; } = AchievementStepType.Single;

        public int Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TimeSpent { get; set; }
        public int TimeEstimation { get; set; }
        public bool IsEditMode { get; set; }
        public double Progress => TimeSpent / (double)TimeEstimation;
        public bool ViewModelChanged { get; set; }

        public bool IsAchieved => Progress >= 1.0;

        public int AchievementId { get; set; }

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

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData == null)
            {
                return base.InitializeAsync(null);
            }

            _achievementStepId = (int) navigationData;

            if (_achievementStepId == 0)
            {
                Title = Resmgr.Value.GetString(ConstantsHelper.NewStep, CultureInfo.CurrentCulture);
            }
            else
            {
                IsEditMode = true;
                _achievementStep = App.AchievementStepRepository.Value.GetAchievementStepAsync(_achievementStepId);
                Title = _achievementStep.Title;
                Description = _achievementStep.Description;
                ImageContent = _achievementStep.ImageContent;
                TimeSpent = _achievementStep.TimeSpent;
                TimeEstimation = _achievementStep.TimeEstimation;
            }
            return base.InitializeAsync(navigationData);
        }
        
        private async Task Save()
        {
            if (string.IsNullOrEmpty(Title))
            {
                await NavigationService.NavigateBackAsync();
                return;
            }
            if (StepType == AchievementStepType.Single)
            {
                TimeEstimation = 1;
            }
            MessagingCenter.Send(this, ConstantsHelper.AchievementStepEditComplete);
            await NavigationService.NavigateBackAsync().ConfigureAwait(false);
        }

        private async Task ChangeProgress(string amount)
        {
            if (int.TryParse(amount, out int result))
            {
                TimeSpent += result;
                if (TimeSpent < 0)
                {
                    TimeSpent = 0;
                }
            }
        }
    }

    public enum AchievementStepType
    {
        Single,
        TimeSpending
    }
}