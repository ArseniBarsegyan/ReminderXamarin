using System.Threading.Tasks;

using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.Services.Navigation
{
    public interface INavigationService
    {
        Task ToRootAsync<TViewModel>() where TViewModel : BaseNavigableViewModel;
        Task NavigateToDetails<TViewModel>(object parameter = null) where TViewModel : BaseNavigableViewModel;
        Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : BaseNavigableViewModel;        
        Task NavigateToPopupAsync<TViewModel>(object parameter = null) where TViewModel : BaseNavigableViewModel;
        Task NavigateBackAsync();
        Task NavigatePopupBackAsync();
    }
}