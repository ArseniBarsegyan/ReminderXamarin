using System.Threading.Tasks;

using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public BaseViewModel MasterViewModel { get; set; }
        public BaseViewModel DetailViewModel { get; set; }

        public MenuViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            MessagingCenter.Subscribe<MenuMasterViewModel, MenuViewIndex>(this, ConstantsHelper.DetailPageChanged, async (sender, pageIndex) =>
            {
                switch (pageIndex)
                {
                    case MenuViewIndex.NotesView:
                        await NavigationService.NavigateToAsync<NotesViewModel>();
                        break;
                    case MenuViewIndex.ToDoPage:
                        await NavigationService.NavigateToAsync<ToDoTabbedViewModel>();
                        break;
                    case MenuViewIndex.BirthdaysView:
                        await NavigationService.NavigateToAsync<BirthdaysViewModel>();
                        break;
                    case MenuViewIndex.AchievementsView:
                        await NavigationService.NavigateToAsync<AchievementsViewModel>();
                        break;
                    case MenuViewIndex.SettingsView:
                        await NavigationService.NavigateToAsync<SettingsViewModel>();
                        break;
                    default:
                        break;
                }
            });
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await base.InitializeAsync(navigationData);

            if (MasterViewModel != null)
            {
                await MasterViewModel.InitializeAsync(navigationData).ConfigureAwait(false);
            }

            if (DetailViewModel != null)
            {
                await DetailViewModel.InitializeAsync(navigationData).ConfigureAwait(false);
            }
        }
    }
}