using System;

namespace ReminderXamarin.ViewModels
{
    public class FriendViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string Name { get; set; }
        public DateTime BirthDayDate { get; set; }
        public string GiftDescription { get; set; }
    }
}