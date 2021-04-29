using System.Threading.Tasks;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class NewAchievementViewModel : BaseViewModel
    {
        private bool _isEnabled;

        public NewAchievementViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            CreateAchievementCommand = commandResolver.AsyncCommand(CreateAchievement, () => { return IsEnabled; });
            NavigateBackCommand = commandResolver.AsyncCommand(NavigateBack);
        }

        public string Title { get; set; }
        public bool IsEnabled 
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Title)
                    || Title.Length < 3)
                {
                    return false;
                }
                return true;
            }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Description { get; set; }

        public IAsyncCommand CreateAchievementCommand { get; }
        public IAsyncCommand NavigateBackCommand { get; }

        private async Task CreateAchievement()
        {
            var model = new AchievementModel
            {
                Title = Title,
                Description = Description,
                UserId = Settings.CurrentUserId
            };
            App.AchievementRepository.Value.Save(model);
            MessagingCenter.Send(this, ConstantsHelper.AchievementCreated);
            await NavigateBack();
        }

        private async Task NavigateBack()
        {
            await NavigationService.NavigatePopupBackAsync();
        }
    }
}
