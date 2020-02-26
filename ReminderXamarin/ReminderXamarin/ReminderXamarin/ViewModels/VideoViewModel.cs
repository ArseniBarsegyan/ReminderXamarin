using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class VideoViewModel : BaseViewModel
    {
        public VideoViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public int Id { get; set; }
        public string Path { get; set; }
        public int NoteId { get; set; }
    }
}