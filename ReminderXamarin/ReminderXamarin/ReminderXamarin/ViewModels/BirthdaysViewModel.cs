using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class BirthdaysViewModel : BaseNavigableViewModel
    {
        public BirthdaysViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
        }
    }
}