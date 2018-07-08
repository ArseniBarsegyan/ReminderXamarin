using System;
using ReminderXamarin.Helpers;
using SQLite;

namespace ReminderXamarin.Models
{
    [Table(ConstantHelper.Friends)]
    public class FriendModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string Name { get; set; }
        public DateTime BirthDayDate { get; set; }
        public string GiftDescription { get; set; }
    }
}