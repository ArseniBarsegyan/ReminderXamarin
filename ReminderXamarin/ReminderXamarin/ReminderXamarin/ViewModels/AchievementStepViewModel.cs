using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels.Base;
using Rm.Data.Data.Entities;
using Rm.Helpers;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class AchievementStepViewModel : BaseViewModel
    {
        private int _achievementStepId;
        private AchievementStep _achievementStep;

        private static readonly IFileSystem FileService = DependencyService.Get<IFileSystem>();
        private static readonly IMediaService MediaService = DependencyService.Get<IMediaService>();

        public AchievementStepViewModel()
        {
            SaveAchievementStepCommand = new Command(async() => await Save());
            ChangeImageCommand = new Command<PlatformDocument>(async document => await ChangeImage(document));
            IncreaseProgressCommand = new Command<string>(async amount => await IncreaseProgress(amount));
        }

        public ICommand SaveAchievementStepCommand { get; set; }
        public ICommand ChangeImageCommand { get; set; }
        public ICommand IncreaseProgressCommand { get; set; }

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
                    var imageContent = FileService.ReadAllBytes(document.Path);
                    var resizedImage = MediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth,
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
            if (StepType == AchievementStepType.Single)
            {
                TimeEstimation = 1;
            }
            MessagingCenter.Send(this, ConstantsHelper.AchievementStepEditComplete);
            await NavigationService.NavigateBackAsync();
        }

        private async Task IncreaseProgress(string amount)
        {
            int.TryParse(amount, out int result);
            TimeSpent += result;
        }
    }

    public enum AchievementStepType
    {
        Single,
        TimeSpending
    }
}