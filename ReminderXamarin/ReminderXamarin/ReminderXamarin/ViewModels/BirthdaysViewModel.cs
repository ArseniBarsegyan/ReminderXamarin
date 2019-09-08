﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using Rm.Helpers;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class BirthdaysViewModel : BaseViewModel
    {
        public BirthdaysViewModel()
        {
            BirthdayViewModels = new ObservableCollection<BirthdayViewModel>();
            RefreshListCommand = new Command(Refresh);
            NavigateToEditBirthdayCommand = new Command<int>(async id => await NavigateToEditView(id));
        }

        public bool IsRefreshing { get; set; }
        public ObservableCollection<BirthdayViewModel> BirthdayViewModels { get; set; }

        public ICommand NavigateToEditBirthdayCommand { get; set; }

        public ICommand RefreshListCommand { get; set; }

        public void OnAppearing()
        {
            LoadFriendsFromDatabase();
        }

        private void Refresh()
        {
            IsRefreshing = true;
            LoadFriendsFromDatabase();
            IsRefreshing = false;
        }

        private void LoadFriendsFromDatabase()
        {
            BirthdayViewModels = App.BirthdaysRepository.Value
                .GetAll()
                .Where(x => x.UserId == Settings.CurrentUserId)
                .ToFriendViewModels()
                .OrderByDescending(x => x.BirthDayDate)
                .ToObservableCollection();
        }

        private async Task NavigateToEditView(int id)
        {
            await NavigationService.NavigateToAsync<BirthdayEditViewModel>(id);
        }
    }
}