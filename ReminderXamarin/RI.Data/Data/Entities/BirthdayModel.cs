using System;
using Realms;
using RI.Data.Data.Core;

namespace RI.Data.Data.Entities
{
    public class BirthdayModel : RealmObject, IEntity
    {
        [PrimaryKey]
        public string Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string Name { get; set; }
        public DateTimeOffset BirthDayDate { get; set; }
        public string GiftDescription { get; set; }
        public string UserId { get; set; }
    }
}