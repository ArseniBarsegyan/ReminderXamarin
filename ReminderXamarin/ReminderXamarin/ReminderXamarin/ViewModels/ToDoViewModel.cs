using System;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class ToDoViewModel : BaseViewModel
    {
        public ToDoViewModel()
        {
            UpdateItemCommand = new Command<ToDoViewModel>(UpdateItemCommandExecute);
            DeleteItemCommand = new Command<ToDoViewModel>(item => DeleteItemCommandExecute(item));
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string WarningLevelImage { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }

        public ICommand UpdateItemCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }

        private void UpdateItemCommandExecute(ToDoViewModel viewModel)
        {
            // Update edit date since user pressed confirm
            viewModel.EditDate = DateTime.Now;
            App.ToDoRepository.Save(viewModel.ToToDoModel());
        }

        private int DeleteItemCommandExecute(ToDoViewModel viewModel)
        {
            return App.ToDoRepository.DeleteModel(viewModel.ToToDoModel());
        }
    }
}