using Rm.Data.Core;

namespace Rm.Data.Entities
{
    public class PhotoModel : Entity
    {
        public string ResizedPath { get; set; }
        public string Thumbnail { get; set; }
        public bool Landscape { get; set; }
        public bool IsVideo { get; set; }

        public int NoteId { get; set; }
        public Note Note { get; set; }
    }
}