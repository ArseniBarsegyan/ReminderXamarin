using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class AchievementViewModel : BaseViewModel
    {
        public AchievementViewModel()
        {
            AchievementNotes = new ObservableCollection<AchievementNoteViewModel>();

            CreateAchievementCommand = new Command(CreateAchievementCommandExecute);
            UpdateAchievementCommand = new Command(UpdateAchievementCommandExecute);
            DeleteAchievementCommand = new Command(viewModel => DeleteAchievementCommandExecute());
        }

        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public int GeneralTimeSpent { get; set; }
        public ObservableCollection<AchievementNoteViewModel> AchievementNotes { get; set; }

        public ICommand CreateAchievementCommand { get; set; }
        public ICommand UpdateAchievementCommand { get; set; }
        public ICommand DeleteAchievementCommand { get; set; }

        // TODO - remove stub
        private void CreateAchievementCommandExecute()
        {
            AchievementNotes.Add(new AchievementNoteViewModel
            {
                Description = Title,
                HoursSpent = 8,
                From = DateTime.Now,
                To = DateTime.Now,
                AchievementId = Id
            });
            App.AchievementRepository.Save(this.ToAchievementModel());
        }

        private void UpdateAchievementCommandExecute()
        {
            App.AchievementRepository.Save(this.ToAchievementModel());
        }

        private int DeleteAchievementCommandExecute()
        {
            return App.AchievementRepository.DeleteAchievement(this.ToAchievementModel());
        }

        private void LoadAchievementNotesFromDataBase()
        {
        }
    }
}