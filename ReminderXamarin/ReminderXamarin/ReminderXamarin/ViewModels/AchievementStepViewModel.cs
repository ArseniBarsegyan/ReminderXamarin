using System;
using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class AchievementStepViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double TimeSpent { get; set; }
        public DateTime AchievedDate { get; set; }
        public int AchievementId { get; set; }
    }
}