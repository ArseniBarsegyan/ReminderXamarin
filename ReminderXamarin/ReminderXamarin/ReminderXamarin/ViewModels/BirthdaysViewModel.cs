using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class BirthdaysViewModel : BaseNavigableViewModel
    {
        public BirthdaysViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
        }
    }
}