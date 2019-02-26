using System;
using Rm.Data.Core;

namespace Rm.Data.Entities
{
    public class VideoModel : Entity
    {
        public string Path { get; set; }

        public Guid NoteId { get; set; }
        public Note Note { get; set; }
    }
}