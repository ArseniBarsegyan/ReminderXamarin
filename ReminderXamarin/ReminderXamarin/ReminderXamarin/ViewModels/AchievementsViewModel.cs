﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class AchievementsViewModel : BaseViewModel
    {
        public AchievementsViewModel()
        {
            Achievements = new ObservableCollection<AchievementViewModel>();

            RefreshListCommand = new Command(RefreshCommandExecute);
            SelectAchievementCommand = new Command<int>(async id => await SelectAchievementCommandExecute(id));
        }

        public bool IsRefreshing { get; set; }
        public ObservableCollection<AchievementViewModel> Achievements { get; set; }

        public ICommand RefreshListCommand { get; set; }
        public ICommand SelectAchievementCommand { get; set; }

        public void OnAppearing()
        {
            LoadAchievementsFromDatabase();
        }

        private void RefreshCommandExecute()
        {
            IsRefreshing = true;
            LoadAchievementsFromDatabase();
            IsRefreshing = false;
        }

        private async Task<AchievementViewModel> SelectAchievementCommandExecute(int id)
        {
            var achievement = await App.AchievementRepository.GetByIdAsync(id);
            return achievement.ToAchievementViewModel();
        }

        private void LoadAchievementsFromDatabase()
        {
            Achievements = App.AchievementRepository
                .GetAll(null, "AchievementNotes")
                .Where(x => x.UserId == Settings.CurrentUserId)
                .ToAchievementViewModels()
                .OrderByDescending(x => x.GeneralTimeSpent)
                .ToObservableCollection();
        }
    }
}