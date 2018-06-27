using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReminderXamarin.Models
{
    [Table("Achievements")]
    public class AchievementModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public int GeneralTimeSpent { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<AchievementNote> AchievementNotes { get; set; }
    }
}