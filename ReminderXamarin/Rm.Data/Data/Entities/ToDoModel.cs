using Rm.Helpers;

using SQLite;

using SQLiteNetExtensions.Attributes;

using System;

namespace Rm.Data.Data.Entities
{
    [Table(ConstantsHelper.ToDoModels)]
    public class ToDoModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime WhenHappens { get; set; }

        [ForeignKey(typeof(UserModel))]
        public string UserId { get; set; }
    }
}