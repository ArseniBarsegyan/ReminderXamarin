using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class MenuDetailsViewModel : BaseViewModel
    {
        public MenuDetailsViewModel()
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
    }
}