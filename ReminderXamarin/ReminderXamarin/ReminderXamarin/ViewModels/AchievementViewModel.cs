using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class AchievementViewModel : BaseViewModel
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