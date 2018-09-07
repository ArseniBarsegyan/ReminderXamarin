using System;

namespace ReminderXamarin.EF.Models
{
    public class ToDoModel : Entity
    {
        public string Priority { get; set; }
        public string Description { get; set; }
        public DateTime WhenHappens { get; set; }

        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}
