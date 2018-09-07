using System;
using System.Collections.Generic;

namespace ReminderXamarin.EF.Models
{
    public class NoteModel : Entity
    {
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }

        public UserModel User { get; set; }
        public int UserId { get; set; }

        public IEnumerable<PhotoModel> Photos { get; set; }
        public IEnumerable<VideoModel> Videos { get; set; }
    }
}
