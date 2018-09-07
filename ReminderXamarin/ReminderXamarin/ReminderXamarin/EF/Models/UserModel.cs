using System.Collections.Generic;

namespace ReminderXamarin.EF.Models
{
    public class UserModel : Entity
    {
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public byte[] ImageContent { get; set; }
        
        public List<NoteModel> Notes { get; set; }
        public List<AchievementModel> Achievements { get; set; }
        public List<BirthdayModel> Birthdays { get; set; }
        public List<ToDoModel> ToDoModels { get; set; }
    }
}
