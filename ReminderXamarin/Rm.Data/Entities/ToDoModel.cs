using System;

namespace Rm.Data.Entities
{
    public class ToDoModel
    {
        public string Priority { get; set; }
        public string Description { get; set; }
        public DateTime WhenHappens { get; set; }

        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}