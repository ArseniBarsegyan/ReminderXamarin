//using System;
//using ReminderXamarin.Helpers;
//using SQLite;
//using SQLiteNetExtensions.Attributes;

//namespace ReminderXamarin.Models
//{
//    [Table(ConstantHelper.ToDoModels)]
//    public class ToDoModel
//    {
//        [PrimaryKey, AutoIncrement]
//        public int Id { get; set; }

//        public string Priority { get; set; }
//        public string Description { get; set; }
//        public DateTime WhenHappens { get; set; }

//        [ForeignKey(typeof(UserModel))]
//        public int UserId { get; set; }
//    }
//}