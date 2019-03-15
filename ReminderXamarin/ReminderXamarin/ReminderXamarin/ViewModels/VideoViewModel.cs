using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class VideoViewModel : BaseViewModel
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string NoteId { get; set; }
    }
}