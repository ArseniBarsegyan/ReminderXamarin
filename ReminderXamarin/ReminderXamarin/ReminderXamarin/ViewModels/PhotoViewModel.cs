using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class PhotoViewModel : BaseViewModel
    {
        public PhotoViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public int Id { get; set; }
        public string ResizedPath { get; set; }
        public string Thumbnail { get; set; }
        public bool Landscape { get; set; }
        public bool IsVideo { get; set; }
        public int NoteId { get; set; }
    }
}