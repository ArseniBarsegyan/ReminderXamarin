using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Enums;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;
using Rm.Data.Data.Repositories;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class ToDoViewModel : BaseViewModel
    {
        private ToDoRepository ToDoRepository => App.ToDoRepository.Value;
        
        public ToDoViewModel(ICommandResolver commandResolver)
        {
            CreateToDoCommand = commandResolver.Command(CreateToDo);
            UpdateItemCommand = commandResolver.Command(UpdateItem);
            DeleteItemCommand = commandResolver.Command(DeleteItem);
        }

        public int Id { get; set; }
        public IList<string> AvailableStatuses => 
            Enum.GetNames(typeof(ToDoStatus)).Select(x => x.ToString()).ToList();

        public ToDoStatus Status { get; set; } = ToDoStatus.Active;
        public string Description { get; set; }
        public DateTime WhenHappens { get; set; }

        public ICommand CreateToDoCommand { get; }
        public ICommand UpdateItemCommand { get; }
        public ICommand DeleteItemCommand { get; }

        private void CreateToDo()
        {
            ToDoRepository.Save(this.ToToDoModel());
        }

        private void UpdateItem()
        {
            // Update edit date since user pressed confirm
            ToDoRepository.Save(this.ToToDoModel());
        }

        private void DeleteItem()
        {
            ToDoRepository.DeleteModel(this.ToToDoModel());
        }
    }
}