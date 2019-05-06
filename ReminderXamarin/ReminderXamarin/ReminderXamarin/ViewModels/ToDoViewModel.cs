using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Enums;
using ReminderXamarin.ViewModels.Base;
using ReminderXamarin.Extensions;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class ToDoViewModel : BaseViewModel
    {
        public ToDoViewModel()
        {
            CreateToDoCommand = new Command(async () => await CreateToDo());
            UpdateItemCommand = new Command(async () => await UpdateItem());
            DeleteItemCommand = new Command(async result => await DeleteItem());
        }

        public int Id { get; set; }
        public IList<string> AvailableStatuses => 
            Enum.GetNames(typeof(ToDoStatus)).Select(x => x.ToString()).ToList();

        public ToDoStatus Status { get; set; } = ToDoStatus.Active;
        public string Description { get; set; }
        public DateTime WhenHappens { get; set; }

        public ICommand CreateToDoCommand { get; set; }
        public ICommand UpdateItemCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }

        private async Task CreateToDo()
        {
            App.ToDoRepository.Save(this.ToToDoModel());
        }

        private async Task UpdateItem()
        {
            // Update edit date since user pressed confirm
            App.ToDoRepository.Save(this.ToToDoModel());
        }

        private async Task DeleteItem()
        {
            App.ToDoRepository.DeleteModel(this.ToToDoModel());
        }
    }
}