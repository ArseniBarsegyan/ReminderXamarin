using System.Threading.Tasks;

using ReminderXamarin.ViewModels.Base;

using Xamarin.Forms.Internals;

namespace ReminderXamarin.Services.Navigation
{
    [Preserve(AllMembers = true)]
    public interface INavigationService
    {
        BaseViewModel PreviousPageViewModel { get; }
        Task InitializeAsync<TViewModel>() where TViewModel : BaseViewModel;
        Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel;
        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel;
        Task NavigateToPopupAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel;
        Task RemoveLastFromBackStackAsync();
        Task RemoveBackStackAsync();
        Task NavigateToRootAsync();
        Task NavigateBackAsync();
        Task NavigatePopupBackAsync();
    }
}