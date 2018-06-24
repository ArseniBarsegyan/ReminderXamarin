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
            CreateAchievementCommand = new Command<AchievementViewModel>(CreateAchievementCommandExecute);
            UpdateAchievementCommand = new Command<AchievementViewModel>(UpdateAchievementCommandExecute);
            DeleteAchievementCommand = new Command<AchievementViewModel>(viewModel => DeleteAchievementCommandExecute(viewModel));
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

        private void CreateAchievementCommandExecute(AchievementViewModel viewModel)
        {
            App.AchievementRepository.Save(viewModel.ToAchievementModel());
        }

        private void UpdateAchievementCommandExecute(AchievementViewModel viewModel)
        {
        }

        private int DeleteAchievementCommandExecute(AchievementViewModel viewModel)
        {
            return App.AchievementRepository.DeleteAchievement(viewModel.ToAchievementModel());
        }
    }
}