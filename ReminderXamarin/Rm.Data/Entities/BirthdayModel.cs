using System;
using Rm.Data.Core;

namespace Rm.Data.Entities
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