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
                        NavigationService.NavigateToAsync<NoteViewModel>();
                        break;
                    case MenuViewIndex.ToDoPage:
                        NavigationService.NavigateToAsync<ToDoViewModel>();
                        break;
                    case MenuViewIndex.BirthdaysView:
                        NavigationService.NavigateToAsync<BirthdaysViewViewModel>();
                        break;
                    case MenuViewIndex.AchievementsView:
                        NavigationService.NavigateToAsync<AchievementsViewViewModel>();
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