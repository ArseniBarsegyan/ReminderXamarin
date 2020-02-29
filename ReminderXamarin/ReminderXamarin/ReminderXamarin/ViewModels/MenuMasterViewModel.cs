using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.Utilities;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class MenuMasterViewModel : BaseViewModel
    {
        private readonly ThemeSwitcher _themeSwitcher;
        private UserModel _appUser;

        public MenuMasterViewModel(INavigationService navigationService,
            ThemeSwitcher themeSwitcher,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _themeSwitcher = themeSwitcher;

            MessagingCenter.Subscribe<UserProfileViewModel>(this,
                ConstantsHelper.ProfileUpdated,
                vm => ProfileUpdated());
            MessagingCenter.Subscribe<ThemeSwitcher>(this,
                ConstantsHelper.AppThemeChanged,
                ts => DrawMenu());

            _appUser = App.UserRepository.Value.GetAll()
                .FirstOrDefault(x => x.UserName == Settings.ApplicationUser);
            if (_appUser != null)
            {
                UserName = _appUser.UserName;
                Settings.CurrentUserId = _appUser.Id;
            }
            DrawMenu();
            LogoutCommand = commandResolver.AsyncCommand(Logout);
            NavigateToUserProfileCommand = commandResolver.AsyncCommand(NavigateToUserProfile);
        }

        private void ProfileUpdated()
        {
            _appUser = App.UserRepository.Value.GetAll()
                .FirstOrDefault(x => x.UserName == Settings.ApplicationUser);
            if (_appUser != null)
            {
                UserName = _appUser.UserName;
            }
            UpdateProfilePhoto();
        }

        public string UserName { get; set; }
        public ImageSource UserProfilePhoto { get; set; }
        public ImageSource HeaderBackgroundImageSource { get; private set; }
        public ImageSource LogoutImageSource { get; private set; }
        public ObservableCollection<MasterPageItem> MasterPageItems { get; set; }
        public IAsyncCommand LogoutCommand { get; }
        public IAsyncCommand NavigateToUserProfileCommand { get; }

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

        private void DrawMenu()
        {
            MasterPageItems = MenuHelper.GetMenu(_themeSwitcher.CurrentThemeType)
                .Where(x => x.IsDisplayed)
                .ToObservableCollection();

            UpdateProfilePhoto();

            HeaderBackgroundImageSource = _themeSwitcher.CurrentThemeType == ThemeTypes.Dark
                ? ImageSource.FromResource(ConstantsHelper.SideMenuDarkBackground)
                : ImageSource.FromResource(ConstantsHelper.SideMenuLightBackground);

            LogoutImageSource = _themeSwitcher.CurrentThemeType == ThemeTypes.Dark
                        ? ConstantsHelper.LogoutLightImage
                        : ConstantsHelper.LogoutDarkImage;
        }

        private void UpdateProfilePhoto()
        {
            if (_appUser.ImageContent == null || _appUser.ImageContent.Length == 0)
            {
                UserProfilePhoto = ImageSource.FromResource(
                    ConstantsHelper.NoPhotoImage);
            }
            else
            {
                UserProfilePhoto = ImageSource.FromStream(
                    () => new MemoryStream(_appUser.ImageContent));
            }
        }
    }
}