using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class AchievementViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double GeneralTimeSpent { get; set; }
    }
}