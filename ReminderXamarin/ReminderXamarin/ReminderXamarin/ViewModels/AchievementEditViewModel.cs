using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Acr.UserDialogs;

using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

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
            ChangeEditEnabledCommand = commandResolver.Command(() => IsEditMode = !IsEditMode);

            //MessagingCenter.Subscribe<AchievementStepViewModel>(this, ConstantsHelper.AchievementStepEditComplete, AddViewModel);
        }

        public void OnDisappearing()
        {
            //MessagingCenter.Unsubscribe<AchievementStepViewModel>(this, ConstantsHelper.AchievementStepEditComplete);
        }        

        public string Title { get; set; }
        public string Description { get; set; }
        public double AchievementProgress 
        {
            get => (double)GeneralTimeSpent / 10000;
        }
        public int GeneralTimeSpent { get; set; }
        public bool IsEditMode { get; set; }

        public ObservableCollection<AchievementStepViewModel> AchievementStepViewModels { get; private set; }

        public IAsyncCommand SaveAchievementCommand { get; }
        public IAsyncCommand DeleteAchievementCommand { get; }
        public IAsyncCommand AddStepCommand { get; }
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
                AchievementStepViewModels = model.AchievementSteps
                    .ToViewModels(NavigationService,
                    _fileService,
                    _mediaService,
                    _commandResolver);
            }
            return base.InitializeAsync(navigationData);
        }

        //private void AddViewModel(AchievementStepViewModel viewModel)
        //{
        //    AchievementStepViewModels.Add(viewModel);
        //}

        private async Task SaveAchievement()
        {
            var model = App.AchievementRepository.Value.GetAchievementAsync(_achievementId);
            model.Description = Description;
            model.Title = Title;
            model.GeneralTimeSpent = GeneralTimeSpent;
            model.AchievementSteps = AchievementStepViewModels.ToModels();            
            App.AchievementRepository.Value.Save(model);
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

                    App.AchievementRepository.Value.DeleteAchievement(achievementToDelete);
                    await NavigationService.NavigateBackAsync();
                }
            }
        }

        private async Task AddStep()
        {
            // Show popup with step name and note
            // and spent time, 2 buttons - ok and cancel
            // When step is added check if achievement progress changed
            // If achievement is completed then show immediate popup
            // with animation.
        }
    }
}