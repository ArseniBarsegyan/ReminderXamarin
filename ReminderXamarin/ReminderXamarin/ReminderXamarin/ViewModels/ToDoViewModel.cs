using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Enums;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderXamarin.ViewModels
{
    public class ToDoViewModel : BaseViewModel
    {
        public ToDoViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            CreateToDoCommand = commandResolver.AsyncCommand(CreateToDo);
            UpdateItemCommand = commandResolver.AsyncCommand(UpdateItem);
            DeleteItemCommand = commandResolver.AsyncCommand(DeleteItem);
        }

        public int Id { get; set; }
        public IList<string> AvailableStatuses => 
            Enum.GetNames(typeof(ToDoStatus)).Select(x => x.ToString()).ToList();

        public ToDoStatus Status { get; set; } = ToDoStatus.Active;
        public string Description { get; set; }
        public DateTime WhenHappens { get; set; }

        public IAsyncCommand CreateToDoCommand { get; }
        public IAsyncCommand UpdateItemCommand { get; }
        public IAsyncCommand DeleteItemCommand { get; }

        private async Task CreateToDo()
        {
            App.ToDoRepository.Value.Save(this.ToToDoModel());
        }

        private async Task UpdateItem()
        {
            // Update edit date since user pressed confirm
            App.ToDoRepository.Value.Save(this.ToToDoModel());
        }

        private async Task DeleteItem()
        {
            App.ToDoRepository.Value.DeleteModel(this.ToToDoModel());
        }
    }
}