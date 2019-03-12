using ReminderXamarin.Data.Core;
using System;

namespace ReminderXamarin.Data.Entities
{
    public class BirthdayModel : Entity
    {
        public byte[] ImageContent { get; set; }
        public string Name { get; set; }
        public DateTime BirthDayDate { get; set; }
        public string GiftDescription { get; set; }

        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}