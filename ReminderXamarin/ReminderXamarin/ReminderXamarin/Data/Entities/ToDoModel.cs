using ReminderXamarin.Data.Core;
using System;

namespace ReminderXamarin.Data.Entities
{
    public class ToDoModel : Entity
    {
        public string Priority { get; set; }
        public string Description { get; set; }
        public DateTime WhenHappens { get; set; }

        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}