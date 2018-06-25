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

            RefreshListCommand = new Command(RefreshCommandExecute);
            CreateAchievementCommand = new Command(CreateAchievementCommandExecute);
            UpdateAchievementCommand = new Command(UpdateAchievementCommandExecute);
            DeleteAchievementCommand = new Command(viewModel => DeleteAchievementCommandExecute());
        }

        public bool IsRefreshing { get; set; }
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public int GeneralTimeSpent { get; set; }
        public ObservableCollection<AchievementNoteViewModel> AchievementNotes { get; set; }

        public ICommand RefreshListCommand { get; set; }
        public ICommand CreateAchievementCommand { get; set; }
        public ICommand UpdateAchievementCommand { get; set; }
        public ICommand DeleteAchievementCommand { get; set; }

        public void OnAppearing()
        {
            LoadAchievementNotesFromDataBase();
        }
        private void RefreshCommandExecute()
        {
            IsRefreshing = true;
            LoadAchievementNotesFromDataBase();
            IsRefreshing = false;
        }

        // TODO - remove stub
        private void CreateAchievementCommandExecute()
        {
            AchievementNotes.Add(new AchievementNoteViewModel
            {
                Description = Title,
                HoursSpent = 0,
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
            AchievementNotes = App.AchievementRepository.GetAchievementAsync(Id)
                .AchievementNotes
                .ToAchievementNoteViewModels()
                .ToObservableCollection();
        }
    }
}