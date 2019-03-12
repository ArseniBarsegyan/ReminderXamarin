using ReminderXamarin.Data.Core;
using System;

namespace ReminderXamarin.Data.Entities
{
    public class AchievementNote : Entity
    {
        public string Description { get; set; }
        public int HoursSpent { get; set; }
        public DateTime Date { get; set; }

        public Guid? AchievementId { get; set; }
        public AchievementModel Achievement { get; set; }
    }
}