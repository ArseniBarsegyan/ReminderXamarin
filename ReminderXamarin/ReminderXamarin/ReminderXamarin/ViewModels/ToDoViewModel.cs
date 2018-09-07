using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Models;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class ToDoViewModel : BaseViewModel
    {
        public ToDoViewModel()
        {
            CreateToDoCommand = new Command(CreateToDoCommandExecute);
            UpdateItemCommand = new Command(UpdateItemCommandExecute);
            DeleteItemCommand = new Command(result => DeleteItemCommandExecute());
        }

        public int Id { get; set; }
        public IList<string> AvailablePriorities => 
            Enum.GetNames(typeof(ToDoPriority)).Select(x => x.ToString()).ToList();

        public ToDoPriority Priority { get; set; } = ToDoPriority.High;
        public string Description { get; set; }
        public DateTime WhenHappens { get; set; }

        public ICommand CreateToDoCommand { get; set; }
        public ICommand UpdateItemCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }

        private async void CreateToDoCommandExecute()
        {
            await App.ToDoRepository.CreateAsync(this.ToToDoModel());
            await App.ToDoRepository.SaveAsync();
        }

        private async void UpdateItemCommandExecute()
        {
            // Update edit date since user pressed confirm
            App.ToDoRepository.Update(this.ToToDoModel());
            await App.ToDoRepository.SaveAsync();
        }

        private async void DeleteItemCommandExecute()
        {
            await App.ToDoRepository.DeleteAsync(Id);
            await App.ToDoRepository.SaveAsync();
        }
    }
}