using Rm.Data.Core;

namespace Rm.Data.Entities
{
    public class VideoModel : Entity
    {
        public string Path { get; set; }

        public int NoteId { get; set; }
        public Note Note { get; set; }
    }
}