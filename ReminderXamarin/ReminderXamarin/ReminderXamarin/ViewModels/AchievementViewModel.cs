using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            CreateAchievementNoteCommand = new Command<AchievementNoteViewModel>(CreateAchievementNoteCommandExecute);
            UpdateAchievementCommand = new Command(UpdateAchievementCommandExecute);
            UpdateAchievementNoteCommand = new Command<AchievementNoteViewModel>(UpdateAchievementNoteCommandExecute);
            DeleteAchievementCommand = new Command(viewModel => DeleteAchievementCommandExecute());
            DeleteAchievementNoteCommand = new Command<AchievementNoteViewModel>(DeleteAchievementNoteCommandExecute);
        }

        public bool IsRefreshing { get; set; }
        public int Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public int GeneralTimeSpent { get; set; }
        public ObservableCollection<AchievementNoteViewModel> AchievementNotes { get; set; }

        public ICommand RefreshListCommand { get; set; }
        public ICommand CreateAchievementCommand { get; set; }
        public ICommand CreateAchievementNoteCommand { get; set; }
        public ICommand UpdateAchievementCommand { get; set; }
        public ICommand UpdateAchievementNoteCommand { get; set; }
        public ICommand DeleteAchievementCommand { get; set; }
        public ICommand DeleteAchievementNoteCommand { get; set; }

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
        private async void CreateAchievementCommandExecute()
        {
            AchievementNotes.Add(new AchievementNoteViewModel
            {
                Description = GeneralDescription,
                HoursSpent = GeneralTimeSpent,
                Date = DateTime.Now,
                AchievementId = Id
            });
            await App.AchievementRepository.CreateAsync(this.ToAchievementModel());
            await App.AchievementRepository.SaveAsync();
        }

        private void CreateAchievementNoteCommandExecute(AchievementNoteViewModel achievementNoteViewModel)
        {
            if (!AchievementNotes.Contains(achievementNoteViewModel))
            {
                AchievementNotes.Add(achievementNoteViewModel);
            }
            UpdateAchievementCommandExecute();
        }

        private async void UpdateAchievementCommandExecute()
        {
            GeneralTimeSpent = AchievementNotes.Sum(x => x.HoursSpent);

            App.AchievementRepository.Update(this.ToAchievementModel());
            await App.AchievementRepository.SaveAsync();
        }

        private void UpdateAchievementNoteCommandExecute(AchievementNoteViewModel achievementNoteViewModel)
        {
            var oldNote = AchievementNotes.FirstOrDefault(x => x.Id == achievementNoteViewModel.Id);
            int oldNoteIndex = AchievementNotes.IndexOf(oldNote);
            AchievementNotes.Insert(oldNoteIndex, achievementNoteViewModel);

            UpdateAchievementCommandExecute();
        }

        private async void DeleteAchievementCommandExecute()
        {
            await App.AchievementRepository.DeleteAsync(Id);
            await App.AchievementRepository.SaveAsync();
        }

        private void DeleteAchievementNoteCommandExecute(AchievementNoteViewModel noteViewModel)
        {
            if (AchievementNotes.Contains(noteViewModel))
            {
                AchievementNotes.Remove(noteViewModel);
            }
            UpdateAchievementCommandExecute();
        }

        private async void LoadAchievementNotesFromDataBase()
        {
            // Fetch all note models from database, order by recent date, then by recent upload.
            AchievementNotes = (await App.AchievementRepository.GetByIdAsync(Id))
                .AchievementNotes
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Id)
                .ToAchievementNoteViewModels()
                .ToObservableCollection();
        }
    }
}