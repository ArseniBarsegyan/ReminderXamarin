using System;
using SQLite;

namespace ReminderXamarin.Models
{
    [Table("AchievementNotes")]
    public class AchievementNote
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public int HoursSpent { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}