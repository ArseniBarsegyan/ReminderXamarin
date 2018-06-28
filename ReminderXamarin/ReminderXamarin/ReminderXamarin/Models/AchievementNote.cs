using System;
using ReminderXamarin.Helpers;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReminderXamarin.Models
{
    [Table(ConstantHelper.AchievementNotes)]
    public class AchievementNote
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public double HoursSpent { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey(typeof(AchievementModel))]
        public int AchievementId { get; set; }
    }
}