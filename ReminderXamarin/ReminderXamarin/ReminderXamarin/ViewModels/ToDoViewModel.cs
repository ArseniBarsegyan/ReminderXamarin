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
            CreateToDoCommand = new Command(async () => await CreateToDoCommandExecute());
            UpdateItemCommand = new Command(async () => await UpdateItemCommandExecute());
            DeleteItemCommand = new Command(async result => await DeleteItemCommandExecute());
        }

        public int Id { get; set; }
        public IList<string> AvailablePriorities => 
            Enum.GetNames(typeof(ToDoStatus)).Select(x => x.ToString()).ToList();

        public ToDoStatus Status { get; set; } = ToDoStatus.Active;
        public string Description { get; set; }
        public DateTime WhenHappens { get; set; }

        public ICommand CreateToDoCommand { get; set; }
        public ICommand UpdateItemCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }

        private async Task CreateToDoCommandExecute()
        {
            App.ToDoRepository.Save(this.ToToDoModel());
        }

        private async Task UpdateItemCommandExecute()
        {
            // Update edit date since user pressed confirm
            App.ToDoRepository.Save(this.ToToDoModel());
        }

        private async Task DeleteItemCommandExecute()
        {
            App.ToDoRepository.DeleteModel(this.ToToDoModel());
        }
    }
}