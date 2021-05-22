using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Enums;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class ToDoViewModel : BaseNavigableViewModel
    {
        public ToDoViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
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
            App.ToDoRepository.Value.Save(this.ToToDoModel());
        }

        private void UpdateItem()
        {
            // Update edit date since user pressed confirm
            App.ToDoRepository.Value.Save(this.ToToDoModel());
        }

        private void DeleteItem()
        {
            App.ToDoRepository.Value.DeleteModel(this.ToToDoModel());
        }
    }
}