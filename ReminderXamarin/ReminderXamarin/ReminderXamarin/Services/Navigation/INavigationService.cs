using System.Threading.Tasks;

using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.Services.Navigation
{
    public interface INavigationService
    {
        Task ToRootAsync<TViewModel>() where TViewModel : BaseViewModel;
        Task NavigateToDetails<TViewModel>(object parameter = null) where TViewModel : BaseViewModel;
        Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel;        
        Task NavigateToPopupAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel;
        Task NavigateBackAsync();
        Task NavigatePopupBackAsync();
    }
}