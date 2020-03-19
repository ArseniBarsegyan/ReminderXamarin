using System.Threading.Tasks;
using System.Windows.Input;

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
        public NewAchievementViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            CreateAchievementCommand = commandResolver.AsyncCommand(CreateAchievement);
            NavigateBackCommand = commandResolver.AsyncCommand(NavigateBack);
        }

        public string Title { get; set; }
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
