using ReminderXamarin.Data.Core;
using System;

namespace ReminderXamarin.Data.Entities
{
    public class PhotoModel : Entity
    {
        public string ResizedPath { get; set; }
        public string Thumbnail { get; set; }
        public bool Landscape { get; set; }
        public bool IsVideo { get; set; }

        public Guid NoteId { get; set; }
        public Note Note { get; set; }
    }
}