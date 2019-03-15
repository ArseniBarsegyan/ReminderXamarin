using System;
using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class AchievementNoteViewModel : BaseViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public int HoursSpent { get; set; }
        public DateTime Date { get; set; }
        public string AchievementId { get; set; }
    }
}