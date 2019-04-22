using System;
using Rm.Helpers;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Rm.Data.Data.Entities
{
    [Table(ConstantsHelper.AchievementNotes)]
    public class AchievementNote
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public int HoursSpent { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey(typeof(AchievementModel))]
        public int AchievementId { get; set; }
    }
}