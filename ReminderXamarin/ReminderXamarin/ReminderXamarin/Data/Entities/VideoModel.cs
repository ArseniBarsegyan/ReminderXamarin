using ReminderXamarin.Data.Core;
using System;

namespace ReminderXamarin.Data.Entities
{
    public class VideoModel : Entity
    {
        public string Path { get; set; }

        public Guid NoteId { get; set; }
        public Note Note { get; set; }
    }
}