using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels.Base;
using ReminderXamarin.Data.Entities;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class MenuMasterViewModel : BaseViewModel
    {
        private AppUser _appUser;

        public MenuMasterViewModel()
        {
            _appUser = App.UserRepository.GetAll().FirstOrDefault(x => x.UserName == Settings.ApplicationUser);
            if (_appUser != null)
            {
                UserName = _appUser.UserName;
                ImageContent = _appUser.ImageContent;
                Settings.CurrentUserId = _appUser.Id.ToString();
            }
            MasterPageItems = MenuHelper.GetMenu().Where(x => x.IsDisplayed).ToObservableCollection();
            LogoutCommand = new Command(async task => await Logout());
            NavigateToUserProfileCommand = new Command(async task => await NavigateToUserProfile());
        }

        public string UserName { get; set; }
        public byte[] ImageContent { get; set; }
        public ObservableCollection<MasterPageItem> MasterPageItems { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand NavigateToUserProfileCommand { get; set; }

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