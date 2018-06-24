using System;
using SQLite;

namespace ReminderXamarin.Models
{
    [Table("ToDoModels")]
    public class ToDoModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public DateTime WhenHappens { get; set; }
    }
}