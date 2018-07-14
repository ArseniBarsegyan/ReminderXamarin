using ReminderXamarin.Helpers;
using SQLite;

namespace ReminderXamarin.Models
{
    [Table(ConstantHelper.Users)]
    public class UserModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[] ImageContent { get; set; }
    }
}