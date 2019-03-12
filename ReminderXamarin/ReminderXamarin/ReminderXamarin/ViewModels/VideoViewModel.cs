using ReminderXamarin.ViewModels.Base;
using System;

namespace ReminderXamarin.ViewModels
{
    public class VideoViewModel : BaseViewModel
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public Guid NoteId { get; set; }
    }
}