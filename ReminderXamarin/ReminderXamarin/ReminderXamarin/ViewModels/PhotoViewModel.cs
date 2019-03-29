using System;
using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class PhotoViewModel : BaseViewModel
    {
        public Guid Id { get; set; }
        public string ResizedPath { get; set; }
        public string Thumbnail { get; set; }
        public bool Landscape { get; set; }
        public bool IsVideo { get; set; }
        public Guid NoteId { get; set; }
    }
}