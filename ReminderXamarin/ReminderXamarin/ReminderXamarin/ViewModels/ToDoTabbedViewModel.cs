﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using ReminderXamarin.Models;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class ToDoTabbedViewModel : MenuDetailsViewModel
    {
        public ToDoTabbedViewModel() : base()
        {
            HighPriorityModels = new ObservableCollection<ToDoViewModel>();
            MidPriorityModels = new ObservableCollection<ToDoViewModel>();
            LowPriorityModels = new ObservableCollection<ToDoViewModel>();

            RefreshListCommand = new Command(RefreshCommandExecute);
            SelectItemCommand = new Command<int>(async id => await SelectItemCommandExecute(id));
        }

        public void OnAppearing()
        {
            LoadModelsFromDatabase();
        }

        public bool IsLoading { get; set; }
        public bool IsRefreshing { get; set; }

        public ObservableCollection<ToDoViewModel> HighPriorityModels { get; set; }
        public ObservableCollection<ToDoViewModel> MidPriorityModels { get; set; }
        public ObservableCollection<ToDoViewModel> LowPriorityModels { get; set; }

        public ICommand RefreshListCommand { get; set; }
        public ICommand ShowDetailsCommand { get; set; }
        public ICommand SelectItemCommand { get; set; }

        private void RefreshCommandExecute()
        {
            IsRefreshing = true;
            LoadModelsFromDatabase();
            IsRefreshing = false;
        }

        private async Task<ToDoViewModel> SelectItemCommandExecute(int id)
        {
            var model = await ToDoRepository.Value.GetByIdAsync(id);
            return model.ToToDoViewModel();
        }

        private void LoadModelsFromDatabase()
        {
            var allModels = ToDoRepository
                .Value
                .GetAll()
                .Where(x => x.UserId == Settings.CurrentUserId)
                .ToToDoViewModels();

            HighPriorityModels = allModels.Where(x => x.Priority == ToDoPriority.High)
                .OrderByDescending(x => x.WhenHappens)
                .ToObservableCollection();

            MidPriorityModels = allModels.Where(x => x.Priority == ToDoPriority.Medium)
                .OrderByDescending(x => x.WhenHappens)
                .ToObservableCollection();

            LowPriorityModels = allModels.Where(x => x.Priority == ToDoPriority.Low)
                .OrderByDescending(x => x.WhenHappens)
                .ToObservableCollection();
        }
    }
}