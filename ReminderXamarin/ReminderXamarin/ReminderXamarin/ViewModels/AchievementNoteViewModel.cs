using System;

namespace ReminderXamarin.ViewModels
{
    public class AchievementNoteViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int HoursSpent { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int AchievementId { get; set; }
    }
}