using System;

namespace ReminderXamarin.EF.Models
{
    public class BirthdayModel : Entity
    {
        public byte[] ImageContent { get; set; }
        public string Name { get; set; }
        public DateTime BirthDayDate { get; set; }
        public string GiftDescription { get; set; }

        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}
