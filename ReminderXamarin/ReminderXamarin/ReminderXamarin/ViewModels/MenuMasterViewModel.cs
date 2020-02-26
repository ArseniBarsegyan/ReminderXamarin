using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class MenuMasterViewModel : BaseViewModel
    {
        private UserModel _appUser;

        public MenuMasterViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            MessagingCenter.Subscribe<UserProfileViewModel>(this, ConstantsHelper.ProfileUpdated, vm => ProfileUpdated());
            _appUser = App.UserRepository.Value.GetAll().FirstOrDefault(x => x.UserName == Settings.ApplicationUser);
            if (_appUser != null)
            {
                UserName = _appUser.UserName;
                ImageContent = _appUser.ImageContent;
                Settings.CurrentUserId = _appUser.Id;
            }
            MasterPageItems = MenuHelper.GetMenu().Where(x => x.IsDisplayed).ToObservableCollection();
            LogoutCommand = new Command(async task => await Logout());
            NavigateToUserProfileCommand = new Command(async task => await NavigateToUserProfile());
        }

        private void ProfileUpdated()
        {
            _appUser = App.UserRepository.Value.GetAll().FirstOrDefault(x => x.UserName == Settings.ApplicationUser);
            if (_appUser != null)
            {
                UserName = _appUser.UserName;
                ImageContent = _appUser.ImageContent;
            }
        }

        public string UserName { get; set; }
        public byte[] ImageContent { get; set; }
        public ObservableCollection<MasterPageItem> MasterPageItems { get; set; }
        public ICommand LogoutCommand { get; }
        public ICommand NavigateToUserProfileCommand { get; }

        private async Task Logout()
        {
            bool.TryParse(Settings.UsePin, out var result);
            if (result)
            {
                await NavigationService.InitializeAsync<PinViewModel>();
            }
            else
            {
                await NavigationService.InitializeAsync<LoginViewModel>();
            }
        }

        private async Task NavigateToUserProfile()
        {
            await NavigationService.NavigateToAsync<UserProfileViewModel>(_appUser);
        }
    }
}