using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.Utilities;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

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
                vm => UpdateProfile());
            MessagingCenter.Subscribe<ThemeSwitcher>(this,
                ConstantsHelper.AppThemeChanged,
                ts => DrawMenu());

            UpdateProfile();
            DrawMenu();            
            
            NavigateToUserProfileCommand = commandResolver.AsyncCommand(NavigateToUserProfile);
            ChangeDetailsPageCommand = commandResolver.AsyncCommand<MenuViewIndex>(ChangeDetailsPageAsync);
            LogoutCommand = commandResolver.AsyncCommand(Logout);
        }

        private void UpdateProfile()
        {
            _appUser = App.UserRepository.Value
                .GetAll(x => x.UserName == Settings.ApplicationUser)
                .FirstOrDefault();
            if (_appUser != null)
            {
                UserName = _appUser.UserName;
                Settings.CurrentUserId = _appUser.Id;
            }
            UpdateProfilePhoto();
        }

        public string UserName { get; private set; }
        public ImageSource UserProfilePhoto { get; private set; }
        public ImageSource HeaderBackgroundImageSource { get; private set; }
        public ImageSource LogoutImageSource { get; private set; }
        public ObservableCollection<MasterPageItem> MasterPageItems { get; private set; }

        public IAsyncCommand NavigateToUserProfileCommand { get; }
        public IAsyncCommand<MenuViewIndex> ChangeDetailsPageCommand { get; }
        public IAsyncCommand LogoutCommand { get; }

        private async Task NavigateToUserProfile()
        {
            await NavigationService.NavigateToDetails<UserProfileViewModel>(_appUser);
        }

        private async Task ChangeDetailsPageAsync(MenuViewIndex pageIndex)
        {
            switch (pageIndex)
            {
                case MenuViewIndex.NotesView:
                    await NavigationService.NavigateToDetails<NotesViewModel>();
                    break;
                case MenuViewIndex.ToDoPage:
                    await NavigationService.NavigateToDetails<ToDoCalendarViewModel>();
                    break;
                case MenuViewIndex.BirthdaysView:
                    await NavigationService.NavigateToDetails<BirthdaysViewModel>();
                    break;
                case MenuViewIndex.AchievementsView:
                    await NavigationService.NavigateToDetails<AchievementsViewModel>();
                    break;
                case MenuViewIndex.SettingsView:
                    await NavigationService.NavigateToDetails<SettingsViewModel>();
                    break;
                default:
                    break;
            }
        }

        private async Task Logout()
        {
            bool.TryParse(Settings.UsePin, out var result);
            if (result)
            {
                await NavigationService.ToRootAsync<PinViewModel>();
            }
            else
            {
                await NavigationService.ToRootAsync<LoginViewModel>();
            }
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