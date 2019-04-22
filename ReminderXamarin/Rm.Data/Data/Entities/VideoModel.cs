using Rm.Helpers;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Rm.Data.Data.Entities
{
    /// <summary>
    /// Store filepath to videos.
    /// </summary>
    [Table(ConstantsHelper.Videos)]
    public class VideoModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Path { get; set; }

        [ForeignKey(typeof(Note))]
        public int NoteId { get; set; }
    }
}