using System;
using SQLite;

namespace ReminderXamarin.Models
{
    [Table("ToDoModels")]
    public class ToDoModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public ToDoPriority Priority { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
    }
}