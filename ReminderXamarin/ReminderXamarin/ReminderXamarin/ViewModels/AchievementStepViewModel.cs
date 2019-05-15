using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class AchievementStepViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public byte[] Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TimeSpent { get; set; }
        public int TimeEstimation { get; set; }
        public float Progress => (TimeSpent / (float)TimeEstimation);

        public int AchievementId { get; set; }
    }
}