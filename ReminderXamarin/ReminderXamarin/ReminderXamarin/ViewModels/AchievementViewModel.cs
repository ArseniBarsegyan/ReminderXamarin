using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class AchievementViewModel : BaseNavigableViewModel
    {
        public AchievementViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double GeneralTimeSpent { get; set; }
    }
}