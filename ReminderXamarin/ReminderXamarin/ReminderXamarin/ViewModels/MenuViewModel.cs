using System.Threading.Tasks;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public BaseViewModel MasterViewModel { get; set; }
        public BaseViewModel DetailViewModel { get; set; }

        public MenuViewModel()
        {
            MessagingCenter.Subscribe<MenuMasterViewModel, MenuViewIndex>(this, ConstantsHelper.DetailPageChanged, (sender, pageIndex) =>
            {
                switch (pageIndex)
                {
                    case MenuViewIndex.NotesView:
                        NavigationService.NavigateToAsync<NotesViewModel>();
                        break;
                    case MenuViewIndex.ToDoPage:
                        NavigationService.NavigateToAsync<ToDoTabbedViewModel>();
                        break;
                    case MenuViewIndex.BirthdaysView:
                        NavigationService.NavigateToAsync<BirthdaysViewModel>();
                        break;
                    case MenuViewIndex.AchievementsView:
                        NavigationService.NavigateToAsync<AchievementsViewModel>();
                        break;
                    case MenuViewIndex.SettingsView:
                        NavigationService.NavigateToAsync<SettingsViewModel>();
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
                await MasterViewModel.InitializeAsync(navigationData);
            }

            if (DetailViewModel != null)
            {
                await DetailViewModel.InitializeAsync(navigationData);
            }
        }
    }
}