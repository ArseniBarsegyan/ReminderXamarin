using System.Collections.Generic;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class AchievementViewModel : BaseViewModel
    {
        public AchievementViewModel()
        {
            CreateAchievementCommand = new Command(CreateAchievementCommandExecute);
            UpdateAchievementCommand = new Command(UpdateAchievementCommandExecute);
            DeleteAchievementCommand = new Command(viewModel => DeleteAchievementCommandExecute());
        }

        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public int GeneralTimeSpent { get; set; }
        public List<AchievementNoteViewModel> AchievementNotes { get; set; }

        public ICommand CreateAchievementCommand { get; set; }
        public ICommand UpdateAchievementCommand { get; set; }
        public ICommand DeleteAchievementCommand { get; set; }

        private void CreateAchievementCommandExecute()
        {
            App.AchievementRepository.Save(this.ToAchievementModel());
        }

        private void UpdateAchievementCommandExecute()
        {
        }

        private int DeleteAchievementCommandExecute()
        {
            return App.AchievementRepository.DeleteAchievement(this.ToAchievementModel());
        }
    }
}