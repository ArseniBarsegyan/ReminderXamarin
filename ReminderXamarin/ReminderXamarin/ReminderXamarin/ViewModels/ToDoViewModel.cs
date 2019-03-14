using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Helpers;
using ReminderXamarin.Models;
using ReminderXamarin.ViewModels.Base;
using ReminderXamarin.Data.Entities;
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

        public Guid Id { get; set; }
        public IList<string> AvailablePriorities => 
            Enum.GetNames(typeof(ToDoPriority)).Select(x => x.ToString()).ToList();

        public ToDoPriority Priority { get; set; } = ToDoPriority.High;
        public string Description { get; set; }
        public DateTime WhenHappens { get; set; }

        public ICommand CreateToDoCommand { get; set; }
        public ICommand UpdateItemCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }

        private async Task CreateToDoCommandExecute()
        {
            var model = new ToDoModel
            {
                Description = Description,
                Priority = Priority.ToString(),
                WhenHappens = WhenHappens,
                UserId = Settings.CurrentUserId
            };

            await ToDoRepository.Value.CreateAsync(model);
            await ToDoRepository.Value.SaveAsync();
        }

        private async Task UpdateItemCommandExecute()
        {
            var model = await ToDoRepository.Value.GetByIdAsync(Id);
            model.Description = Description;
            model.Priority = Priority.ToString();
            model.UserId = Settings.CurrentUserId;
            model.WhenHappens = WhenHappens;
           
            ToDoRepository.Value.Update(model);
            await ToDoRepository.Value.SaveAsync();
        }

        private async Task DeleteItemCommandExecute()
        {
            await ToDoRepository.Value.DeleteAsync(Id);
            await ToDoRepository.Value.SaveAsync();
        }
    }
}