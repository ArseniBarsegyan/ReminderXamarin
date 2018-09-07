namespace ReminderXamarin.EF.Models
{
    public class VideoModel : Entity
    {
        public string Path { get; set; }

        public int NoteId { get; set; }
        public NoteModel Note { get; set; }       
    }
}
