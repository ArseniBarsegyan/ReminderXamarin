using System.Collections.Generic;

namespace ReminderXamarin.ViewModels
{
    public class AchievementViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public int GeneralTimeSpent { get; set; }
        public List<AchievementNoteViewModel> AchievementNotes { get; set; }
    }
}