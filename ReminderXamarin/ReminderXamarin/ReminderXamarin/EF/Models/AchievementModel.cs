using System.Collections.Generic;

namespace ReminderXamarin.EF.Models
{
    public class AchievementModel : Entity
    {
        public byte[] ImageContent { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public int GeneralTimeSpent { get; set; }

        public int UserId { get; set; }
        public UserModel User { get; set; }        

        public IEnumerable<AchievementNoteModel> AchievementNotes { get; set; }
    }
}
